using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IUnlockablePhoneCarrierServiceAsync
    {
        Task<UnlockablePhonePhoneCarrier> CreateAsync(UnlockablePhonePhoneCarrier entity);
        Task<bool> ExistsAsync(UnlockablePhonePhoneCarrier entity);
    }
}