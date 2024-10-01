using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Controllers
{
    /// <summary>
    /// Provides endpoints for user account management, including registration, login, and other related operations.
    /// </summary>
    /// /// <remarks>
    /// This controller handles HTTP requests related to user accounts and interacts with the <see cref="IAccountService"/>
    /// to perform actions such as user registration and authentication.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService">
        /// The account service that handles user registration, authentication, and related operations.
        /// </param>
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Authenticates a user and generates a JWT token if the credentials are valid.
        /// </summary>
        /// <param name="model">
        ///The login model containing the user's email and password.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the result of the login process.
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _accountService.AuthenticateUserAsync(model.Email, model.Password);

                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("Invalid credentials");
                }

                return Ok(new { Token = token });

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="model">
        /// The user registration model containing user details such as username, password, and email.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> indicating the result of the registration:
        /// </returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                await _accountService.RegisterUserAsync(model);
                return Ok("User registered successfully");
            }

            return BadRequest(400);
        }

        /// <summary>
        /// Enter with external provider
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[action]/{scheme}")]
        public IActionResult ExternalAuthentication(string scheme, [FromQuery] string? returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // Start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback), new { scheme }),
                Items =
                {
                    { "returnUrl", returnUrl },
                }
            };

            return Challenge(props, scheme);
        }

        /// <summary>
        /// Post-processing of external authentication
        /// </summary>
        [HttpGet]
        [Route("[action]/{scheme}")]
        public async Task<IActionResult> Callback(string scheme)
        {
            try
            {
                // read external identity from the temporary cookie
                var result = await HttpContext.AuthenticateAsync(scheme);
                if (result.Succeeded != true)
                {
                    throw new Exception("External authentication error");
                }

                // retrieve return URL
                var returnUrl = result.Properties?.Items["returnUrl"] ?? "~/";

                return Redirect(returnUrl);
            }
            catch (Exception e)
            {
                return Redirect("~/");
            }
        }
    }
}
