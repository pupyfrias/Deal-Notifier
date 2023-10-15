using Azure.Core;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Services;
using DealNotifier.Core.Application.ViewModels.eBay;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;
using DealNotifier.Core.Application.Enums;
using System.Collections.Concurrent;
using DealNotifier.Core.Application.Extensions;
using System.Xml.Linq;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;
using DealNotifier.Infrastructure.Persistence.Repositories;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using DealNotifier.Core.Application.Interfaces.Repositories;
using System.Diagnostics;
using DealNotifier.Core.Application.Constants;

namespace EbayDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ConcurrentBag<ItemCreateRequest> _itemList = new ConcurrentBag<ItemCreateRequest>();

        private IItemSyncService _itemSyncService;
        private readonly EbayUrlConfig _ebayUrlConfig;
        private readonly HttpClient _httpClient;
        private IEmailService _emailService;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly string _baseUrl;
        private string? _currentUrl = string.Empty;
        private Timer _timer;

        public Worker(
            ILogger logger, 
            IOptions<EbayUrlConfig> ebayUrlConfig,
            IServiceScopeFactory serviceScopeFactory,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory
            )
        {
            _logger = logger;
            _ebayUrlConfig = ebayUrlConfig.Value;
            _baseUrl = _ebayUrlConfig.Base;
            _serviceScopeFactory = serviceScopeFactory;
            _memoryCache = memoryCache;
            _httpClient = httpClientFactory.CreateClient();

        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Starting ExecuteAsync.");
            TimerCallback callback = async (state) => await TimerElapsed();

            _timer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            await Task.Delay(-1, stoppingToken);
            _logger.Information("ExecuteAsync finished.");
        }

        private async Task TimerElapsed()
        {
            _logger.Information($"Timer elapsed. Running eBay Service Init. {DateTime.Now}");
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _itemSyncService = scope.ServiceProvider.GetRequiredService<IItemSyncService>();
                _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await InitAsync();
            }

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;

            _logger.Information($"Time taken: {timeTaken.ToString(@"m\:ss\.fff")}");
            _logger.Information("eBay Service Init completed.");
        }


        public async Task InitAsync()
        {
            string searchUrl = _baseUrl+ _ebayUrlConfig.Paths.Search.First();
            await _itemSyncService.LoadDataAsync();
            await RunAsync(searchUrl);
        }

        private async Task RunAsync(string? url)
        {
                _currentUrl = url;
                _memoryCache.TryGetValue("accessToken", out string? accessToken);

                if(string.IsNullOrEmpty(accessToken)) 
                {
                    await RefreshTokenAsync();
                    await RunAsync(_currentUrl);
                    return;
                }


                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync(_currentUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<EBayResponse>();

                    await ItemMapperAsync(content?.ItemSummaries);
                    await _itemSyncService.SaveOrUpdateAsync(_itemList);
                    _currentUrl = content?.Next;

                    if(_currentUrl != null)
                    {
                        await RunAsync(_currentUrl);
                        return;
                    }

                     await _itemSyncService.UpdateStockStatusAsync(OnlineStore.eBay);
                     await _emailService.NotifyUsersOfItemsByEmail();
                    
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _logger.Information("Unauthorized");
                    await RefreshTokenAsync();
                    await RunAsync(_currentUrl);

                }
                else
                {
                    _logger.Warning($"{(int)response.StatusCode} | {response.ReasonPhrase}");
                }
            

        }

        #region Private Methods

        private async Task ItemMapperAsync(List<ItemSummary>? itemSummaries)
        {
            if (itemSummaries != null)
            {
                var tasks = itemSummaries.Select(async element =>
                {
                    try
                    {
                        using (var scope = _serviceScopeFactory.CreateScope())
                        {
                            var itemSyncService = scope.ServiceProvider.GetRequiredService<IItemSyncService>();

                            var itemCreate = new ItemCreateRequest();
                            itemCreate.Name = element.Title;
                            itemCreate.Link = element.ItemWebUrl.Substring(0, element.ItemWebUrl.IndexOf("?"));

                            if (await itemSyncService.CanBeSavedAsync(itemCreate))
                            {
                                decimal price = 0;
                                bool isAuction = decimal.TryParse(element.CurrentBidPrice?.Value, out decimal currentBidPrice);

                                if (isAuction)
                                {
                                    price = currentBidPrice;
                                }
                                else
                                {
                                    decimal.TryParse(element.Price.Value, out price);
                                }


                                decimal.TryParse(element.ShippingOptions?[0]?.ShippingCost?.Value, out decimal shippingCost);
                                price += shippingCost;

                                itemCreate.BidCount = element!.BidCount;
                                itemCreate.ConditionId = GetCondition(element);
                                itemCreate.Image = GetImage(element);
                                itemCreate.IsAuction = isAuction;
                                itemCreate.ItemEndDate = element.ItemEndDate?.AddHours(-4);
                                itemCreate.ItemTypeId = (int)ItemType.Phone;
                                itemCreate.OnlineStoreId = (int)OnlineStore.eBay;
                                itemCreate.Price = price;
                                itemCreate.StockStatusId = (int)StockStatus.InStock;
                                await itemSyncService.TryAssignUnlockabledPhoneIdAsync(itemCreate);
                                await itemSyncService.SetUnlockProbabilityAsync(itemCreate);

                                _itemList.Add(itemCreate);
                            }
                        }
                       
                    }
                    catch (Exception e)
                    {
                        _logger.Warning(e.Message);
                    }
                });

                await Task.WhenAll(tasks);
            }
            else
            {
                _logger.Warning("itemSummaries is null");
            }
        }


        private string GetImage(ItemSummary itemSummary)
        {

            return itemSummary.ThumbnailImages?[0]?.ImageUrl ?? itemSummary.Image?.ImageUrl ?? string.Empty;
        }

        private int GetCondition(ItemSummary itemSummary)
        {
            return itemSummary.Condition == "New" ? (int) Condition.New : (int)Condition.Used;
        }

        private async Task RefreshTokenAsync()
        {
            string clientId = Environment.GetEnvironmentVariable("ClientId") ?? string.Empty;
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret") ?? string.Empty;
            string refreshToken = Environment.GetEnvironmentVariable("RefreshToken") ?? string.Empty;
            string scope = _ebayUrlConfig.Scope;

            string tokenUrl = _baseUrl + _ebayUrlConfig.Paths.Token;
            string credentials = $"{clientId}:{clientSecret}";
            string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("scope", scope),
            });

            HttpResponseMessage response = await _httpClient.PostAsync(tokenUrl, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<EbayLoginResponse>();
                _memoryCache.Set("accessToken", data!.AccessToken);
                _logger.Information("Token Refreshed");
            }
            else
            {
                _logger.Error(await response.Content.ReadAsStringAsync());
            }
        }

        #endregion Private Methods
    }
}