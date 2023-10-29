namespace Catalog.Application.Interfaces.Services
{
    public interface IUnlockabledPhonePhoneCarrierService
    {
        Task CreateMassiveAsync(int unlockedPhoneId, string carriers);
    }
}