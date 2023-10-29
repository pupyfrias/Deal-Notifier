using Catalog.Application.ViewModels.V1.Auth;
using Catalog.Application.ViewModels.V1.Token;
using Catalog.Application.Wrappers;

namespace Catalog.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);

        Task<Response<RefreshTokenResponseDto?>> VerifyRefreshToken(RefreshTokenRequestDto request);
    }
}