using Catalog.Application.ViewModels.V1.Item;

namespace Catalog.Application.Interfaces.Services
{
    public interface IUnlockVerificationService
    {
        Task<bool> CanBeUnlockedBasedOnModelNameAsync(ItemCreateRequest itemCreate);
        Task<bool> CanBeUnlockedBasedOnUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate);
    }
}