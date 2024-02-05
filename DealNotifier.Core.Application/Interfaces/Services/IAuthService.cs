using DealNotifier.Core.Application.ViewModels.V1.Auth;
using DealNotifier.Core.Application.Wrappers;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthenticationResponse>> LoginAsync(AuthenticationRequest request);
    }
}