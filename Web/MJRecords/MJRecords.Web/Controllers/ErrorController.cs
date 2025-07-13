using Microsoft.AspNetCore.Mvc;

namespace MJRecords.Web.Controllers
{
    public class ErrorController : Controller
    {

        /// <summary>
        /// Displays the Access Denied view when a user attempts to access a restricted resource.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the Access Denied page.
        /// </returns>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }
    }
}
