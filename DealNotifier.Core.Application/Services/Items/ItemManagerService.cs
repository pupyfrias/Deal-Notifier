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
        private readonly IItemNotificationService _itemNotificationService;
        private readonly IItemValidationService _itemValidationService;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;
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

        public ConcurrentBag<int> EvaluatedItemIds { get; set; } = new();
        public void AddNewItemIdToEvaluatedIdList(IEnumerable<Item> itemListToCreate)
        {
            foreach (var item in itemListToCreate)
            {
                EvaluatedItemIds.Add(item.Id);
            }
        }

        public void SetBrand(ItemDto item)
        {
            int brandId = Enum.GetNames(typeof(Enums.Brand))
                   .Select((e, i) => new { Name = e, Id = i + 1 })
                   .Where(e => item.Title.Contains(e.Name, StringComparison.OrdinalIgnoreCase))
                   .Select(e => e.Id)
                   .FirstOrDefault();

            item.BrandId = brandId != 0 ? brandId : (int) Enums.Brand.Unknown;
        }


        public async Task<(ConcurrentBag<Item>, ConcurrentBag<Item>)> SplitExistingItemsFromNewItems(ConcurrentBag<ItemDto> itemsToProcess)
        {

            ConcurrentBag<Item> itemsToCreate = new();
            ConcurrentBag<Item> itemsToUpdate = new();

            var tasks = itemsToProcess.Select(async item =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var itemRepository = scope.ServiceProvider.GetRequiredService<IItemRepository>();
                    var oldItem = await itemRepository.FirstOrDefaultAsync(i => i.Link == item.Link);

                    if (oldItem == null)
                    {
                        HandleNewItem(item, itemsToCreate);
                    }
                    else
                    {
                        HandleExistingItem(oldItem, item, itemsToUpdate);
                    }
                }
            });

            await Task.WhenAll(tasks);
            return (itemsToCreate, itemsToUpdate);
        }

        private void HandleExistingItem(Item oldItem, ItemDto newItem, ConcurrentBag<Item> itemsToUpdate)
        {
            EvaluatedItemIds.Add(oldItem.Id);
            
            if (!_itemValidationService.CanItemBeUpdated(oldItem, newItem)) return;
            
            UpdateOldItemFromNew(oldItem, newItem);
            itemsToUpdate.Add(oldItem);

            decimal priceDifference = oldItem.Price - newItem.Price;

            if (priceDifference >= _minPriceDifferenceForNotification || (bool)oldItem.IsAuction!)
            {
                _itemNotificationService.EvaluateIfNotifiable(oldItem);
            }
        }

        private void HandleNewItem(ItemDto itemCreate, ConcurrentBag<Item> itemsToCreate)
        {
            Item newItem = _mapper.Map<Item>(itemCreate);
            itemsToCreate.Add(newItem);
            _itemNotificationService.EvaluateIfNotifiable(newItem);
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
        private void UpdateOldItemFromNew(Item oldItem, ItemDto newItem)
        {
            UpdateItemPrice(oldItem, newItem.Price, oldItem.Price);
            oldItem.Notified = null;
            _mapper.Map(newItem, oldItem);
        }
    }
}