using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockablePhoneCarrierRepositoryAsync
    {
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);
    }
}