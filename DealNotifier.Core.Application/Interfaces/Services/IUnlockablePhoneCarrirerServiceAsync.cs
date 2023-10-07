using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockablePhoneCarrierServiceAsync
    {
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);
    }
}