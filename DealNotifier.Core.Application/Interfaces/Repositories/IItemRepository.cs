using Catalog.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task CreateRangeAsync(IEnumerable<Item> items);
        Task UpdateRangeAsync(IEnumerable<Item> items);

        Task<Item?> GetByPublicIdAsync(Guid id);

        Task<TDestination?> GetByPublicIdProjected<TDestination>(Guid id);

        Task UpdateStockStatusAsync(string query, SqlParameter idListString, SqlParameter onlineStoreId,
            SqlParameter outputResult, SqlParameter errorMessage);
    }
}