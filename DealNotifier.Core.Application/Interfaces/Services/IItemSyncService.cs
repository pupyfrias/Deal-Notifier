using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IItemSyncService
    {
        Task<bool> CanBeSavedAsync(ItemCreateRequest itemCreate);

        Task LoadDataAsync();

        Task SaveOrUpdateAsync(ConcurrentBag<ItemCreateRequest> itemCreateList);

        Task SetUnlockProbabilityAsync(ItemCreateRequest itemCreate);
        Task TryAssignUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate);

        Task UpdateStockStatusAsync(Enums.OnlineStore onlineStore);

    }
}