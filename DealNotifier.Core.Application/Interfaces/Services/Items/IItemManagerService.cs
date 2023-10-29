using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;
using System.Collections.Concurrent;

namespace Catalog.Application.Interfaces.Services.Items
{
    public interface IItemManagerService
    {
        ConcurrentBag<int> EvaluatedItemIdList { get; set; }

        void AddNewItemIdToEvaluatedIdList(IEnumerable<Item> itemListToCreate);

        Task SplitExistingItemsFromNewItems(ConcurrentBag<ItemCreateRequest> itemCreateList,
                            ConcurrentBag<Item> itemListToCreate, ConcurrentBag<Item> itemListToUpdate);
    }
}