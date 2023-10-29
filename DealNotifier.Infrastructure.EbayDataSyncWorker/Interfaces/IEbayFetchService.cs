using DealNotifier.Infrastructure.EbayDataSyncWorker.ViewModels;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces
{
    public interface IEbayFetchService

    {
        Task<EBayResponse?> GetItemsAsync(string url);
    }
}