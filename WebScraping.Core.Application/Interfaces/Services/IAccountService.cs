using WebScraping.Core.Application.Dtos.Auth;
using WebScraping.Core.Application.Dtos.Token;
using WebScraping.Core.Application.Wrappers;

namespace WebScraping.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);

        Task<Response<RefreshTokenResponseDto?>> VerifyRefreshToken(RefreshTokenRequestDto request);
    }
}