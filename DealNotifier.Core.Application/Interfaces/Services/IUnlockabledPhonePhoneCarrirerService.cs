namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhonePhoneCarrierService
    {
        Task CreateMassiveAsync(int unlockedPhoneId, string carriers);
    }
}