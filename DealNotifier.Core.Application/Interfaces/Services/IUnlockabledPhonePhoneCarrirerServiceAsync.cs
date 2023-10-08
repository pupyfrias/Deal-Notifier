using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhonePhoneCarrierServiceAsync
    {
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);

        Task CreateRangeAsync(int PhoneCarrierId, PhoneCarrierUnlockabledPhoneCreateRequest request);
    }
}