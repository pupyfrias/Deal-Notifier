using Microsoft.AspNetCore.Mvc;
using WebScraping.Core.Application.DTOs.Auth;
using WebScraping.Core.Application.DTOs.Token;
using WebScraping.Core.Application.Interfaces.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;


        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(AuthenticationRequest request)
        {
            var response = await _accountService.LoginAsync(request);
            SetRefreshTokenInCookie(response.Data.RefreshToken);
            return Ok(response);

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync(RefreshTokenRequestDTO request) 
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest();
            }

            request.RefreshToken = refreshToken;
            var response = await _accountService.VerifyRefreshToken(request);
            if(response == null)
            {
                return BadRequest();
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
                Secure= true
                
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
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
