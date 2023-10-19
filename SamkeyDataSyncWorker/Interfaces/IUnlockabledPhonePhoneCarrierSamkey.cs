namespace SamkeyDataSyncWorker.Interfaces
{
    public interface IUnlockabledPhonePhoneCarrierSamkey
    {
        Task CreateMassive(int unlockedPhoneId, string supportCarriers);
    }
}