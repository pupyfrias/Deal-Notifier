namespace WorkerService.T_Unlock_WebScraping.Interfaces
{
    public interface IUnlockabledPhonePhoneCarrierTUnlock
    {
        Task CreateMassive(int unlockedPhoneId, string carriers);
    }
}