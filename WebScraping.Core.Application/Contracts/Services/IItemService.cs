using System.Collections.Concurrent;
using WebScraping.Core.Application.Dtos.Item;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IItemService
    {
        ConcurrentBag<Item> itemToNotifyList { get; set; }

        Task LoadData();

        Task NotifyByEmail();

        void SaveOrUpdate(ref ConcurrentBag<ItemCreateDto> items);

        void UpdateStatus(ref ConcurrentBag<string> checkedList);

        void TrySetModelNumberModelNameAndBrand(ref ItemCreateDto item);

        void SetUnlockProbability(ref ItemCreateDto item);
    }
}