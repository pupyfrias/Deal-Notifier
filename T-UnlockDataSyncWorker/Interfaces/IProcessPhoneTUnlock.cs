using DealNotifier.Core.Application.Enums;

namespace WorkerService.T_Unlock_WebScraping.Interfaces
{
    public interface IProcessPhoneTUnlock
    {
        Task ProcessAsync(string pageHtml, Brand brand);
    }
}