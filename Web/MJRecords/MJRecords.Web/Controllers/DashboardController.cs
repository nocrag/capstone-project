using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;
using MJRecords.Web.Models;

namespace MJRecords.Web.Controllers
{
    public class DashboardController : Controller
    {

        private readonly IPurchaseOrderService _purchaseOrderService;

        public DashboardController(IPurchaseOrderService purchaseOrderService)
        {
            _purchaseOrderService = purchaseOrderService;
        }
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// Displays the dashboard view for a supervisor or CEO, showing monthly expenses and pending purchase orders for review.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the supervisor dashboard view with relevant data, 
        /// or redirects to the Access Denied page if the user is not authorized.
        /// </returns>
        /// <remarks>
        /// This action requires the logged-in user to have a role containing "Supervisor" or "CEO".
        /// </remarks>
        public async Task<IActionResult> SupervisorDashboard()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null)
                return RedirectToAction("AccessDenied", "Error");

            if (!loggedInUser.Role.Contains("Supervisor") && !loggedInUser.Role.Contains("CEO"))
                return RedirectToAction("AccessDenied", "Error");

            var monthlyExpenses = _purchaseOrderService.GetMonthlyExpensesForSupervisor(loggedInUser.Id);

            var pendingPOs = await _purchaseOrderService.GetPendingAndUnderReviewPOsByDepartmentAsync(loggedInUser.DepartmentId, loggedInUser.Id);
            var pendingReviewCount = pendingPOs.Count;

            var vm = new DashboardViewModel
            {
                MonthlyExpenses = monthlyExpenses,
                PendingReviewCount = pendingReviewCount
            };

            return View("SupervisorDashboard", vm);
        }


        /// <summary>
        /// Displays the dashboard view for a regular employee, showing their monthly purchase order expenses.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the employee dashboard view with relevant data,
        /// or redirects to the Access Denied page if the user is not authenticated.
        /// </returns>
        /// <remarks>
        /// This action is intended for employees who are not supervisors or CEOs.
        /// </remarks>
        public async Task<IActionResult> EmployeeDashboard()
        {
            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser == null)
                return RedirectToAction("AccessDenied", "Error");

            var monthlyExpenses = _purchaseOrderService.GetMonthlyExpensesForEmployee(loggedInUser.Id);

            var vm = new DashboardViewModel
            {
                MonthlyExpenses = monthlyExpenses,
                PendingReviewCount = 0
            };

            return View("EmployeeDashboard", vm);
        }
    }
}
