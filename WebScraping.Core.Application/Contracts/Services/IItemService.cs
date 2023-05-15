using System.Collections.Concurrent;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IItemService
    {
        ConcurrentBag<Item> itemToNotifyList { get; set; }

        Task LoadData();

        Task NotifyByEmail();

        void SaveOrUpdate(ref ConcurrentBag<Item> items);

        void UpdateStatus(ref ConcurrentBag<string> checkedList);
    }
}