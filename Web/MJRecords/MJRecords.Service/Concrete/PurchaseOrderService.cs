using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MJRecords.Model;
using MJRecords.Repository;
using MJRecords.Types;

namespace MJRecords.Service
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepo _repo;
        private readonly IPurchaseOrderItemRepo _itemRepo;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly IEmailService _emailService;

        public PurchaseOrderService(IPurchaseOrderRepo repo, IPurchaseOrderItemRepo itemRepo, IEmployeeRepo employeeRepo, IEmailService emailService)
        {
            _repo = repo;
            _itemRepo = itemRepo;
            _employeeRepo = employeeRepo;
            _emailService = emailService;
        }

        public async Task ClosePurchaseOrderAsync(string purchaseOrderId)
        {
            var po = await _repo.GetByIdAsync(purchaseOrderId);

            if (po == null)
            {
                throw new Exception("Purchase order not found.");
            }

            if (po.Status == PurchaseOrderStatusEnum.Closed)
            {
                return;
            }

            po.Status = PurchaseOrderStatusEnum.Closed;

            _repo.Update(po);

            var employee = _employeeRepo.GetAll().SingleOrDefault(p=>p.Id==po.EmployeeId);

            try
            {
                if (employee != null && !string.IsNullOrEmpty(employee.EmailAddress))
                {
                    await _emailService.SendAsync(new EmailMessage
                    {
                        To = employee.EmailAddress,
                        Subject = "Your Purchase Order Has Been Closed",
                        Body = $"Your purchase order #{po.PurchaseOrderId} has been processed and closed by your supervisor."
                    });
                }
            }
            catch
            {
                //do nothing here
            }
        }


        public async Task<List<PurchaseOrder>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<PurchaseOrder?> GetByIdAsync(string id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<List<POSByDepartmentSummaryDto>> GetPOsByDepartment(int departmentId)
        {
            return await _repo.GetPOsByDepartment(departmentId);
        }

        public List<POSummaryDto> GetSummary()
        {
            var pos = _repo.GetAll();
            var result = new List<POSummaryDto>();

            foreach (var po in pos)
            {
                var subTotal = CalculateSubTotal(po);
                var tax = CalculateTaxTotal(subTotal);
                var grand = CalculateGrandTotal(subTotal, tax);

                result.Add(new POSummaryDto
                {
                    PurchaseOrderId = po.PurchaseOrderId,
                    DateCreated = po.DateCreated,
                    Status = po.Status,
                    Employee = po.EmployeeId,
                    Subtotal = subTotal,
                    TaxTotal = tax,
                    GrandTotal = grand
                });
            }

            return result;
        }



        public decimal CalculateSubTotal(PurchaseOrder po)
        {
            return _repo.GetItemsByOrderId(po.PurchaseOrderId)
                        .Sum(item => item.Price * item.Quantity);
        }

        public decimal CalculateTaxTotal(decimal subTotal)
        {
            const decimal TaxRate = 0.15m;
            return Math.Round(subTotal * TaxRate, 2);
        }

        public decimal CalculateGrandTotal(decimal subTotal, decimal tax)
        {
            return Math.Round(subTotal + tax, 2);
        }

        private void ValidateModel(PurchaseOrder p, PurchaseOrderItem pI)
        {
            List<ValidationResult> results = new();
            Validator.TryValidateObject(p, new ValidationContext(p), results, true);

            foreach (ValidationResult e in results)
            {
                p.AddError(new(e.ErrorMessage, ErrorType.Model));
            }
        }

        public async Task<PurchaseOrder> CreateAsync(POCreateDto dto)
        {
            if (dto.Item == null)
                throw new InvalidOperationException("A purchase order must include an item.");

            var po = new PurchaseOrder
            {
                EmployeeId = dto.EmployeeId,
                Status = PurchaseOrderStatusEnum.Pending,
                DateCreated = DateTime.Now
            };

            var createdPo = await _repo.CreateAsync(po, dto.Item);

            return createdPo;

        }



        private bool Validate(PurchaseOrder po, PurchaseOrderItem item)
        {
            List<ValidationResult> results = new();

            Validator.TryValidateObject(po, new ValidationContext(po), results, true);

            Validator.TryValidateObject(item, new ValidationContext(item), results, true);

            

            foreach (var e in results)
            {
                if (e.MemberNames.Any(x => x.Equals("PurchaseOrderId")))
                    continue;

                po.AddError(new (e.ErrorMessage, ErrorType.Model));
            }

            return po.Errors.Count == 0;
        }

        public async Task<List<POSummaryDto>> SearchSummariesAsync(
            POSearchDto criteria, 
            bool includeDepartment = false, 
            bool isCeo = false,
            string? employeeId = null)
        {
            var pos = await _repo.SearchByCriteriaAsync(criteria, includeDepartment, isCeo, employeeId);
            var employees = await _employeeRepo.GetAllAsync();

            var employeeMap = employees.ToDictionary(
                e => e.Id,
                e => $"{e.FirstName} {e.LastName}"
            );
            var result = new List<POSummaryDto>();

            foreach (var po in pos)
            {
                var items = await _itemRepo.GetItemsByPurchaseOrderIdAsync(po.PurchaseOrderId);
                var subtotal = items.Sum(i => i.Price * i.Quantity);
                var tax = Math.Round(subtotal * 0.15m, 2);
                var grand = subtotal + tax;
                var fullName = employeeMap.ContainsKey(po.EmployeeId)
                    ? employeeMap[po.EmployeeId]
                         : "Unknown";

                result.Add(new POSummaryDto
                {
                    PurchaseOrderId = po.PurchaseOrderId,
                    DateCreated = po.DateCreated,
                    Status = po.Status,
                    Employee = po.EmployeeId,
                    EmployeeName = fullName,
                    Subtotal = subtotal,
                    TaxTotal = tax,
                    GrandTotal = grand
                });
            }

            return result;
        }

        public List<MonthlyExpenseDto> GetMonthlyExpensesForSupervisor(string supervisorId)
        {
            return _repo.GetMonthlyExpensesForSupervisor(supervisorId);
        }

        public List<MonthlyExpenseDto> GetMonthlyExpensesForEmployee(string employeeId)
        {
            return _repo.GetMonthlyExpensesForEmployee(employeeId);
        }

        public async Task<List<POSByDepartmentSummaryDto>> GetPendingAndUnderReviewPOsByDepartmentAsync(int departmentId, string supervisorId)

        {
            var allPOs = await GetPOsByDepartment(departmentId);

            var pendingStatus = PurchaseOrderStatusEnum.Pending;
            var underReviewStatus = PurchaseOrderStatusEnum.UnderReview;

            var filteredPOs = allPOs
                .Where(po => (po.Status == pendingStatus || po.Status == underReviewStatus) && po.EmployeeId != supervisorId)
                .ToList();

            return filteredPOs;
        }


        public async Task<POSummaryApiDto?> GetPurchaseOrderSummaryApiAsync(string poId)
        {
            if (string.IsNullOrWhiteSpace(poId))
                throw new ArgumentException("Purchase Order ID must be provided.", nameof(poId));

            var summary = await _repo.GetPurchaseOrderSummaryApiAsync(poId);

            return summary;
        }

    }
}

