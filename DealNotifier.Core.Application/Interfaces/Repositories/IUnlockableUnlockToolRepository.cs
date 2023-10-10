using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneCarrierRepository
    {


        #region Async
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);

        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities);
        #endregion Async

        #region Sync
        bool Exists(UnlockabledPhonePhoneCarrier entity);
        #endregion Sync
    }
}