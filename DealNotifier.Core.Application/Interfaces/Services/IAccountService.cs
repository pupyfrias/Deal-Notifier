using DealNotifier.Core.Application.ViewModels.V1.Auth;
using DealNotifier.Core.Application.ViewModels.V1.Token;
using DealNotifier.Core.Application.Wrappers;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);

        Task<Response<RefreshTokenResponseDto?>> VerifyRefreshToken(RefreshTokenRequestDto request);
    }
}