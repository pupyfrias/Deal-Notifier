using DealNotifier.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task CreateRangeAsync(IEnumerable<Item> items);
        Task UpdateRangeAsync(IEnumerable<Item> items);

        Task<Item?> GetByPublicIdAsync(Guid id);

        Task<TDestination?> GetByPublicIdProjected<TDestination>(Guid id);
    }
}