using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MJRecords.Model;
using MJRecords.Service;
using MJRecords.Types;
using MJRecords.Web.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MJRecords.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;
        private readonly IJobService _jobService;
        private readonly IEmploymentStatusService _employmentStatusService;

        public EmployeeController(IEmployeeService employeeService, IDepartmentService departmentService, IJobService jobService,
            IEmploymentStatusService employmentStatusService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
            _jobService = jobService;
            _employmentStatusService = employmentStatusService;
        }

        // GET: EmployeeController
        public ActionResult Index()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor, AccessLevels.HREmployee, AccessLevels.RegularEmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            if(RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.RegularSupervisor, AccessLevels.RegularEmployee))
            {
                List<EmployeeListDTO> listDTOs = _employeeService.GetEmployeesListDisplay().Where(e => loggedInUser.MiddleInitial == null
                        ? e.FullName.Equals($"{loggedInUser.FirstName}  {loggedInUser.LastName}")
                        : e.FullName.Equals($"{loggedInUser.FirstName} {loggedInUser.MiddleInitial}  {loggedInUser.LastName}")
                ).ToList();

                return View(listDTOs);
            }
            //List<EmployeeDTO> employees = _employeeService.GetAll();
            List<EmployeeListDTO> employees = _employeeService.GetEmployeesListDisplay();
            return View(employees);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(string id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            if(!RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee) || loggedInUser.Id != id)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            return View();
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {

            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            EmployeeVM vm = new EmployeeVM();

            // Populate Supervisors
            //vm.Employees = GetAvailableSupervisors();

            // Populate Departments
            vm.Departments = GetDepartments();

            // Populate Jobs
            vm.Jobs = GetJobs();

            // Populate Statuses
            vm.Statuses = GetStatuses();

            return View(vm);
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeVM emp)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            EmployeeDTO empDto = _employeeService.EmployeeConverter(emp.Employee);
            empDto.Status = 1;

            Employee newEmp = _employeeService.Create(empDto);

            try
            {
                if(newEmp.Errors.Count == 0)
                {
                    TempData["Success"] = $"Employee was added successfully with ID: {newEmp.Id}!";
                    return RedirectToAction(nameof(Index));
                }

                // Populate Supervisors
                //emp.Employees = GetAvailableSupervisors();

                // Populate Departments
                emp.Departments = GetDepartments();

                // Populate Jobs
                emp.Jobs = GetJobs();

                emp.Employee = newEmp;

                // Populate Statuses
                emp.Statuses = GetStatuses();

                return View(emp);
            }
            catch
            {
                // Populate Supervisors
                //emp.Employees = GetAvailableSupervisors();

                // Populate Departments
                emp.Departments = GetDepartments();

                // Populate Jobs
                emp.Jobs = GetJobs();

                emp.Statuses = GetStatuses();

                return View(emp);
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(string id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                if(!RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.RegularEmployee, AccessLevels.RegularSupervisor) &&
                    !loggedInUser.Id.Equals(id))
                {
                    return RedirectToAction("AccessDenied", "Error");
                }

            }

            Employee emp = _employeeService.EmployeeConverter(_employeeService.Get(id));
            //EmployeeVM empVM = new();
            EmployeeUpdateVM empUpdateVM = new();
            empUpdateVM.Statuses = GetStatuses();
            empUpdateVM.Jobs = GetJobs();
            empUpdateVM.Departments = GetDepartments();

            empUpdateVM.Employee = ToEmployeeUpdateDTO(emp);

            return View(empUpdateVM);
        }

        [HttpGet]
        public IActionResult EditPersonalInfo(string id)
        {
            Employee dbEmp = _employeeService.GetEmp(id);
            EmployeePersonalInfoDTO empPersonalInfo = new();
            empPersonalInfo.Id = dbEmp.Id;
            empPersonalInfo.FirstName = dbEmp.FirstName;
            empPersonalInfo.LastName = dbEmp.LastName;
            empPersonalInfo.MiddleInitial = dbEmp.MiddleInitial;
            empPersonalInfo.City = dbEmp.City;
            empPersonalInfo.StreetAddress = dbEmp.StreetAddress;
            empPersonalInfo.PostalCode = dbEmp.PostalCode;
            empPersonalInfo.Province = dbEmp.Province;
            
            return View(empPersonalInfo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPersonalInfo(EmployeePersonalInfoDTO empInfo)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            // Check if user is logged in
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Regular employee access level check (only allowed to edit their own profile)
            bool isRegularEditingSelf = RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.RegularEmployee, AccessLevels.RegularSupervisor) &&
                                        loggedInUser.Id.Equals(empInfo.Id);

            if (!isRegularEditingSelf)
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            try
            {
                Employee dbEmp = _employeeService.GetEmp(empInfo.Id);

                if (dbEmp == null)
                {
                    return View(empInfo);
                }

                dbEmp.FirstName = empInfo.FirstName;
                dbEmp.LastName = empInfo.LastName;
                dbEmp.Password = empInfo.Password;
                dbEmp.MiddleInitial = empInfo.MiddleInitial;
                dbEmp.StreetAddress = empInfo.StreetAddress;
                dbEmp.City = empInfo.City;
                dbEmp.Province = empInfo.Province;
                dbEmp.PostalCode = empInfo.PostalCode;

                Employee updatedEmp = _employeeService.Update(dbEmp);
                if(updatedEmp.Errors.Count == 1 && updatedEmp.Errors[0].Description.Equals("Unknown error occurred during update."))
                {
                    // Create a fresh LoginOutputDTO instead of modifying the existing one
                    LoginOutputDTO refreshedUser = new LoginOutputDTO
                    {
                        Id = updatedEmp.Id,
                        FirstName = updatedEmp.FirstName,
                        LastName = updatedEmp.LastName,
                        MiddleInitial = updatedEmp.MiddleInitial,
                        // Make sure to preserve the role and other important session data
                        Role = loggedInUser.Role,
                        // Add any other properties from loggedInUser that need to be preserved
                        Token = loggedInUser.Token
                    };

                    // Update the session
                    HttpContext.Session.Remove("loggedInUser");
                    HttpContext.Session.SetObject("loggedInUser", refreshedUser);

                    TempData["Success"] = $"Employee was updated successfully with ID: {updatedEmp.Id}!";
                    return View(empInfo);
                }
                empInfo.Errors = updatedEmp.Errors;

                return View(empInfo);

            }
            catch (Exception ex)
            {
                return View(empInfo);
            }
        }
        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeUpdateVM vm)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            // Check if user is logged in
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Retrieve the employee from the database
            Employee dbEmp = _employeeService.GetEmp(vm.Employee.Id);

            // Handle job assignment change
            if (vm.Employee.JobAssignmentId != dbEmp.JobAssignmentId)
            {
                dbEmp.JobAssignmentId = vm.Employee.JobAssignmentId;
                dbEmp.JobStartDate = DateTime.Now; // Change the job start date if the job changes
            }

            // Handle employment status changes
            switch (vm.Employee.Status)
            {
                case 1: // Active
                        // If employee was previously retired or terminated and now active, clear dates
                    if (dbEmp.Status != 1)
                    {
                        dbEmp.TerminationDate = null;
                        // Note: RetirementDate should not be cleared if it was already set
                    }
                    break;

                case 2: // Retired
                        // Check if employee is at least 65 years old
                    int age = DateTime.Today.Year - vm.Employee.DateOfBirth.Year;
                    if (vm.Employee.DateOfBirth.Date > DateTime.Today.AddYears(-age)) age--; // Adjust if birthday hasn't occurred this year

                    if (age < 65)
                    {
                        vm.Employee.AddError(new("Employee must be at least 65 years old to retire.", ErrorType.Business));

                        ModelState.AddModelError("Employee.Status", "Employee must be at least 65 years old to retire.");
                        // Populate Departments
                        vm.Departments = GetDepartments();

                        // Populate Jobs
                        vm.Jobs = GetJobs();

                        // Populate Statuses
                        vm.Statuses = GetStatuses();
                        return View(vm);
                    }

                    // Handle retirement date
                    if (dbEmp.Status == 2 && dbEmp.RetirementDate != null)
                    {
                        // Retirement date is already set, use existing date (make read-only)
                        vm.Employee.RetirementDate = dbEmp.RetirementDate;
                    }
                    else
                    {
                        // New retirement - ensure retirement date is provided
                        //if (vm.Employee.RetirementDate == null)
                        //{
                        //    ModelState.AddModelError("Employee.RetirementDate", "Retirement date is required.");
                        //    return View(vm);
                        //}

                        // Set the retirement date
                        dbEmp.RetirementDate = DateTime.Now;
                    }
                    break;

                case 3: // Terminated
                        // Handle termination date
                    if (dbEmp.Status == 3 && dbEmp.TerminationDate != null)
                    {
                        // Termination date is already set, use existing date (make read-only)
                        vm.Employee.TerminationDate = dbEmp.TerminationDate;
                    }
                    else
                    {
                        // New termination - ensure termination date is provided
                        // Set the termination date
                        dbEmp.TerminationDate = DateTime.Now;
                        vm.Employee.TerminationDate = dbEmp.TerminationDate;
                    }
                    break;

                default:
                    vm.Employee.AddError(new("Termination date is required.", ErrorType.Business));
                    ModelState.AddModelError("Employee.Status", "Invalid employment status.");
                    // Populate Departments
                    vm.Departments = GetDepartments();

                    // Populate Jobs
                    vm.Jobs = GetJobs();

                    // Populate Statuses
                    vm.Statuses = GetStatuses();
                    return View(vm);
            }

            // Update status after validation
            dbEmp.Status = vm.Employee.Status;

            // Update other fields from vm.Employee to dbEmp...

            // Save changes
            try
            {
                dbEmp.Password = vm.Employee.Password;
                dbEmp.FirstName = vm.Employee.FirstName;
                dbEmp.LastName = vm.Employee.LastName;
                dbEmp.MiddleInitial = vm.Employee.MiddleInitial ?? null;
                dbEmp.StreetAddress = vm.Employee.StreetAddress;
                dbEmp.City = vm.Employee.City;
                dbEmp.PostalCode = vm.Employee.PostalCode;
                dbEmp.Province = vm.Employee.Province;
                dbEmp.DateOfBirth = vm.Employee.DateOfBirth;
                dbEmp.CellPhone = vm.Employee.CellPhone;

                dbEmp.SIN = vm.Employee.SIN;
                dbEmp.SeniorityDate = vm.Employee.SeniorityDate;
                dbEmp.DepartmentId = vm.Employee.DepartmentId;
                dbEmp.SupervisorId = vm.Employee.SupervisorId;
                dbEmp.EmailAddress = vm.Employee.EmailAddress;
                dbEmp.WorkPhone = vm.Employee.WorkPhone;
                dbEmp.OfficeLocation = vm.Employee.OfficeLocation;


                Employee updatedEmp = _employeeService.Update(dbEmp);
                if(updatedEmp.Errors.Count == 0 || updatedEmp.Errors.Count == 1 && updatedEmp.Errors[0].Description.Equals("Unknown error occurred during update."))
                {
                    if(updatedEmp.Id == loggedInUser.Id)
                    {
                        // Create a fresh LoginOutputDTO instead of modifying the existing one
                        LoginOutputDTO refreshedUser = new LoginOutputDTO
                        {
                            Id = updatedEmp.Id,
                            FirstName = updatedEmp.FirstName,
                            LastName = updatedEmp.LastName,
                            MiddleInitial = updatedEmp.MiddleInitial,
                            // Make sure to preserve the role and other important session data
                            Role = loggedInUser.Role,
                            // Add any other properties from loggedInUser that need to be preserved
                            Token = loggedInUser.Token
                        };

                        // Update the session
                        HttpContext.Session.Remove("loggedInUser");
                        HttpContext.Session.SetObject("loggedInUser", refreshedUser);
                    }

                    TempData["Success"] = $"Employee Info was updated successfully for {updatedEmp.FirstName} {updatedEmp.LastName} with ID: {updatedEmp.Id}!";

                    // Populate Departments
                    vm.Departments = GetDepartments();

                    // Populate Jobs
                    vm.Jobs = GetJobs();

                    // Populate Statuses
                    vm.Statuses = GetStatuses();

                    return View(vm);
                }
                // Populate Departments
                vm.Departments = GetDepartments();

                // Populate Jobs
                vm.Jobs = GetJobs();

                // Populate Statuses
                vm.Statuses = GetStatuses();


                vm.Employee = ToEmployeeUpdateDTO(updatedEmp);
                return View(vm);
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating the employee: " + ex.Message);
                return View(vm);
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            return View();
        }

        // POST: EmployeeController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        //[NonAction]
        //private List<SelectListItem> GetAvailableSupervisors(int deptId)
        //{

        //    return _employeeService.GetAvailableSuperVisorsByDepartment(deptId)
        //            .Select(s => new SelectListItem
        //            {
        //                Value = s.Id.ToString(),
        //                Text = $"{s.FirstName} {s.LastName} - {_employeeService.GetEmployeeDetails(s.Id)?.Department}"
        //            }).OrderBy(s => s.Text).ToList();
        //}

        [HttpGet]
        public JsonResult GetAvailableSupervisors([FromQuery] int deptId)
        {

            var supervisors = _employeeService.GetAvailableSuperVisorsByDepartment(deptId)
                    .Select(s => new
                    {
                        s.Id,
                        FullName = $"{s.FirstName} {s.LastName} - {_employeeService.GetEmployeeDetails(s.Id)?.Department}",
                        s.JobTitle,
                        s.SupervisedEmployeeCount
                    }).ToList();

            return Json(supervisors);
        }

        //[NonAction]
        //public List<SelectListItem> GetAvailableSupervisors(Employee emp)
        //{
        //    var supervisors = _employeeService.GetAvailableSuperVisorsByDepartment(emp.DepartmentId!)
        //            .Select(s => new SelectListItem
        //            {
        //                Value = s.Id.ToString(),
        //                Text = $"{s.FirstName} {s.LastName} - {_employeeService.GetEmployeeDetails(s.Id)?.Department}",
        //                // You could store additional data using data attributes in the view if needed
        //            }).ToList();
        //    return supervisors;
        //}

        [NonAction]
        private List<SelectListItem> GetDepartments()
        {
            // Only return active departments
            return _departmentService.GetAll()
                    .Where(d => d.InvocationDate <= DateTime.Now)
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = $"{d.Name}"
                    }).ToList();
        }

        [NonAction]
        private List<SelectListItem> GetJobs()
        {
            return _jobService.GetAll()
                    .Select(j => new SelectListItem
                    {
                        Value = j.Id.ToString(),
                        Text = $"{j.Title}"
                    }).ToList();
        }

        [NonAction]
        private List<SelectListItem> GetStatuses()
        {
            return _employmentStatusService.GetAll()
                    .Select(s => new SelectListItem
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.Status}"
                    }).ToList();
        }

        [NonAction]
        private EmployeeUpdateDTO ToEmployeeUpdateDTO(Employee emp)
        {
            return new EmployeeUpdateDTO
            {
                Id = emp.Id,
                SupervisorId = emp.SupervisorId,
                DepartmentId = emp.DepartmentId,
                JobAssignmentId = emp.JobAssignmentId,
                Status = emp.Status,
                Password = emp.Password,
                PasswordSalt = emp.PasswordSalt,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                MiddleInitial = emp.MiddleInitial,
                StreetAddress = emp.StreetAddress,
                City = emp.City,
                Province = emp.Province,
                PostalCode = emp.PostalCode,
                DateOfBirth = emp.DateOfBirth,
                SIN = emp.SIN,
                SeniorityDate = emp.SeniorityDate,
                JobStartDate = emp.JobStartDate,
                WorkPhone = emp.WorkPhone,
                CellPhone = emp.CellPhone,
                EmailAddress = emp.EmailAddress,
                OfficeLocation = emp.OfficeLocation,
                TerminationDate = emp.TerminationDate,
                RetirementDate = emp.RetirementDate,
                RecordVersion = emp.RecordVersion,
                
            };
        }
    }
}
