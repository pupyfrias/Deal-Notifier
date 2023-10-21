

using DealNotifier.Core.Application.ViewModels.eBay;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces
{
    public interface IEbayFetchService

    {
        Task<EBayResponse?> GetItemsAsync(string url);
    }
}