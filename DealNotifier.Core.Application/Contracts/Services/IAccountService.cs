using DealNotifier.Core.Application.DTOs.Auth;
using DealNotifier.Core.Application.DTOs.Token;
using DealNotifier.Core.Application.Wrappers;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);

        Task<Response<RefreshTokenResponseDto?>> VerifyRefreshToken(RefreshTokenRequestDto request);
    }
}