using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneCarrierRepository
    {
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);

        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities);
    }
}