using Catalog.Application.ViewModels.V1.Item;
using DealNotifier.Infrastructure.EbayDataSyncWorker.ViewModels;
using System.Collections.Concurrent;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces
{
    public interface IItemSummaryManagerService
    {
        Task<ConcurrentBag<ItemCreateRequest>> MapToItemCreatesAsync(List<ItemSummary>? itemSummaries);
    }
}