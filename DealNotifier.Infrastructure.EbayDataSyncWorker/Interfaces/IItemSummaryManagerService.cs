using DealNotifier.Core.Application.ViewModels.eBay;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using System.Collections.Concurrent;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces
{
    public interface IItemSummaryManagerService
    {
        Task<ConcurrentBag<ItemDto>> MapToItemAsync(List<ItemSummary>? itemSummaries);
    }
}