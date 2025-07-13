using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;
using MJRecords.Types;
using MJRecords.Web.Models;

namespace MJRecords.Web.Controllers
{
    public class EmployeeSearchController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeSearchController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        // GET: EmployeeSearchController
        public IActionResult Index()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }
            EmployeeSearchVM vm = new();

            return View(vm);
        }

        // GET: EmployeeSearchController/Details/5
        //public ActionResult Details(int id)
        //{
        //    var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

        //    if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
        //    {
        //        return RedirectToAction("AccessDenied", "Error");

        //    }

        //    return View();
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EmployeeSearchVM vm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    vm.empSearchResult = await _employeeService.SearchEmployeeAsync(vm.empSearchParms);
                }

                return View(vm);

            }
            catch (Exception ex)
            {
                return View(vm);
            }
        }

        //GET: EmployeeSearch/Details/00000005
        public async Task<IActionResult> Details(string id)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.HREmployee))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            try
            {
                if (id == null)
                {
                   return RedirectToAction(nameof(Index));
                }
                EmployeeSearchDTO search = new();
                search.EmployeeId = id;
                var result = await _employeeService.SearchEmployeeDetailedAsync(search);
                if(result.Count == 0 || result is null)
                {
                    return RedirectToAction(nameof(Index));
                }
                EmployeeSearchResultDetailedDTO foundEmp = result.FirstOrDefault();

                return View(foundEmp);
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: EmployeeSearchController/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: EmployeeSearchController/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: EmployeeSearchController/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        // POST: EmployeeSearchController/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        // GET: EmployeeSearchController/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        // POST: EmployeeSearchController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
