
using MJRecords.Model;

namespace MJRecords.Repository
{
    public interface IPurchaseOrderRepo
    {

        Task<PurchaseOrder?> GetByIdAsync(string id);

        List<PurchaseOrder> GetAll();

        Task<List<POSByDepartmentSummaryDto>> GetPOsByDepartment(int departmentId);

        Task<List<PurchaseOrder>> GetAllAsync();
        PurchaseOrder? GetById(string id);

        List<PurchaseOrderItem> GetItemsByOrderId(string OrderId);

        Task<PurchaseOrder> CreateAsync(PurchaseOrder po, ItemCreateDto item);

        Task<List<PurchaseOrder>> SearchByCriteriaAsync(
            POSearchDto criteria, 
            bool includeDepartment = false,
            bool isSupervisor = false,
            string employeeId = null
            );


        void Update(PurchaseOrder po);
        void Delete(string id);

        List<MonthlyExpenseDto> GetMonthlyExpensesForSupervisor(string supervisorId);

        List<MonthlyExpenseDto> GetMonthlyExpensesForEmployee(string employeeId);

        Task<POSummaryApiDto?> GetPurchaseOrderSummaryApiAsync(string poId);

    }

}

