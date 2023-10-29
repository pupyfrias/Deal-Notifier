using Catalog.Domain.Entities;
using System.Linq.Expressions;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneCarrierRepository
    {


        #region Async
        Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity);

        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneCarrier> entities);
        Task<UnlockabledPhonePhoneCarrier?> FirstOrDefaultAsync(Expression<Func<UnlockabledPhonePhoneCarrier, bool>> predicate);
        #endregion Async

        #region Sync
        bool Exists(UnlockabledPhonePhoneCarrier entity);

        UnlockabledPhonePhoneCarrier? FirstOrDefault(Expression<Func<UnlockabledPhonePhoneCarrier, bool>> predicate);
        #endregion Sync
    }
}