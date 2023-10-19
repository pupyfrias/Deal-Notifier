

namespace WorkerService.T_Unlock_WebScraping.Interfaces
{
    public interface IFetchTUnlock
    {
        Task<string?> GetPageHTMLAsync(string path);
    }
}