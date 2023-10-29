using ApiGateway.Interfaces;
using ApiGateway.Wrappers;
using AuthProto;

namespace ApiGateway.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthProto.AuthService.AuthServiceClient _authServiceClient;

        public AuthService(AuthProto.AuthService.AuthServiceClient authServiceClient)
        {
            _authServiceClient = authServiceClient;
        }

        public async Task<ApiResponse<AuthenticationResponse>> LoginAsync(AuthenticationRequest request)
        {
            var response = await _authServiceClient.loginAsync(request);            
            return new ApiResponse<AuthenticationResponse>(response);
        }

        public async Task<ApiResponse<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailRequest request)
        {
            var response = await _authServiceClient.ConfirmEmailAsync(request);
            return new ApiResponse<ConfirmEmailResponse>(response);
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest request)
        {
            await _authServiceClient.ForgotPasswordAsync(request);
        }

        public async Task ResetPasswordAsync(ResetPasswordRequest request)
        {
            await _authServiceClient.ResetPasswordAsync(request);
        }
    }
}