namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces
{
    public interface ISamkeyPhoneProcessService
    {
        Task ProcessAsync(List<string> phoneList);
    }
}