using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.ViewModels.eBay;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces;
using System.Collections.Concurrent;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Services
{
    public class ItemSummaryManagerService : IItemSummaryManagerService
    {
        private readonly ILogger _logger;
        private readonly IItemValidationService _itemValidationService;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IItemManagerService _itemManagerService;
        private readonly ICacheDataService _cacheDataService;

        public ItemSummaryManagerService(
            ILogger logger,
            IItemValidationService itemValidationService,
            IServiceScopeFactory serviceScopeFactory, 
            IItemManagerService itemManagerService,
            ICacheDataService cacheDataService
            )
        {
            _logger = logger;
            _itemValidationService = itemValidationService;
            _serviceScopeFactory = serviceScopeFactory;
            _itemManagerService = itemManagerService;
            _cacheDataService = cacheDataService;
        }

        public async Task<ConcurrentBag<ItemDto>> MapToItemAsync(List<ItemSummary>? itemSummaries)
        {
            var mappedItems = new ConcurrentBag<ItemDto>();

            if (itemSummaries != null)
            {
                var tasks = itemSummaries.Select(async itemSummary =>
                {
                    try
                    {
                        var item = new ItemDto();
                        item.Title = itemSummary.Title;
                        item.Link = itemSummary.ItemWebUrl.Substring(0, itemSummary.ItemWebUrl.IndexOf("?"));
                        
                        if (_itemValidationService.CanBeSaved(item))
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                                var unlockProbabilityService = scope.ServiceProvider.GetRequiredService<IUnlockProbabilityService>();

                                item.ShortDescription = itemSummary.ShortDescription;
                                item.BidCount = itemSummary!.BidCount;
                                item.ConditionId = GetConditionId(itemSummary);
                                item.Image = GetImage(itemSummary);
                                item.IsAuction = itemSummary.CurrentBidPrice != null;
                                item.ItemEndDate = itemSummary.ItemEndDate?.AddHours(-4);
                                item.ItemTypeId = (int)ItemType.Phone;
                                item.OnlineStoreId = (int)OnlineStore.eBay;
                                item.Price = GetPrice(itemSummary);
                                item.StockStatusId = (int)StockStatus.InStock;
                                _itemManagerService.SetBrand(item);
                                await unlockabledPhoneService.TryAssignUnlockabledPhoneIdAsync(item);
                                await unlockProbabilityService.SetUnlockProbabilityAsync(item);
                                mappedItems.Add(item);
                                _cacheDataService.CheckedList.Add(item.Link);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Warning($"Exception occurred while processing item: {itemSummary.Title}, Error: {e.Message}");
                    }
                });

                await Task.WhenAll(tasks);
            }
            else
            {
                _logger.Warning("itemSummaries is null");
            }

            return mappedItems;
        }

        private decimal GetPrice(ItemSummary itemSummary)
        {
            decimal price = 0;
            bool isAuction = decimal.TryParse(itemSummary.CurrentBidPrice?.Value, out decimal currentBidPrice);

            if (isAuction) price = currentBidPrice;
            else decimal.TryParse(itemSummary.Price.Value, out price);
            decimal.TryParse(itemSummary.ShippingOptions?[0]?.ShippingCost?.Value, out decimal shippingCost);
            price += shippingCost;

            return price;
        }

        private int GetConditionId(ItemSummary itemSummary)
        {
            return itemSummary.Condition == "New" ? (int)Condition.New : (int)Condition.Used;
        }

        private string GetImage(ItemSummary itemSummary)
        {
            return itemSummary.ThumbnailImages?[0]?.ImageUrl ?? itemSummary.Image?.ImageUrl ?? string.Empty;
        }
    }
}