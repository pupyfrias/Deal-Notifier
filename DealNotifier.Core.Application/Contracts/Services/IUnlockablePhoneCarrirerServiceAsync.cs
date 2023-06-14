using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IUnlockablePhoneCarrierServiceAsync
    {
        Task<UnlockablePhonePhoneCarrier> CreateAsync(UnlockablePhonePhoneCarrier entity);
        Task<bool> ExistsAsync(UnlockablePhonePhoneCarrier entity);
    }
}