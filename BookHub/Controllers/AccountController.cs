using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using BookHub.Application.Interfaces;
using BookHub.Application.Models;
using BookHub.Constants;
using BookHub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet("GetUserInfo")]
        public IActionResult GetUserInfo()
        {
            var userIdClaim = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            var userInfo = new UserInfoDto
            {
                Id = Guid.Parse(userIdClaim.Value)
            };

            return Ok(userInfo);
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TokenResponseDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!(await _accountService.CheckPassword(model.Email, model.Password)))
                {
                    return Unauthorized("Invalid credentials");
                }

                var token = await _accountService.GenerateTokenAsync(model.Email);

                return Ok(new TokenResponseDto { Token = token });

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
        [HttpPost("Register")]
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
        /// Enter with external provider.
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="callbackUrl"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login/External/{scheme}")]
        public IActionResult ExternalAuthentication(string scheme,
            [FromQuery][Required] string callbackUrl,
            [FromQuery] string? returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl)) returnUrl = "~/";

            // Start challenge and roundtrip the return URL and scheme 
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(Callback), new { scheme }),
                Items =
                {
                    { "callbackUrl", callbackUrl },
                    { "returnUrl", returnUrl },
                }
            };

            return Challenge(props, scheme);
        }

        /// <summary>
        /// Handles the callback from the external provider after authentication.
        /// </summary>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("Callback/{scheme}")]
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

                var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    throw new Exception("Email not found");
                }

                var isExists = await _accountService.IsExistsInExternalLoginProviderUserAsync(email);

                //if user not fount create new user
                if (!isExists)
                {
                    string externalId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                                        throw new Exception("externalId not found");

                    await _accountService.RegisterUserByExternalProviderAsync(email, scheme, externalId);
                }

                // If the user is found, Login user and redirect to the return URL
                var token = await _accountService.GenerateTokenAsync(email) ??
                            throw new ArgumentNullException($"Token is null");

                var callbackUrl = result.Properties?.Items["callbackUrl"]!;

                // retrieve return URL
                var returnUrl = result.Properties?.Items["returnUrl"] ?? "~/";

                Console.WriteLine($"User {email} authorized");
                return Redirect(callbackUrl.TrimEnd('/') + $"?token={token}&returnUrl{returnUrl}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Message: {e.Message}");
                Console.WriteLine($"Inner Exception: {e.InnerException}");

                return Redirect("~/");
            }
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>Returns a NoContent result upon successful logout.</returns>
        [Authorize]
        [HttpPost("Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(AuthConstants.ExternalProviderAuthScheme);
                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
