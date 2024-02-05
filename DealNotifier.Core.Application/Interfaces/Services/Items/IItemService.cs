using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;
using OnlineStore = DealNotifier.Core.Application.Enums.OnlineStore;

namespace DealNotifier.Core.Application.Interfaces.Services.Items
{
    public interface IItemService : IAsyncService<Item>
    {
        Task<TDestination?> GetByPublicIdProjectedAsync<TDestination>(Guid id);
        Task UpdateAsync<TSource>(Guid id, TSource source) where TSource : IHasId<Guid>;
        Task DeleteAsync(Guid id);
        Task BulkDeleteAsync(IEnumerable<int> ids);
        Task SaveOrUpdateRangeAsync(ConcurrentBag<ItemDto> itemsToProcess);
        Task UpdateStockStatusAsync(OnlineStore onlineStore);
    }
}