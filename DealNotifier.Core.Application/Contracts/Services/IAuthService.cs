
using DealNotifier.Core.Application.DTOs.Auth;
using DealNotifier.Core.Application.Wrappers;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IAuthService
    {
        Task<Response<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);
    }
}
