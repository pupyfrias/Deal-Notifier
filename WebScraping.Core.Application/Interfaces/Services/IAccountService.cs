using WebScraping.Core.Application.DTOs.Account;
using WebScraping.Core.Application.Wrappers;

namespace WebScraping.Core.Application.Interfaces.Services
{
    public interface IAccountService
    {
        Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress);
    }
}
