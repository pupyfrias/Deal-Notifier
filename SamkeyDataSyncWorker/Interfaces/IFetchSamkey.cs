using SamkeyDataSyncWorker.ViewModels;

namespace SamkeyDataSyncWorker.Interfaces
{
    public interface IFetchSamkey
    {
        Task<List<string>?> GetPhoneModelsAsync();
        Task<PhoneDetailsResponse?> GetPhoneDetailsAsync(string modelNumber);
    }
}