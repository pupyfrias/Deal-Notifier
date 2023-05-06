using System.Collections.Concurrent;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Interfaces.Services
{
    public interface IItemService
    {
        Task LoadData();

        void SaveOrUpdate(ref ConcurrentBag<Item> items);

        void UpdateStatus(ref ConcurrentBag<string> checkedList);
    }
}