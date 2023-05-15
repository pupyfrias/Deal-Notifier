using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Repositories
{
    public interface IUnlockablePhoneCarrierRepositoryAsync
    {
        Task<UnlockablePhoneCarrier> CreateAsync(UnlockablePhoneCarrier entity);
        Task<bool> ExistsAsync(UnlockablePhoneCarrier entity);

    }
}
