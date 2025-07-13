using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MJRecords.Model;
using MJRecords.Service;
using MJRecords.Types;
using MJRecords.Web.Models;

namespace MJRecords.Web.Controllers
{
    public class EmployeeReviewController : Controller
    {
        private readonly IEmployeeReviewService _employeeReviewService;
        private readonly IEmployeeService _employeeService;
        private readonly IEmploymentStatusService _employmentStatusService;
        private readonly IRatingOptionsService _ratingOptionsService;

        public EmployeeReviewController(IEmployeeReviewService employeeReviewService, IEmployeeService employeeService,
            IRatingOptionsService ratingOptionsService, IEmploymentStatusService employmentStatusService)
        { 
            _employeeReviewService = employeeReviewService;
            _employeeService = employeeService;
            _ratingOptionsService = ratingOptionsService;
            _employmentStatusService = employmentStatusService;
        }


        // GET: EmployeeReviewController
        public ActionResult Index()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            List<EmployeeReviewListDTO> reviews = _employeeReviewService.GetAllReviewsMadeBySupervisor(loggedInUser.Id);
            return View(reviews);
        }

        // GET: EmployeeReviewController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmployeeReviewController/Create
        public ActionResult Create()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            EmployeeReviewVM vm = new EmployeeReviewVM();
            vm.Employees = GetEmployeesPendingReviewBySupervisor(loggedInUser.Id, GetQuarter(DateTime.Now), DateTime.Now.Year);
            vm.Quarter = PopulateYearQuarters();
            vm.RatingOptions = GetAllRatingOptions();

            return View(vm);
        }

        // POST: EmployeeReviewController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeReviewVM vm)
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null || !RoleHelper.IsAllowed(loggedInUser.Role, AccessLevels.CEO, AccessLevels.HRSupervisor, AccessLevels.RegularSupervisor))
            {
                return RedirectToAction("AccessDenied", "Error");

            }

            try
            {
                vm.Review =  _employeeReviewService.Create(vm.Review, GetQuarter(DateTime.Now) - 1, DateTime.Now.Year);

                if (vm.Review.Errors.Count == 0)
                {
                    TempData["Success"] = $"Employee Review was added successfully";
                    return RedirectToAction(nameof(Index));
                }
                if (vm.Review.Errors.Count == 1 && vm.Review.Errors[0].Description.Equals("There was an issue with adding the employee review to the database."))
                {
                    TempData["Success"] = $"Employee Review was added successfully";
                    return RedirectToAction(nameof(Index));
                }
                vm.Employees = GetEmployeesPendingReviewBySupervisor(loggedInUser.Id, GetQuarter(DateTime.Now), DateTime.Now.Year);
                vm.Quarter = PopulateYearQuarters();
                vm.RatingOptions = GetAllRatingOptions();

                return View(vm);
            }
            catch(Exception ex)
            {
                if(ex.Message.Equals("There was an issue with adding the employee review to the database.") || vm.Review.Errors.Count == 1 && vm.Review.Errors[0].Description.Equals("There was an issue with adding the employee review to the database.")){
                    TempData["Success"] = $"Employee Review was added successfully";
                    return RedirectToAction(nameof(Index));
                }
                vm.Employees = GetEmployeesPendingReviewBySupervisor(loggedInUser.Id, GetQuarter(DateTime.Now), DateTime.Now.Year);
                vm.Quarter = PopulateYearQuarters();
                vm.RatingOptions = GetAllRatingOptions();
                return View(vm);
            }
        }

        // GET: EmployeeReviewController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmployeeReviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeReviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmployeeReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        private List<SelectListItem> GetAllRatingOptions()
        {
            return _ratingOptionsService.GetAll().Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
                Text = r.RatingOption
            }).ToList();
        }

        [NonAction]
        private List<SelectListItem> GetEmployeesPendingReviewBySupervisor(string id, int quarter, int year)
        {
            return _employeeReviewService.FindEmployeesWithoutReviewInQuarterBySupervisor(id, quarter, year)
                 ?.OrderBy(e => e.LastName)
                 .ThenBy(e => e.FirstName)
                 .Select(e => new SelectListItem
                    {
                        Value = e.EmployeeId,
                        Text = $"{e.LastName}, {e.FirstName}"
                    }).ToList() ?? new List<SelectListItem>();
        }

        [NonAction]
        private static int GetQuarter(DateTime date)
        {
            // Quarter is determined by the month:
            // Q1: Jan(1), Feb(2), Mar(3)
            // Q2: Apr(4), May(5), Jun(6)
            // Q3: Jul(7), Aug(8), Sep(9)
            // Q4: Oct(10), Nov(11), Dec(12)

            return (date.Month - 1) / 3 + 1;
        }

        [NonAction]
        private List<SelectListItem> PopulateYearQuarters()
        {
            int year = DateTime.Now.Year;

            List<SelectListItem> quarters = new();
            quarters.Add(new SelectListItem
            {
                Value = "1",
                Text = $"Q1: Jan 1 - Mar 31 {year}"
            });
            quarters.Add(new SelectListItem
            {
                Value = "2",
                Text = $"Q2: Apr 1 - Jun 30 {year}"
            });
            quarters.Add(new SelectListItem
            {
                Value = "3",
                Text = $"Q3: Jul 1 - Sep 30 {year}"
            });
            quarters.Add(new SelectListItem
            {
                Value = "4",
                Text = $"Q4: Oct 1 - Dec 31  {year}"
            });

            return quarters;
        }
    }
}
