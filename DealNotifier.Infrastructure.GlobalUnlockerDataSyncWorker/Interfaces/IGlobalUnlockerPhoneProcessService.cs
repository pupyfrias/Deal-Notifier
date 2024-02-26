using DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces
{
    public interface IGlobalUnlockerPhoneProcessService
    {
        Task ProcessAsync(string pageHtml, Brand brand);
    }
}