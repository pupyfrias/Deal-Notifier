using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IUnlockablePhoneCarrierServiceAsync
    {
        Task<UnlockablePhoneCarrier> CreateAsync(UnlockablePhoneCarrier entity);
        Task<bool> ExistsAsync(UnlockablePhoneCarrier entity);
    }
}