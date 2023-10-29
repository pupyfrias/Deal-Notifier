using Catalog.Application.Enums;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Application.ViewModels.V1.UnlockabledPhone;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Services
{
    public interface IUnlockabledPhoneService : IGenericService<UnlockabledPhone>
    {
        Task HandleNewUnlockedPhoneAsync(UnlockedPhoneDetailsDto unlockedPhoneDetails, Enums.Brand brand, UnlockTool unlockTool);
        Task HandleExistingUnlockedPhoneAsync(UnlockabledPhone possibleUnlockedPhone, string carriers, UnlockTool unlockTool);

        Task TryAssignUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate);
    }
}