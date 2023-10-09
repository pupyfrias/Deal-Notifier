using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IItemRepository : IGenericRepository<Item, Guid>
    {
        Task CreateRangeAsync(IEnumerable<Item> items);
        Task UpdateRangeAsync(IEnumerable<Item> items);
    }
}