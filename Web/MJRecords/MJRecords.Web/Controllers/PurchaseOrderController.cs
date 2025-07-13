using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using MJRecords.Model;
using MJRecords.Model.ViewModels;
using MJRecords.Service;
using MJRecords.Types;
using MJRecords.Web.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MJRecords.Web.Controllers
{
    public class PurchaseOrderController : Controller
    {
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IPurchaseOrderItemService _purchaseOrderItemService;
        private readonly IEmployeeService _employeeService;
        private readonly ILoginService _loginService;
        private readonly IDepartmentService _departmentService;

        public PurchaseOrderController(
            IPurchaseOrderService purchaseOrderService,
            IPurchaseOrderItemService purchaseOrderItemService,
            IEmployeeService employeeService,
            ILoginService loginService,
            IDepartmentService departmentService)
        {
            _purchaseOrderService = purchaseOrderService;
            _purchaseOrderItemService = purchaseOrderItemService;
            _employeeService = employeeService;
            _loginService = loginService;
            _departmentService = departmentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(POSearchViewModel vm)
        {

            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            var loggedInUserId = loggedInUser.Id;
            var loggedInUserRole = loggedInUser.Role;

            if (vm.Criteria.StartDate.HasValue && vm.Criteria.EndDate.HasValue && vm.Criteria.EndDate < vm.Criteria.StartDate)
            {
                vm.Criteria.StartDate = null;
                vm.Criteria.EndDate = null;
                ViewBag.EndDateError = "Start date cannot be after end date.";
            }
            else
            {
                ViewBag.EndDateError = null;
            }

            if (loggedInUserRole == "Regular Supervisor" || loggedInUserRole == "HR Supervisor")
            {
                var resultsSupervisor = await _purchaseOrderService.SearchSummariesAsync(
                    vm.Criteria, 
                    true, 
                    false,
                    loggedInUserId );
                vm.Results = resultsSupervisor;

            } else if(loggedInUserRole == "CEO")
            {
                var resultsCeo = await _purchaseOrderService.SearchSummariesAsync(
                     vm.Criteria,
                     true,
                     true,
                     loggedInUserId);

                vm.Results = resultsCeo;
            }
            else
            {
                var resultsRegular = await _purchaseOrderService.SearchSummariesAsync(
                    vm.Criteria, 
                    false, 
                    false,
                    loggedInUserId);

                vm.Results = resultsRegular;

            }

            return View(vm);

        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            string employeeId = loggedInUser.Id;

            var employee = await _employeeService.GetEmployeeDetailsAsync(employeeId);

            var viewModel = new POCreateViewModel
            {
                NewItem = new ItemCreateDto(),
                Form = new POCreateDto { EmployeeId = employeeId },
                ExistingItems = new List<PurchaseOrderItem>(),
                Subtotal = 0,
                TaxTotal = 0,
                GrandTotal = 0,


                EmployeeFullName = employee?.FullName ?? "Unknown",
                EmployeeDepartment = string.IsNullOrWhiteSpace(employee?.Department) ? "No Department" : employee.Department,
                EmployeeSupervisor = string.IsNullOrWhiteSpace(employee?.SupervisorFullName) ? "No Supervisor" : employee.SupervisorFullName
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(POCreateViewModel vm)
        {
            string poId;
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");
            ViewBag.LoggedInUserId = loggedInUser.Id;

            if (string.IsNullOrEmpty(vm.PurchaseOrderId))
            {

                vm.Form ??= new POCreateDto();
                vm.Form.EmployeeId = loggedInUser.Id;
                vm.Form.Item = vm.NewItem;

                var createdPo = await _purchaseOrderService.CreateAsync(vm.Form);
                poId = createdPo.PurchaseOrderId;

                if (createdPo.Errors?.Count > 0)
                {
                    vm.ExistingItems = new List<PurchaseOrderItem>();
                    return View(vm);
                }
            }
            else
            {
                poId = vm.PurchaseOrderId;
                await _purchaseOrderItemService.AddItemToPurchaseOrderAsync(poId, vm.NewItem);
            }

            var items = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(poId);
            var (subtotal, tax, grandTotal) = await _purchaseOrderItemService.GetPurchaseOrderTotalsAsync(poId);

            string employeeId = loggedInUser.Id;
            var employee = await _employeeService.GetEmployeeDetailsAsync(employeeId);

            var newVm = new POCreateViewModel
            {
                PurchaseOrderId = poId.ToString(),
                ExistingItems = items,
                NewItem = new ItemCreateDto(),
                Form = new POCreateDto
                {
                    EmployeeId = loggedInUser.Id
                },
                Subtotal = subtotal,
                TaxTotal = tax,
                GrandTotal = grandTotal,
                EmployeeFullName = employee?.FullName ?? "Unknown",
                EmployeeDepartment = string.IsNullOrWhiteSpace(employee?.Department) ? "No Department" : employee.Department,
                EmployeeSupervisor = string.IsNullOrWhiteSpace(employee?.SupervisorFullName) ? "No Supervisor" : employee.SupervisorFullName
            };

            ModelState.Clear();

            ViewBag.SuccessMessage = "✅ Item added to the purchase order! You can add more items if you wish.";
            return View(newVm);
        }


        [HttpPost]
        public async Task<IActionResult> Details(PODetailsViewModel vmPO)
        {
            string poId;
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");
            ViewBag.LoggedInUserId = loggedInUser.Id;
            
            TempData["LoggedInUserRole"] = loggedInUser.Role;
            TempData["LoggedInUserId"] = loggedInUser.Id;

            poId = vmPO.PurchaseOrderId;
            var po = await _purchaseOrderService.GetByIdAsync(poId);


            try
            {
                await _purchaseOrderItemService.AddItemToPurchaseOrderAsync(poId, vmPO.NewItem);
                ViewBag.SuccessMessage = "✅ Item added to the purchase order! You can add more items if you wish.";
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewBag.ConcurrencyError = ex.Message;
            }



            TempData["PurchaseOwnerId"] = po.EmployeeId;

            var poOwner = await _employeeService.GetAsync(po.EmployeeId);
            var currentUser = await _employeeService.GetAsync(loggedInUser.Id);

            var empDetails = await _employeeService.GetEmployeeDetailsAsync(po.EmployeeId);
            var empDetailsSuper = await _employeeService.GetEmployeeDetailsAsync(loggedInUser.Id);

            var department = await _departmentService.GetAsync(poOwner.DepartmentId ?? 0);

            var supervisor = await _employeeService.GetAsync(poOwner.SupervisorId ?? null);

            TempData["SupervisorOfPoOwner"] = poOwner.SupervisorId;
            TempData["LoggedInUserDepartmentId"] = currentUser.DepartmentId;
            TempData["PurchaseOwnerDepartmentId"] = poOwner.DepartmentId;

            var supervisorFirstName = supervisor?.FirstName ?? "No supervisor";
            var supervisorLastName = supervisor?.LastName ?? "";

            var items = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(poId);
            var (subtotal, tax, grand) = await _purchaseOrderItemService.GetPurchaseOrderTotalsAsync(poId);

            var vm = new PODetailsViewModel
            {
                EmployeeFullName = empDetails.FullName,
                Supervisor = $"{supervisorFirstName} {supervisorLastName}",
                Department = department?.Name ?? "No Department",
                PurchaseOrder = po,
                Items = items,
                Subtotal = subtotal,
                TaxTotal = tax,
                GrandTotal = grand
            };

            List<PurchaseOrderItem> pendingItems = await VerifyPendings(po.PurchaseOrderId);

            if (pendingItems.Count == 0)
            {
                if (po.Status != PurchaseOrderStatusEnum.Closed)
                {
                    TempData["ShowClosePrompt"] = true;
                    TempData["PurchaseOrderId"] = po.PurchaseOrderId;
                }
            }


            ModelState.Clear();

            return View(vm);
        }



        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");
            ViewBag.LoggedInUserId = loggedInUser.Id;

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            TempData["LoggedInUserRole"] = loggedInUser.Role;
            TempData["LoggedInUserId"] = loggedInUser.Id;

            var po = await _purchaseOrderService.GetByIdAsync(id);
            if (po == null)
                return NotFound();

            var poOwner = await _employeeService.GetAsync(po.EmployeeId);
            var currentUser = await _employeeService.GetAsync(loggedInUser.Id);

            TempData["SupervisorOfPoOwner"] = poOwner.SupervisorId;
            TempData["PurchaseOwnerId"] = po.EmployeeId;

            TempData["LoggedInUserDepartmentId"] = currentUser.DepartmentId;
            TempData["PurchaseOwnerDepartmentId"] = poOwner.DepartmentId;

            bool isOwner = loggedInUser.Id == po.EmployeeId;

            if (loggedInUser.Role == "CEO" || loggedInUser.Role == "HR Supervisor" || loggedInUser.Role == "Regular Supervisor")
                isOwner = true;

            bool isSupervisor = RoleHelper.IsAllowed(
                loggedInUser.Role,
                AccessLevels.HRSupervisor,
                AccessLevels.RegularSupervisor,
                AccessLevels.CEO
            ) && currentUser.DepartmentId == poOwner.DepartmentId;

            if (!isOwner && !isSupervisor)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var empDetails = await _employeeService.GetEmployeeDetailsAsync(po.EmployeeId);
            var department = await _departmentService.GetAsync(poOwner.DepartmentId ?? 0);

            var supervisor = await _employeeService.GetAsync(poOwner.SupervisorId ?? null);

            var supervisorFirstName = supervisor?.FirstName ?? "No supervisor";
            var supervisorLastName = supervisor?.LastName ?? "";

            var items = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(id);
            var (subtotal, tax, grand) = await _purchaseOrderItemService.GetPurchaseOrderTotalsAsync(id);

            var vm = new PODetailsViewModel
            {
                EmployeeFullName = empDetails.FullName,
                Supervisor = $"{supervisorFirstName} {supervisorLastName}",
                Department = department?.Name ?? "No Department",
                PurchaseOrder = po,
                Items = items,
                Subtotal = subtotal,
                TaxTotal = tax,
                GrandTotal = grand,
                NewItem = new ItemCreateDto
                {
                    RecordVersion = po.RecordVersion
                }

            };

            List<PurchaseOrderItem> pendingItems = await VerifyPendings(po.PurchaseOrderId);

            if (pendingItems.Count == 0 &&
                ((loggedInUser.Id != poOwner.Id && 
                isSupervisor) || 
                loggedInUser.Role == "CEO"))
            {
                if (po.Status != PurchaseOrderStatusEnum.Closed)
                {
                    TempData["ShowClosePrompt"] = true;
                    TempData["PurchaseOrderId"] = po.PurchaseOrderId;
                }
            }
            else
            {
                TempData["ShowClosePrompt"] = false;
            }

            if (TempData["ConcurrencyError"] != null)
            {
                ViewBag.ConcurrencyError = TempData["ConcurrencyError"];
            }

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int itemId, string recordVersionBase64)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role))
            {
                return RedirectToAction("AccessDenied", "Error");
            }


            var item = await _purchaseOrderItemService.GetByIdAsync(itemId);
            var purchaseOrderId = item.PurchaseOrderId;

            if (item == null)
                return NotFound();

            var po = await _purchaseOrderService.GetByIdAsync(item.PurchaseOrderId);
            if (po == null)
                return NotFound();


            if (
                po.Status == PurchaseOrderStatusEnum.Closed
                || item.Status != PurchaseOrderItemStatusEnum.Pending
                )
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            if (po.EmployeeId != loggedInUser.Id)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var vm = new POItemEditViewModel
            {
                ItemId = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                Name = item.Name,
                Description = item.Description,
                Justification = item.Justification,
                PurchaseLocation = item.PurchaseLocation,
                Status = item.Status,
                Quantity = item.Quantity,
                Price = item.Price,
                RecordVersion  = Convert.FromBase64String(recordVersionBase64)
            };

            List<PurchaseOrderItem> pendingItems = await VerifyPendings(item.PurchaseOrderId);

            if (pendingItems.Count == 0)
            {
                TempData["ShowClosePrompt"] = true;
                TempData["PurchaseOrderId"] = item.PurchaseOrderId;
            }

            if (TempData["ConcurrencyError"] != null)
            {
                ViewBag.ConcurrencyError = TempData["ConcurrencyError"];
            }


            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(POItemEditViewModel vm)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            var updatedItem = new PurchaseOrderItem
            {
                Id = vm.ItemId,
                PurchaseOrderId = vm.PurchaseOrderId,
                Name = vm.Name,
                Description = vm.Description,
                Justification = vm.Justification,
                PurchaseLocation = vm.PurchaseLocation,
                Quantity = vm.Quantity,
                Price = vm.Price,
                Status = vm.Status,
                RecordVersion = vm.RecordVersion
            };

            var result = _purchaseOrderItemService.Update(updatedItem);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                    TempData["ConcurrencyError"] = TempData["ConcurrencyError"] + error.Description;
                }

                return RedirectToAction("Details", "PurchaseOrder", new { id = vm.PurchaseOrderId });

            }

            TempData["SuccessMessage"] = "✅ Item updated successfully!";
            return RedirectToAction("Details", "PurchaseOrder", new { id = vm.PurchaseOrderId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NoLongerNecessary(int id, string purchaseOrderId)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");
            var item = await _purchaseOrderItemService.GetByIdAsync(id);

            var base64Version = Request.Form["RecordVersionBase64"];
            var recordVersion = Convert.FromBase64String(base64Version);

            var po = await _purchaseOrderService.GetByIdAsync(item.PurchaseOrderId);

            if (po.EmployeeId != loggedInUser.Id)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            item.Description = "No longer needed";
            item.Quantity = 0;
            item.Price = 0;
            item.Status = PurchaseOrderItemStatusEnum.Denied;
            item.RecordVersion = recordVersion;

            var result = _purchaseOrderItemService.UpdateNoLongerNeeded(item);

            if (result.Errors.Any())
            {
                TempData["ConcurrencyError"] = result.Errors.First().Description;
                return RedirectToAction("Details", "PurchaseOrder", new { id = purchaseOrderId });
            }

            var pendingItems = await VerifyPendings(purchaseOrderId);

            if (pendingItems.Count == 0)
            {
                TempData["ShowClosePrompt"] = true;
                TempData["PurchaseOrderId"] = purchaseOrderId;
            }
            else
            {
                TempData["SuccessMessage"] = "Item no longer needed.";
            }

            return RedirectToAction("Details", "PurchaseOrder", new { id = purchaseOrderId });
        }


        private async Task<List<PurchaseOrderItem>> VerifyPendings(string purchaseOrderId)
        {
            var remainingItems = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(purchaseOrderId);
            var pendingItems = remainingItems.Where(i => i.Status == PurchaseOrderItemStatusEnum.Pending).ToList();
            return pendingItems;
        }

        public async Task<IActionResult> Deny(int id, string purchaseOrderId, string recordVersionBase64)
        {

            var item = await _purchaseOrderItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            var vm = new ConfirmDenyViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Quantity = item.Quantity,
                Price = item.Price,
                PurchaseOrderId = purchaseOrderId,
                RecordVersion = Convert.FromBase64String(recordVersionBase64),
                RecordVersionBase64 = recordVersionBase64
            };

            return View("ConfirmDeny", vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDeny(ConfirmDenyViewModel model)
        {
            var base64Version = Request.Form["RecordVersionBase64"];
            var recordVersion = Convert.FromBase64String(base64Version);

            if (!ModelState.IsValid)
            {
                var fullItem = await _purchaseOrderItemService.GetByIdAsync(model.Id);
                model.Name = fullItem?.Name ?? "";
                model.Description = fullItem?.Description ?? "";
                model.Price = fullItem?.Price ?? 0;
                model.Quantity = fullItem?.Quantity ?? 0;

                return View("ConfirmDeny", model);
            }

            var result = await _purchaseOrderItemService.SetItemStatusAsync(
                model.Id,
                "Denied",
                recordVersion,
                model.DenialReason
            );

            if (result == null)
            {
                TempData["ConcurrencyError"] = "Item not found.";
                return RedirectToAction("Details", "PurchaseOrder", new { id = model.PurchaseOrderId });
            }

            if (result.Errors.Any())
            {
                TempData["ConcurrencyError"] = result.Errors.First().Description;
                return RedirectToAction("Details", "PurchaseOrder", new { id = model.PurchaseOrderId });
            }

            List<PurchaseOrderItem> pendingItems = await VerifyPendings(model.PurchaseOrderId);

            if (pendingItems.Count == 0)
            {
                TempData["ShowClosePrompt"] = true;
                TempData["PurchaseOrderId"] = model.PurchaseOrderId;
            }
            else
            {
                TempData["SuccessMessage"] = "Item denied successfully.";
            }

            return RedirectToAction("Details", "PurchaseOrder", new { id = model.PurchaseOrderId });
        }




        [HttpPost]
        public async Task<IActionResult> Close(string purchaseOrderId)
        {
            try
            {
                var items = await _purchaseOrderItemService.GetByPurchaseOrderIdAsync(purchaseOrderId);
                bool hasPendingItems = items.Any(i => i.Status == PurchaseOrderItemStatusEnum.Pending);

                if (hasPendingItems)
                {
                    TempData["ConcurrencyError"] = "A new item was added and is still pending. You cannot close this PO.";
                    return RedirectToAction("Details", new { id = purchaseOrderId });
                }

                await _purchaseOrderService.ClosePurchaseOrderAsync(purchaseOrderId);
                TempData["SuccessMessage"] = "Purchase Order closed successfully.";
            }
            catch (ValidationException ex)
            {
                TempData["ConcurrencyError"] = ex.Message;
            }

            return RedirectToAction("Details", new { id = purchaseOrderId });
        }




        [HttpPost]
        public async Task<IActionResult> Approve(int id, string purchaseOrderId)
        {
            var base64Version = Request.Form["RecordVersionBase64"];
            var recordVersion = Convert.FromBase64String(base64Version);

            try
            {
                var result = await _purchaseOrderItemService.SetItemStatusAsync(id, "Approved", recordVersion);

                if (result.Errors.Any())
                {
                    TempData["ConcurrencyError"] = result.Errors.First().Description;
                }
                else
                {
                    List<PurchaseOrderItem> pendingItems = await VerifyPendings(purchaseOrderId);

                    if (pendingItems.Count == 0)
                    {
                        TempData["ShowClosePrompt"] = true;
                        TempData["PurchaseOrderId"] = purchaseOrderId;
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Item approved successfully.";
                    }
                }
            }
            catch (ValidationException ex)
            {
                TempData["ConcurrencyError"] = ex.Message;
            }

            return RedirectToAction("Details", "PurchaseOrder", new { id = purchaseOrderId });
        }



        [HttpPost]
        public async Task<IActionResult> SetPending(int id, string purchaseOrderId)
        {

            var base64Version = Request.Form["RecordVersionBase64"];
            var recordVersion = Convert.FromBase64String(base64Version);

            try
            {
                await _purchaseOrderItemService.SetItemStatusAsync(id, "Pending", recordVersion);

                TempData["SuccessMessage"] = "Purchase order item status set back to Pending.";
            }
            catch (ValidationException ex)
            {
                TempData["ConcurrencyError"] = ex.Message;
            }

            return RedirectToAction("Details", "PurchaseOrder", new { id = purchaseOrderId });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditBySupervisor(SupervisorPOItemUpdateDto dto)
        {
            var base64Version = Request.Form["RecordVersionBase64"];
            dto.RecordVersion = Convert.FromBase64String(base64Version);

            var result = _purchaseOrderItemService.UpdateBySupervisor(dto);

            if (result.Errors.Any())
            {
                foreach (var error in result.Errors)
                {
                    TempData["ConcurrencyError"] = error.Description;
                }

                return RedirectToAction("Details", "PurchaseOrder", new { id = result.PurchaseOrderId });
            }

            TempData["SuccessMessage"] = "Item updated successfully.";
            return RedirectToAction("Details", "PurchaseOrder", new { id = result.PurchaseOrderId });
        }


        [HttpGet]
        public async Task<IActionResult> EditBySupervisor(int id, string recordVersionBase64)
        {

            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (
                loggedInUser == null || 
                !RoleHelper.IsAllowed(loggedInUser.Role, 
                AccessLevels.HRSupervisor, 
                AccessLevels.RegularSupervisor,
                AccessLevels.CEO
                )

                )
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            var item = await _purchaseOrderItemService.GetByIdAsync(id);
            if (item == null)
                return NotFound();

            var dto = new SupervisorPOItemUpdateDto
            {
                Id = item.Id,
                Quantity = item.Quantity,
                Price = item.Price,
                PurchaseLocation = item.PurchaseLocation,
                ModificationReason = item.ModificationReason ?? "",
                RecordVersion = Convert.FromBase64String(recordVersionBase64)
            };

            ViewBag.PurchaseOrderId = item.PurchaseOrderId;

            return View("SupervisorEdit", dto);
        }


    }

}
