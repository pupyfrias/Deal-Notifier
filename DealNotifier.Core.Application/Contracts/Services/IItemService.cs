using DealNotifier.Core.Application.DTOs.Item;
using System.Collections.Concurrent;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IItemService
    {
        ConcurrentBag<Item> itemToNotifyList { get; set; }

        Task LoadData();

        Task NotifyByEmail();

        void SaveOrUpdate(in ConcurrentBag<ItemCreateDto> items);

        void UpdateStatus(ref ConcurrentBag<string> checkedList);

        void TrySetModelNumberModelNameAndBrand(ref ItemCreateDto item);

        void SetUnlockProbability(ref ItemCreateDto item);
    }
}