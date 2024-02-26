using DealNotifier.Core.Application.Configs;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Infrastructure.EbayDataSyncWorker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.EbayDataSyncWorker.Services
{
    public class EbayPhoneProcessService : IEbayPhoneProcessService
    {
        private readonly EbayUrlConfig _ebayUrlConfig;
        private readonly IEbayFetchService _ebayFetchService;
        private readonly IItemSummaryManagerService _itemSummaryManagerService;
        private readonly IItemService _itemService;
        private readonly ILogger _logger;
        private readonly IItemDependencyLoaderService _itemDependencyLoaderService;
        private readonly IItemNotificationService _itemNotificationService;
        public EbayPhoneProcessService(
            IEmailService emailService,
            IEbayFetchService fetchEbayService,
            IItemSummaryManagerService itemSummaryManagerService,
            ILogger logger,
            IOptions<EbayUrlConfig> ebayUrlConfig,
            IItemDependencyLoaderService itemDependencyLoaderService,
            IItemService itemService,
            IServiceScopeFactory serviceScopeFactory

            )
        {
            _ebayUrlConfig = ebayUrlConfig.Value;
            _ebayFetchService = fetchEbayService;
            _itemSummaryManagerService = itemSummaryManagerService;
            _logger = logger;
            _itemDependencyLoaderService = itemDependencyLoaderService;
            _itemNotificationService = serviceScopeFactory.CreateScope()
                 .ServiceProvider.GetRequiredService<IItemNotificationService>();
            _itemService = itemService;
        }

        public async Task ProcessAsync()
        {
            try
            {
                await _itemDependencyLoaderService.LoadDataAsync();
                string baseUrl = _ebayUrlConfig.Base;

                foreach (var path in _ebayUrlConfig.Paths.Search)
                {
                    string url = $"{baseUrl}{path}";
                    await ProcessURLAsync(url);
                    _logger.Information($"Processed URL: {url}");
                }

                await _itemService.UpdateStockStatusAsync(OnlineStore.eBay);
                await _itemNotificationService.NotifyUsersOfItemsByEmail();
            }
            catch (Exception ex)
            {
                _logger.Error($"An error occurred while processing: {ex.Message}, StackTrace: {ex.StackTrace}");
            }
        }

        private async Task ProcessURLAsync(string url)
        {
            string? currentUrl = url;
            do
            {
                var ebayResponse = await _ebayFetchService.GetItemsAsync(currentUrl);
                if (ebayResponse == null) break;
                var itemsToProcess = await _itemSummaryManagerService.MapToItemAsync(ebayResponse.ItemSummaries);
                await _itemService.SaveOrUpdateRangeAsync(itemsToProcess);



                if (ebayResponse.Total < ebayResponse.Limit + ebayResponse.Offset)
                {
                    _logger.Information($"total itmes: {ebayResponse.Total}. limit: {ebayResponse.Limit}. offset: {ebayResponse.Offset}");
                    break;
                }
                currentUrl = ebayResponse.Next;
            }
            while (currentUrl != null);

            
        }
    }
}