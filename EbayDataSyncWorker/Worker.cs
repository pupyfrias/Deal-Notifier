using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.eBay;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using ILogger = Serilog.ILogger;

namespace EbayDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly string _baseUrl;
        private readonly EbayUrlConfig _ebayUrlConfig;
        private readonly HttpClient _httpClient;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IEmailService _emailService;
        private IItemSyncService _itemSyncService;
        private int _retryFetch = 0;

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
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        }

        public async Task InitAsync()
        {
            _logger.Information($"Timer elapsed. Running eBay Service Init. {DateTime.Now}");
            var timer = new Stopwatch();
            timer.Start();

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _itemSyncService = scope.ServiceProvider.GetRequiredService<IItemSyncService>();
                _emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                string searchUrl = _baseUrl + _ebayUrlConfig.Paths.Search.First();
                await _itemSyncService.LoadNecessaryDataAsync();
                await ProcessDataAsync(searchUrl);
            }

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;

            _logger.Information($"Time taken: {timeTaken.ToString(@"m\:ss\.fff")}");
            _logger.Information("eBay Service completed.");


           
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Starting ExecuteAsync.");
             await InitAsync();
             await StopAsync(stoppingToken);
            _logger.Information("ExecuteAsync finished.");
        }


        private async Task ProcessDataAsync(string? initialUrl) 
        {
            try
            {
                string? currentUrl = initialUrl;
                do
                {
                    var ebayResponse = await FetchDataAsync(currentUrl);
                    if (ebayResponse == null) break;
                    var itemCreateList = await MapItemSummariesToItemCreatesAsync(ebayResponse.ItemSummaries);
                    await _itemSyncService.SaveOrUpdateAsync(itemCreateList);
                    currentUrl = ebayResponse.Next;
                }
                while (currentUrl != null);


                await _itemSyncService.UpdateStockStatusAsync(OnlineStore.eBay);
                await _emailService.NotifyUsersOfItemsByEmail();
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }  
        }



        private async Task<EBayResponse?> FetchDataAsync(string? url)
        {
            EBayResponse? ebayResponse = default;
            try
            {
                _memoryCache.TryGetValue("accessToken", out string? accessToken);
              
                if (string.IsNullOrEmpty(accessToken))
                {
                    await RefreshTokenAsync();
                    _memoryCache.TryGetValue("accessToken", out accessToken);
                }

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    ebayResponse = await response.Content.ReadFromJsonAsync<EBayResponse>();
                    _retryFetch = 0;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if(_retryFetch > 3)
                    {
                        _logger.Information("Maximum retry limit reached");
                        _retryFetch = 0;
                        return null;
                    }

                    _retryFetch++;
                    _logger.Information("Unauthorized");
                    await RefreshTokenAsync();
                    return await FetchDataAsync(url);

                }
                else
                {
                    _logger.Warning($"{(int)response.StatusCode} | {response.ReasonPhrase}");
                }
            }
            catch(Exception ex)
            {
                _logger?.Error(ex.Message);

            }
           
            return ebayResponse;

        }


        #region Private Methods

        private int GetConditionId(ItemSummary itemSummary)
        {
            return itemSummary.Condition == "New" ? (int)Condition.New : (int)Condition.Used;
        }

        private string GetImage(ItemSummary itemSummary)
        {

            return itemSummary.ThumbnailImages?[0]?.ImageUrl ?? itemSummary.Image?.ImageUrl ?? string.Empty;
        }

        private async Task<ConcurrentBag<ItemCreateRequest>> MapItemSummariesToItemCreatesAsync(List<ItemSummary>? itemSummaries)
        {
            var mappedItemList = new ConcurrentBag<ItemCreateRequest>();

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

                            if (await itemSyncService.CanBeSaved(itemCreate))
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
                                itemCreate.ConditionId = GetConditionId(element);
                                itemCreate.Image = GetImage(element);
                                itemCreate.IsAuction = isAuction;
                                itemCreate.ItemEndDate = element.ItemEndDate?.AddHours(-4);
                                itemCreate.ItemTypeId = (int)ItemType.Phone;
                                itemCreate.OnlineStoreId = (int)OnlineStore.eBay;
                                itemCreate.Price = price;
                                itemCreate.StockStatusId = (int)StockStatus.InStock;
                                await itemSyncService.TryAssignUnlockabledPhoneIdAsync(itemCreate);
                                await itemSyncService.SetUnlockProbabilityAsync(itemCreate);

                                mappedItemList.Add(itemCreate);
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

            return mappedItemList;
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
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception(error);
                
            }
        }

        #endregion Private Methods
    }
}