using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhoneService : IAsyncService<UnlockabledPhone>
    {
        Task HandleNewUnlockedPhoneAsync(UnlockedPhoneDetailsDto unlockedPhoneDetails, Enums.Brand brand, UnlockTool unlockTool);
        Task HandleExistingUnlockedPhoneAsync(UnlockabledPhone possibleUnlockedPhone, string carriers, UnlockTool unlockTool);

        Task TryAssignUnlockabledPhoneIdAsync(ItemDto itemCreate);
    }
}