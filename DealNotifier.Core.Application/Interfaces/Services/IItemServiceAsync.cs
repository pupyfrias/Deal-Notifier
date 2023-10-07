using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IItemServiceAsync : IGenericServiceAsync<Item, Guid>
    {
        ConcurrentBag<Item> itemToNotifyList { get; set; }

        Task LoadData();

        Task NotifyByEmail();

        void SaveOrUpdate(in ConcurrentBag<ItemCreateRequest> items);

        void UpdateStatus(ref ConcurrentBag<string> checkedList);

        void TrySetModelNumberModelNameAndBrand(ref ItemCreateRequest item);

        void SetUnlockProbability(ref ItemCreateRequest item);
    }
}