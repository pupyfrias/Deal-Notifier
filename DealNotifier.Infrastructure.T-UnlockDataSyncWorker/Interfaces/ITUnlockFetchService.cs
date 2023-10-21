

namespace WorkerService.T_Unlock_WebScraping.Interfaces
{
    public interface ITUnlockFetchService
    {
        Task<string?> GetPageHTMLAsync(string path);
    }
}