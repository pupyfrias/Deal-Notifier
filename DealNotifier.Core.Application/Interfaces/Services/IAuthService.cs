using Catalog.Application.ViewModels.V1.Auth;
using Catalog.Application.Wrappers;

namespace Catalog.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);
    }
}