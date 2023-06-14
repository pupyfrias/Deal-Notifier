using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Repositories
{
    public interface IUnlockablePhoneCarrierRepositoryAsync
    {
        Task<UnlockablePhonePhoneCarrier> CreateAsync(UnlockablePhonePhoneCarrier entity);
        Task<bool> ExistsAsync(UnlockablePhonePhoneCarrier entity);

    }
}
