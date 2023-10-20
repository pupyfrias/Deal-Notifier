using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Domain.Entities;
using WorkerService.T_Unlock_WebScraping.ViewModels;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhoneService : IGenericService<UnlockabledPhone>
    {
        Task HandleNewUnlockedPhoneAsync(UnlockedPhoneDetailsDto unlockedPhoneDetails, Enums.Brand brand, UnlockTool unlockTool);
        Task HandleExistingUnlockedPhoneAsync(UnlockabledPhone possibleUnlockedPhone, string carriers, UnlockTool unlockTool);
    }
}