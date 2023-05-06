using Serilog;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WebScraping.Core.Application.Extensions;
using WebScraping.Core.Application.Interfaces.Services;
using WebScraping.Core.Application.Models;
using WebScraping.Core.Domain.Entities;
using Condition = WebScraping.Core.Application.Emuns.Condition;
using Shop = WebScraping.Core.Application.Emuns.Shop;
using Status = WebScraping.Core.Application.Emuns.Status;
using Type = WebScraping.Core.Application.Emuns.Type;

namespace WebScraping.Infrastructure.Persistence.Models
{
    public class EbayService: IEbayService
    {
        private  ILogger _logger;
        private  IItemService _itemService;
        private  ConcurrentBag<Item> itemList = new ConcurrentBag<Item>();
        private ConcurrentBag<string> checkedList = new ConcurrentBag<string>();
        private  bool tokenRefreshed = false;
        private readonly string baseUrl = @"https://api.ebay.com/buy/browse/v1/item_summary/search?filter=price:[20..100],priceCurrency:USD,conditionIds:{1000|3000},itemLocationCountry:US&sort=price&limit=200&aspect_filter=categoryId:9355,Operating System:{Android},Storage Capacity:{512 GB|256 GB|64 GB|32 GB|128 GB},Brand :{LG|Motorola|Samsung}&q=(LG,Motorola,Samsung) (Metro,Virgin,Boost,Sprint,T-Mobile,Unlocked)&category_ids=9355\";
        private string? currentUrl = string.Empty;


        public EbayService(ILogger logger, IItemService itemService) 
        {
            _logger = logger;
            _itemService = itemService;
        }


        public async Task Init()
        {
            await _itemService.LoadData();
            await RunAsync(baseUrl);
        }

        private async Task RunAsync(string url)
        {
            int counter = 1;
            currentUrl = url;

            string accessToken = Environment.GetEnvironmentVariable("AccessToken") ?? string.Empty;
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = httpClient.GetAsync(currentUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<eBayResponse>();
                Mapping(content?.ItemSummaries);
                _itemService.SaveOrUpdate(ref itemList);

                _logger.Information($"1\t| {counter}\t| {itemList.Count}");
                counter++;
                itemList.Clear();
                currentUrl = content?.Next;

                while (currentUrl != null)
                {
                    using (HttpResponseMessage httpResponseMessage = httpClient.GetAsync(currentUrl).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var data = await httpResponseMessage.Content.ReadFromJsonAsync<eBayResponse>();
                            Mapping(data?.ItemSummaries);
                            currentUrl = data?.Next;
                            _itemService.SaveOrUpdate(ref itemList);
                            _logger.Information($"1\t| {counter}\t| {itemList.Count}");
                            counter++;
                            itemList.Clear();
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            await RefreshTokenAsync();
                            await RunAsync(currentUrl);
                            tokenRefreshed = true;
                        }
                        else
                        {
                            _logger.Warning($"{(int)response.StatusCode} | {response.ReasonPhrase}");
                        }
                    }

                }

                _itemService.UpdateStatus(ref checkedList);


            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshTokenAsync();
                await RunAsync(currentUrl);
                tokenRefreshed = true;
            }
            else
            {
                _logger.Warning($"{(int)response.StatusCode} | {response.ReasonPhrase}");
            }

           
            httpClient.Dispose();
        }


        #region Private Methods
        private void Mapping(List<ItemSummary>? itemSummaries)
        {
            if( itemSummaries != null)
            {
                Parallel.ForEach(itemSummaries, async (element, stateMain) =>
                {
                    try
                    {
                        var shippingCost = element.ShippingOptions?[0]?.ShippingCost?.Value;
                        decimal price = decimal.Parse(element.Price.Value);

                        if (shippingCost != null) { price += decimal.Parse(shippingCost); }

                        Item item = new Item();
                        item.Name = element.Title;
                        item.Link = element.ItemWebUrl.Substring(0, element.ItemWebUrl.IndexOf("?"));
                        item.Image = element?.ThumbnailImages?[0]?.ImageUrl ?? element?.Image?.ImageUrl ?? string.Empty;
                        item.Price = price;
                        item.ShopId = (int)Shop.eBay;
                        item.TypeId = (int)Type.Phone;
                        item.StatusId = (int)Status.InStock;
                        item.ConditionId = element?.Condition == "New" ?  (int)Condition.New: (int)Condition.Used;
                        bool canBeSaved = await item.CanBeSaved();

                        if (canBeSaved)
                        {
                            itemList.Add(item);
                            checkedList.Add(item.Link);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.Warning(e.Message);
                    }
                });
            }
            else
            {
                _logger.Warning("itemSummaries is null");
            }

            
        }

        private async Task RefreshTokenAsync()
        {
            string clientId = Environment.GetEnvironmentVariable("ClientId") ?? string.Empty;
            string clientSecret = Environment.GetEnvironmentVariable("ClientSecret") ?? string.Empty;
            string refreshToken = Environment.GetEnvironmentVariable("RefreshToken") ?? string.Empty;
            string scope = "https://api.ebay.com/oauth/api_scope " +
                            "https://api.ebay.com/oauth/api_scope/sell.marketing.readonly " +
                            "https://api.ebay.com/oauth/api_scope/sell.marketing " +
                            "https://api.ebay.com/oauth/api_scope/sell.inventory.readonly " +
                            "https://api.ebay.com/oauth/api_scope/sell.inventory " +
                            "https://api.ebay.com/oauth/api_scope/sell.account.readonly " +
                            "https://api.ebay.com/oauth/api_scope/sell.account " +
                            "https://api.ebay.com/oauth/api_scope/sell.fulfillment.readonly " +
                            "https://api.ebay.com/oauth/api_scope/sell.fulfillment " +
                            "https://api.ebay.com/oauth/api_scope/sell.analytics.readonly " +
                            "https://api.ebay.com/oauth/api_scope/sell.finances " +
                            "https://api.ebay.com/oauth/api_scope/sell.payment.dispute " +
                            "https://api.ebay.com/oauth/api_scope/commerce.identity.readonly " +
                            "https://api.ebay.com/oauth/api_scope/commerce.notification.subscription " +
                            "https://api.ebay.com/oauth/api_scope/commerce.notification.subscription.readonly";



            using HttpClient httpClient = new HttpClient();
            string tokenUrl = "https://api.ebay.com/identity/v1/oauth2/token";
            string credentials = $"{clientId}:{clientSecret}";
            string base64Credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));


            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64Credentials);

            var requestBody = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("scope", scope),
            });

            HttpResponseMessage response = await httpClient.PostAsync(tokenUrl, requestBody);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonDocument.Parse(responseBody);
                var newAccessToken = jsonResponse.RootElement.GetProperty("access_token").GetString() ?? string.Empty;
                Environment.SetEnvironmentVariable("AccessToken", newAccessToken);

            }
            else
            {
                _logger.Error(await response.Content.ReadAsStringAsync());
            }

        }

        #endregion Private Methods


    }
}