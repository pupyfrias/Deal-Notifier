using DealNotifier.Core.Application.ViewModels.V1.Item;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockVerificationService
    {
        Task<bool> CanBeUnlockedBasedOnModelNameAsync(ItemCreateRequest itemCreate);
        Task<bool> CanBeUnlockedBasedOnUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate);
    }
}