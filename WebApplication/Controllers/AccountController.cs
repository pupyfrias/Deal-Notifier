using Microsoft.AspNetCore.Mvc;
using WebScraping.Core.Application.DTOs.Account;
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

        [HttpPost("authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            string ipAddress = GenerateIpAdrress();
            var response = await _accountService.AuthenticateAsync(request, ipAddress);

            return Ok(response);

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
