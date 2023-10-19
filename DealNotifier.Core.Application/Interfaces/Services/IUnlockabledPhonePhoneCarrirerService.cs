using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhonePhoneCarrierService
    {
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrierCreateRequest entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrierDto entity);

        Task CreateRangeAsync(int phoneCarrierId, PhoneCarrierUnlockabledPhoneCreateRequest request);

        Task CreateIfNotExists(int unlockabledPhoneId, int phoneCarrierId);
    }
}