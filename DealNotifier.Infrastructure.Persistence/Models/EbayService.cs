﻿using Serilog;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Models.eBay;
using Enums = DealNotifier.Core.Application.Enums;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;

namespace DealNotifier.Infrastructure.Persistence.Models
{
    public class EbayService : IEbayService
    {
        private ConcurrentBag<ItemCreateDto> itemList = new ConcurrentBag<ItemCreateDto>();
        private ConcurrentBag<string> checkedList = new ConcurrentBag<string>();
        private DateTime expiresAt;
        private readonly IItemServiceAsync _itemService;
        private readonly ILogger _logger;
        private readonly string baseUrl = @"https://api.ebay.com/buy/browse/v1/item_summary/search?filter=price:[20..100],priceCurrency:USD,conditionIds:{1000|3000},itemLocationCountry:US&sort=price&limit=200&aspect_filter=categoryId:9355,Operating System:{Android},Storage Capacity:{512 GB|256 GB|64 GB|32 GB|128 GB}&q=(LG,Motorola,Samsung)&category_ids=9355";
        private string accessToken = string.Empty;
        private string? currentUrl = string.Empty;

        public EbayService(ILogger logger, IItemServiceAsync itemService, IEmailServiceAsync emailService)
        {
            _logger = logger;
            _itemService = itemService;
        }

        public async Task Init()
        {
            await _itemService.LoadData();
            await RunAsync(baseUrl);
        }

        private async Task RunAsync(string? url, int counter = 1)
        {
            currentUrl = url;
            HttpClient httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = httpClient.GetAsync(currentUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<eBayResponse>();
                Mapping(content?.ItemSummaries);
                _itemService.SaveOrUpdate(in itemList);

                _logger.Information($"Total: {content?.Total}");
                _logger.Information($"1\t| {counter}\t| {itemList.Count}");
                counter++;
                itemList.Clear();
                currentUrl = content?.Next;


                while (true)
                {
                    if (expiresAt <= DateTime.Now)
                    {
                        await RefreshTokenAsync();
                        await RunAsync(currentUrl, counter);
                        return;
                    }


                    using (HttpResponseMessage httpResponseMessage = httpClient.GetAsync(currentUrl).Result)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var data = await httpResponseMessage.Content.ReadFromJsonAsync<eBayResponse>();
                            if (data?.Next == null) break;

                            Mapping(data?.ItemSummaries);
                            currentUrl = data?.Next;
                            
                            _itemService.SaveOrUpdate(in itemList);
                            _logger.Information($"1\t| {counter}\t| {itemList.Count}");
                            counter++;
                            itemList.Clear();
                        }
                        else if (response.StatusCode == HttpStatusCode.Unauthorized)
                        {
                            _logger.Information("Unauthorized inside while");
                            await RefreshTokenAsync();
                            await RunAsync(currentUrl, counter);
                            return;

                        }
                        else
                        {
                            _logger.Warning($"{(int)response.StatusCode} | {response.ReasonPhrase}");
                            break;
                        }
                    }
                }

                _itemService.UpdateStatus(ref checkedList);
                await _itemService.NotifyByEmail();

            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized || accessToken.IsNullOrEmpty())
            {
                _logger.Information("Unauthorized");
                await RefreshTokenAsync();
                await RunAsync(currentUrl);
                
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
            if (itemSummaries != null)
            {
                Parallel.ForEach(itemSummaries, async (element, stateMain) =>
                {
                    try
                    {
                        ItemCreateDto item = new ItemCreateDto();
                        item.Name = element.Title;
                        item.Link = element.ItemWebUrl.Substring(0, element.ItemWebUrl.IndexOf("?"));
                        bool canBeSaved = await item.CanBeSaved();

                        if (canBeSaved)
                        {
                            decimal price = 0;
                            bool isAuction = decimal.TryParse(element.CurrentBidPrice?.Value, out decimal currentBidPrice);

                            if (isAuction)
                            {
                                price = currentBidPrice;
                            }
                            else
                            {
                                decimal.TryParse(element.Price?.Value, out price);
                            }

                            decimal.TryParse(element.ShippingOptions?[0]?.ShippingCost?.Value, out decimal shippingCost);
                            price += shippingCost;

                            item.Image = element?.ThumbnailImages?[0]?.ImageUrl ?? element?.Image?.ImageUrl ?? string.Empty;
                            item.Price = price;
                            item.ShopId = (int)Enums.Shop.eBay;
                            item.TypeId = (int)Enums.Type.Phone;
                            item.StatusId = (int)Enums.Status.InStock;
                            item.ConditionId = element?.Condition == "New" ? (int)Enums.Condition.New : (int)Enums.Condition.Used;
                            item.ItemEndDate = element?.ItemEndDate?.AddHours(-4);
                            item.SetPhoneCarrier();
                            _itemService.TrySetModelNumberModelNameAndBrand(ref item);
                            item.BidCount = element?.BidCount ?? 0;
                            item.IsAuction = isAuction;

                            _itemService.SetUnlockProbability(ref item);
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
                accessToken = jsonResponse.RootElement.GetProperty("access_token").GetString() ?? string.Empty;
                var expiresIn = jsonResponse.RootElement.GetProperty("expires_in").GetInt32();
                expiresAt = DateTime.Now.AddSeconds(expiresIn - 60);
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