using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;
using MJRecords.Web.Models;

namespace MJRecords.Web.Controllers
{
    public class LoginController : Controller
    {

        private readonly ILoginService _loginService;
        private readonly IEmployeeService _employeeService;
        private readonly ITokenService _tokenService;
        public LoginController(ILoginService loginService, IEmployeeService employeeService, ITokenService tokenService)
        {
            _loginService = loginService;
            _employeeService = employeeService;
            _tokenService = tokenService;
        }


        [HttpGet]
        public IActionResult Index()
        {

            var loggedInUser = HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser");

            if (loggedInUser != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(LoginDTO login)
        {

            if (!ModelState.IsValid)
            {
                return View(login);
            }

            LoginOutputDTO employee = await _loginService.LoginAsync(login);

            if(employee == null)
            {
                ViewBag.Error = "Invalid username or password";
                return View();
            }

            var token = _tokenService.CreateToken(employee);
            employee.Token = token;

            HttpContext.Session.SetObject("loggedInUser", employee);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {

            if (HttpContext.Session.GetObject<LoginOutputDTO>("loggedInUser") != null)
            {
                HttpContext.Session.Remove("loggedInUser");
            };

            return RedirectToAction("Index", "Login");
        }
    }
}
