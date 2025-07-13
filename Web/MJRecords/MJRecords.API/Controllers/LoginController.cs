using Microsoft.AspNetCore.Mvc;
using MJRecords.Model;
using MJRecords.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MJRecords.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly ITokenService  _tokenService;

        public LoginController(ILoginService loginService, ITokenService tokenService)
        {
            _loginService = loginService;
            _tokenService = tokenService;
        }



        /// <summary>
        /// Default endpoint for the Login API. Returns a simple confirmation message.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a plain text message confirming the endpoint is reachable.
        /// </returns>
        // GET: api/<LoginController>
        [HttpGet("")]
        public IActionResult Get()
        {
            return Ok("You're on the LOGIN API");
        }


        /// <summary>
        /// Authenticates a user with the provided login credentials and returns a JWT token if successful.
        /// </summary>
        /// <param name="login">The login credentials as a <see cref="LoginDTO"/>.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a <see cref="LoginOutputDTO"/> with the authenticated user details and token,
        /// or an unauthorized response if authentication fails.
        /// </returns>
        /// <response code="200">Returns the authenticated user and token.</response>
        /// <response code="401">Returned when login credentials are invalid.</response>
        // POST api/<LoginController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            LoginOutputDTO loggedInEmployee = await _loginService.LoginAsync(login);

            if (loggedInEmployee == null)
                return Unauthorized("Invalid Login");

            string token = _tokenService.CreateToken(loggedInEmployee);
            loggedInEmployee.Token = token;
            return Ok(loggedInEmployee);
        }
    }
}
