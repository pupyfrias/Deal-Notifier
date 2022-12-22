using WebScraping.Core.Application.DTOs.Auth;
using WebScraping.Core.Application.DTOs.Token;
using WebScraping.Core.Application.Wrappers;

namespace WebScraping.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);
        Task<Response<RefreshTokenResponseDTO?>> VerifyRefreshToken(RefreshTokenRequestDTO request);
    }
}
