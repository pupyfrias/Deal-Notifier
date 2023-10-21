using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Data;

namespace DealNotifier.Core.Application.Services.Items
{
    public class ItemManagerService : IItemManagerService
    {
        private const decimal _minPriceDifferenceForNotification = 5m;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly IItemValidationService _itemValidationService;
        private readonly IItemNotificationService _itemNotificationService;

        public ConcurrentBag<int> EvaluatedItemIdList { get; set; } = new();

        public ItemManagerService(
            IServiceScopeFactory serviceScopeFactory,
            IMapper mapper,
            IItemValidationService itemValidationService,
            IItemNotificationService itemNotificationService
            )
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _itemValidationService = itemValidationService;
            _itemNotificationService = itemNotificationService;
        }

        public void AddNewItemIdToEvaluatedIdList(IEnumerable<Item> itemListToCreate)
        {
            foreach (var item in itemListToCreate)
            {
                EvaluatedItemIdList.Add(item.Id);
            }
        }


        private void HandleExistingItem(Item oldItem, ItemCreateRequest newItem, ConcurrentBag<Item> itemListToUpdate)
        {
            UpdateOldItemFromNew(oldItem, newItem);
            itemListToUpdate.Add(oldItem);
            EvaluatedItemIdList.Add(oldItem.Id);

            decimal priceDifference = oldItem.Price - newItem.Price;

            if (priceDifference >= _minPriceDifferenceForNotification || (bool)oldItem.IsAuction!)
            {
                _itemNotificationService.EvaluateIfNotifiable(oldItem);
            }
        }

        private void HandleNewItem(ItemCreateRequest itemCreate, ConcurrentBag<Item> itemListToCreate)
        {
            Item newItem = _mapper.Map<Item>(itemCreate);
            itemListToCreate.Add(newItem);
            _itemNotificationService.EvaluateIfNotifiable(newItem);
        }

        public async Task SplitExistingItemsFromNewItems(
            ConcurrentBag<ItemCreateRequest> itemCreateList,
            ConcurrentBag<Item> itemListToCreate,
            ConcurrentBag<Item> itemListToUpdate)
        {
            var tasks = itemCreateList.Select(async itemCreate =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                    var oldItem = await itemRepository.FirstOrDefaultAsync(i => i.Link == itemCreate.Link);

                    if (oldItem == null)
                    {
                        HandleNewItem(itemCreate, itemListToCreate);
                    }
                    else
                    {
                        if (_itemValidationService.CanItemBeUpdated(oldItem, itemCreate))
                        {
                            HandleExistingItem(oldItem, itemCreate, itemListToUpdate);
                        }
                    }
                }
            });

            await Task.WhenAll(tasks);
        }
        private void UpdateItemPrice(Item oldItem, decimal newPrice, decimal oldPrice)
        {
            decimal saving = oldPrice - newPrice;

            if (oldPrice > newPrice)
            {
                oldItem.Saving = saving;
                oldItem.SavingsPercentage = saving / oldPrice * 100;
                oldItem.OldPrice = oldPrice;
            }
            else if (oldPrice < newPrice)
            {
                oldItem.OldPrice = 0;
                oldItem.Saving = 0;
                oldItem.SavingsPercentage = 0;
            }
        }
        private void UpdateOldItemFromNew(Item oldItem, ItemCreateRequest newItem)
        {
            UpdateItemPrice(oldItem, newItem.Price, oldItem.Price);
            oldItem.Notified = null;
            _mapper.Map(newItem, oldItem);
        }
    }
}