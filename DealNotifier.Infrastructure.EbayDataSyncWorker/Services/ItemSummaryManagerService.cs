using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
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

        public ItemSummaryManagerService(
            IItemValidationService itemValidationService,
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _itemValidationService = itemValidationService;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

        }

        public async Task<ConcurrentBag<ItemCreateRequest>> MapToItemCreatesAsync(List<ItemSummary>? itemSummaries)
        {
            var mappedItemList = new ConcurrentBag<ItemCreateRequest>();

            if (itemSummaries != null)
            {
                var tasks = itemSummaries.Select(async itemSummary =>
                {
                    try
                    {
                        var itemCreate = new ItemCreateRequest();
                        itemCreate.Name = itemSummary.Title;
                        itemCreate.Link = itemSummary.ItemWebUrl.Substring(0, itemSummary.ItemWebUrl.IndexOf("?"));

                        if (_itemValidationService.CanBeSaved(itemCreate))
                        {
                            using (var scope = _serviceScopeFactory.CreateScope())
                            {
                                var unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                                var unlockProbabilityService = scope.ServiceProvider.GetRequiredService<IUnlockProbabilityService>();

                                itemCreate.BidCount = itemSummary!.BidCount;
                                itemCreate.ConditionId = GetConditionId(itemSummary);
                                itemCreate.Image = GetImage(itemSummary);
                                itemCreate.IsAuction = itemSummary.CurrentBidPrice != null;
                                itemCreate.ItemEndDate = itemSummary.ItemEndDate?.AddHours(-4);
                                itemCreate.ItemTypeId = (int)ItemType.Phone;
                                itemCreate.OnlineStoreId = (int)OnlineStore.eBay;
                                itemCreate.Price = GetPrice(itemSummary);
                                itemCreate.StockStatusId = (int)StockStatus.InStock;
                                await unlockabledPhoneService.TryAssignUnlockabledPhoneIdAsync(itemCreate);
                                await unlockProbabilityService.SetUnlockProbabilityAsync(itemCreate);

                                mappedItemList.Add(itemCreate);
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

            return mappedItemList;
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