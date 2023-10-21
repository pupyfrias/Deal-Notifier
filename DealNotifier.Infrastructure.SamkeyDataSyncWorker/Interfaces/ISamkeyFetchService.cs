using DealNotifier.Infrastructure.SamkeyDataSyncWorker.ViewModels;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces
{
    public interface ISamkeyFetchService
    {
        Task<List<string>?> GetPhoneModelsAsync();
        Task<PhoneDetailsResponse?> GetPhoneDetailsAsync(string modelNumber);
    }
}