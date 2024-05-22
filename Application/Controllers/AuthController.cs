using Api.Application.Models.Auth;
using Api.Application.Models.Base;
using Api.Application.Services.Auth;
using Api.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Application.Controllers
{
    [Route("api/v1/auth")]
    [IgnoreAntiforgeryToken]
    [AllowAnonymous]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly IConfiguration _configuration;

        public AuthController(
            AuthService authService,
            IConfiguration config
        )
        {
            _authService = authService;
            _configuration = config;
        }

        /// <summary>
        /// Requests a login
        /// </summary>
        /// <param name="request">The username and password</param>
        /// <returns>Authenticated user ID and bearer token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<string>("One or more fields does not match requirement."));

            var response = await _authService.AuthenticateUserAsync(request);
            return response.Match<IActionResult>(
                created => CreatedAtAction(nameof(Login), new BaseResponse<JwtAccessToken>(created.Value.Result)),
                error => Unauthorized(new BaseResponse<JwtAccessToken>(error.Message))
            );
        }

        /// <summary>
        /// Register a user
        /// </summary>
        /// <param name="request">The username and password</param>
        /// <returns>Authenticated user ID and bearer token</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new BaseResponse<string>("One or more fields does not match requirement."));

            var response = await _authService.RegisterUserAsync(request);
            return response.Match<IActionResult>(
                created => CreatedAtAction(nameof(Register), new BaseResponse<JwtAccessToken>(created.Value.Result)),
                badRequest => BadRequest(new BaseResponse<JwtAccessToken>(badRequest.Message))
            );
        }
    }
}