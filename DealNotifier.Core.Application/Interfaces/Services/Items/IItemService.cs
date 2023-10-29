using Catalog.Application.Interfaces;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;
using System.Collections.Concurrent;
using OnlineStore = Catalog.Application.Enums.OnlineStore;

namespace Catalog.Application.Interfaces.Services.Items
{
    public interface IItemService : IGenericService<Item>
    {
        Task<TDestination?> GetByPublicIdProjectedAsync<TDestination>(Guid id);
        Task UpdateAsync<TSource>(Guid id, TSource source) where TSource : IHasId<Guid>;
        Task DeleteAsync(Guid id);
        Task SaveOrUpdateRangeAsync(ConcurrentBag<ItemCreateRequest> itemCreateList);
        Task UpdateStockStatusAsync(OnlineStore onlineStore);
    }
}