using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Repositories
{
    public interface IUnlockablePhoneCarrierRepositoryAsync
    {
        Task<UnlockablePhonePhoneCarrier> CreateAsync(UnlockablePhonePhoneCarrier entity);
        Task<bool> ExistsAsync(UnlockablePhonePhoneCarrier entity);

    }
}
