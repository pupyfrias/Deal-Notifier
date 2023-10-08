using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneCarrierRepositoryAsync
    {
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);

        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities);
    }
}