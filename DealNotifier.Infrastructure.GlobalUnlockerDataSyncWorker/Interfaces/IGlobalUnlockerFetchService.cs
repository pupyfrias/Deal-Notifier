namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces
{
    public interface IGlobalUnlockerFetchService
    {
        Task<string?> GetPageHTMLAsync(string path);
    }
}