using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.Auth;
using DealNotifier.Core.Application.ViewModels.V1.Token;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;

namespace DealNotifier.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        private readonly ILogger _logger;

        public AccountController(IAccountService accountService, ILogger logger, IAuthService authService)
        {
            _accountService = accountService;
            _logger = logger;
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AuthenticationRequest request)
        {
            var response = await _authService.LoginAsync(request);
            SetRefreshTokenInCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenRequestDto request)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            request.RefreshToken = refreshToken;

            var response = await _accountService.VerifyRefreshToken(request);
            if (response == null)
            {
                return BadRequest($"RefreshToken {refreshToken}");
            }
            SetRefreshTokenInCookie(response.Data.RefreshToken);
            return Ok(response);
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10),
                SameSite = SameSiteMode.None,
                // Domain = "https://stores.com",
                Secure = true
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
            Response.Headers.AccessControlAllowCredentials = "true";
        }

        private string GenerateIpAdrress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}