using DealNotifier.Core.Application.Enums;

namespace WorkerService.T_Unlock_WebScraping.Interfaces
{
    public interface ITUnlockPhoneProcessService
    {
        Task ProcessAsync(string pageHtml, Brand brand);
    }
}