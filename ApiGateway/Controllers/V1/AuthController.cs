using ApiGateway.Interfaces;
using ApiGateway.Wrappers;
using Asp.Versioning;
using AuthProto;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/[controller]")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> LoginAsync(AuthenticationRequest request)
        {
            var response = await _authService.LoginAsync(request);
            return Ok(response);
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult<ApiResponse<ConfirmEmailResponse>>> ConfirmEmailAsync([FromQuery] ConfirmEmailRequest request)
        {
            var response = await _authService.ConfirmEmailAsync(request);
            return Ok(response);
        }

        [HttpPost("forgot-password")]
        public async Task<ActionResult<string>> ForgotPassword(ForgotPasswordRequest request)
        {
            await _authService.ForgotPasswordAsync(request);
            return NoContent();
        }

        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword(ResetPasswordRequest request)
        {
             await _authService.ResetPasswordAsync(request);
            return NoContent();
        }

    }
}
