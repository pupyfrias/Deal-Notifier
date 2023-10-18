using DealNotifier.Core.Application.ViewModels.V1.Item;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IItemSyncService
    {
        bool CanBeSaved(ItemCreateRequest itemCreate);

        Task LoadNecessaryDataAsync();

        Task SaveOrUpdateAsync(ConcurrentBag<ItemCreateRequest> itemCreateList);

        Task SetUnlockProbabilityAsync(ItemCreateRequest itemCreate);
        Task TryAssignUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate);

        Task UpdateStockStatusAsync(Enums.OnlineStore onlineStore);

    }
}