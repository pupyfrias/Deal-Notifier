using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services.Items
{
    public interface IItemManagerService
    {
        ConcurrentBag<int> EvaluatedItemIdList { get; set; }

        void AddNewItemIdToEvaluatedIdList(IEnumerable<Item> itemListToCreate);

        Task SplitExistingItemsFromNewItems(ConcurrentBag<ItemCreateRequest> itemCreateList,
                            ConcurrentBag<Item> itemListToCreate, ConcurrentBag<Item> itemListToUpdate);
    }
}