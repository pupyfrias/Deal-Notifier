using ApiGateway.Wrappers;
using AuthProto;

namespace ApiGateway.Interfaces
{
    public interface IAuthService
    {
        Task<ApiResponse<ConfirmEmailResponse>> ConfirmEmailAsync(ConfirmEmailRequest request);

        Task ForgotPasswordAsync(ForgotPasswordRequest request);

        Task<ApiResponse<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);

        Task ResetPasswordAsync(ResetPasswordRequest request);
    }
}