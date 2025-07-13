using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;
using MJRecords.Types;
using MJRecords.Web.Models;
using System.Linq;

namespace MJRecords.Web.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;


        public DepartmentController(IDepartmentService departmentService, IEmployeeService employeeService)
        {
            _departmentService = departmentService;
            _employeeService = employeeService;
        }


        // GET: DepartmentController
        [HttpGet]
        public ActionResult Index()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            List<DepartmentDTO> departments = _departmentService.GetAll();

            if (loggedInUser.Role.Equals("Regular Supervisor"))
            {
                Employee emp = _employeeService.GetEmp(loggedInUser.Id);

                departments = departments.Where(d => d.Id == emp.DepartmentId).ToList();
            }

            return View(departments);
        }

        // GET: DepartmentController/Details/5
        public ActionResult Details(int id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            if (!RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee) || !loggedInUser.Id.Equals(id.ToString()))
            {
                return RedirectToAction("AccessDenied", "Error");
            }

            return View();
        }

        // GET: DepartmentController/Create
        public ActionResult Create()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            return View(new Department());
        }

        // POST: DepartmentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Department department)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            try
            {
                Department createdDept = _departmentService.Create(department);
                if (createdDept.Errors.Count == 0)
                {
                    TempData["Success"] = $"{createdDept.Name} Department was added successfully!";
                    return RedirectToAction(nameof(Index));
                }
                department.Errors = createdDept.Errors;
                return View(createdDept);
            }
            catch
            {
                return View(department);
            }
        }

        // GET: DepartmentController/Edit/5
        public ActionResult Edit(int id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee, AccessLevels.RegularSupervisor))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            Employee dbEmp = _employeeService.GetEmp(loggedInUser.Id);

            if(loggedInUser.Role.Equals("Regular Supervisor") && dbEmp.DepartmentId != id)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            DepartmentDTO deptDto = _departmentService.Get(id);
            Department dept = _departmentService.PopulateDepartmentFromDTO(deptDto);


            return View(dept);
        }

        // POST: DepartmentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department dept)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee, AccessLevels.RegularSupervisor))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            Employee dbEmp = _employeeService.GetEmp(loggedInUser.Id);

            if (loggedInUser.Role.Equals("Regular Supervisor") && dbEmp.DepartmentId != dept.Id)
            {
                return RedirectToAction("AccessDenied", "Error");
            }
            Department dbDept = _departmentService.GetDept(dept.Id);

            if (dept.InvocationDate != dbDept.InvocationDate)
            {
                if (dept.InvocationDate < DateTime.Now)
                {
                    dept.AddError(new("Invocation Date can't be set in the past", ErrorType.Business));
                }
            }

            if(dept.Errors.Count > 0)
            {
                return View(dept);
            }
            dbDept.Name = dept.Name;
            dbDept.Description = dept.Description;
            dbDept.InvocationDate = dept.InvocationDate;

            try
            {
                Department updatedDept = _departmentService.Update(dbDept);

                if (updatedDept.Errors.Count == 0 || updatedDept.Errors.Count == 1 && updatedDept.Errors[0].Description.Equals("There was an issue while updating the record in the database."))
                {
                    TempData["Success"] = $"The {dept.Name} Department was updated successfully with ID: {updatedDept.Id}!";
                    return RedirectToAction("Index");
                }
                if(updatedDept.Errors.Count == 1 && updatedDept.Errors.Any(e => e.Description.Contains("UQ_Department_Name") && e.ErrorType == ErrorType.Business))
                {
                    updatedDept.Errors.Clear();
                    updatedDept.Errors.Add(new("A department with this name already exists.", ErrorType.Business));
                }
                return View(updatedDept);
            }
            catch
            {
                return View(dept);
            }
        }

        // GET: DepartmentController/Delete/5
        public ActionResult Delete(int id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            return View();
        }

        // POST: DepartmentController/Delete/5
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
    }
}
