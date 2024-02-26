using DealNotifier.Core.Domain.Entities;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IItemRepository : IAsyncRepository<Item>
    {
        Task CreateRangeAsync(IEnumerable<Item> items);
        Task UpdateRangeAsync(IEnumerable<Item> items);

        Task<Item?> GetByPublicIdAsync(Guid id);

        Task DeleteRangeAsync(Expression<Func<Item, bool>> predicate);

        Task<TDestination?> GetByPublicIdProjected<TDestination>(Guid id);

        Task UpdateStockStatusAsync(string query, SqlParameter idListString, SqlParameter onlineStoreId, 
            SqlParameter outputResult, SqlParameter errorMessage);

        Task DeleteByKeyword(string keyword);
        IQueryable<Item> Where(Expression<Func<Item, bool>> predicate);
    }
}