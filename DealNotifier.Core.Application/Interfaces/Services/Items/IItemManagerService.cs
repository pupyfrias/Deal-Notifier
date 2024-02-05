using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services.Items
{
    public interface IItemManagerService
    {
        ConcurrentBag<int> EvaluatedItemIds { get; set; }

        void AddNewItemIdToEvaluatedIdList(IEnumerable<Item> itemListToCreate);

        void SetBrand(ItemDto item);

        Task<(ConcurrentBag<Item> itemListToCreate, ConcurrentBag<Item> itemListToUpdate)> SplitExistingItemsFromNewItems(
            ConcurrentBag<ItemDto> itemsToProcess);
    }
}