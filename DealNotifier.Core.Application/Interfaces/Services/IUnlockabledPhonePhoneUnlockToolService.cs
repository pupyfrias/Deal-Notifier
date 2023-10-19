using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhonePhoneUnlockToolService
    {

        Task CreateAsync(int unlockedPhoneId, int phoneUnlockToolId);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneUnlockToolDto entity);

        Task CreateIfNotExists(int unlockabledPhoneId, int phoneUnlockToolId);
    }
}