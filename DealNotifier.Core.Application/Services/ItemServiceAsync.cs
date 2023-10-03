using AutoMapper;
using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.DTOs.Email;
using DealNotifier.Core.Application.DTOs;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Application.DTOs.PhoneCarrier;
using DealNotifier.Core.Application.DTOs.Unlockable;
using DealNotifier.Core.Application.Heplers;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Data;
using System.Text.RegularExpressions;
using System.Text;
using Serilog;
using Microsoft.Extensions.Configuration;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.Models;
using DealNotifier.Core.Domain.Contracts;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;

namespace DealNotifier.Core.Application.Services
{
    public class ItemServiceAsync : GenericServiceAsync<Item>, IItemServiceAsync
    {
        #region Fields

        private readonly IConfigurationProvider _configurationProvider;
        private readonly IEmailServiceAsync _emailService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public ConcurrentBag<Item> itemToNotifyList { get; set; } = new ConcurrentBag<Item>();

        #endregion Fields

        #region Constructor

        public ItemServiceAsync(IItemRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache, IItemRepositoryAsync itemRepository, IConfigurationProvider configurationProvider, IEmailServiceAsync emailService, ILogger logger) : base(repository, mapper, httpContext, cache)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
            _emailService = emailService;
            _mapper = mapper;
        }
        #endregion Constructor


        #region Methods

        public async Task LoadData()
        {
            Task blackList = Task.Run(async () =>
            {
                await LoadBlackList();
            });

            Task bannedKeywordList = Task.Run(async () =>
            {
                await LoadBannedKeyword();
            });

            Task conditionToNotifyList = Task.Run(async () =>
            {
                await LoadConditionsToNotify();
            });

            Task brandList = Task.Run(async () =>
            {
                await LoadBrands();
            });

            Task phoneCarrierList = Task.Run(async () =>
            {
                await LoadPhoneCarriers();
            });

            await Task.WhenAll(blackList, bannedKeywordList, conditionToNotifyList, brandList, phoneCarrierList);
            _logger.Information("All needed data Loaded");
        }

        public async Task NotifyByEmail()
        {
            if (itemToNotifyList.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();

                var orderItemToNotifyList = itemToNotifyList.OrderBy(item => item.Price);
                _logger.Information($"orderItemToNotifyList {orderItemToNotifyList.Count()}");

                foreach (var item in orderItemToNotifyList)
                {
                    var probability = item.UnlockProbabilityId == 3 ? Enums.UnlockProbability.High.ToString() : Enums.UnlockProbability.Middle.ToString();

                    stringBuilder.AppendFormat(
                        @"<div style=""margin: 50px 20px;border-radius: 10px;box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.4);padding: 10px;"">
                        <h2 style='margin:0;'>{0}</h2>
                        <p style ='font-size:large; margin:0;'>US$ {1}</p>
                        <p style ='font-size:large; margin:0;'>Unlock Probability: {5}</p>
                        <a href='https://10.0.0.3:8081/api/Items/{4}/cancel-notification' style= 'display:block'> Cancel Notification</a>
                        <a href='{2}'>
                            <img src='{3}' style=""width: 540px;height: 720px;object-fit: cover;""/>
                        </a>
                    </div>", item.Name, item.Price, item.Link, item.Image, item.Id, probability);
                }
                string body = stringBuilder.ToString();

                var email = new EmailDto
                {
                    To = "pupyfrias@gmail.com",
                    Subject = "Phones Offer",
                    Body = body
                };

                await _emailService.SendAsync(email);
                itemToNotifyList.Clear();
            }
        }

        /// <summary>
        /// Save item's data
        /// </summary>
        /// <param name="items">Items ConcurrentBag</param>
        public void SaveOrUpdate(in ConcurrentBag<ItemCreateDto> items)
        {
            /*var itemListToSave = new ConcurrentBag<Item>();
            var itemListToUpdate = new ConcurrentBag<Item>();

            Parallel.ForEach(items, item =>
            {
                using (var context = new ApplicationDbContext())
                {

                    var oldItem = context.Items.FirstOrDefault(i => i.Link == item.Link);
                    if (oldItem == null)
                    {
                        var mappedItem = _mapper.Map<Item>(item);

                        ToNotify(mappedItem);
                        itemListToSave.Add(mappedItem);

                    }
                    else
                    {
                        decimal oldPrice = oldItem.Price;
                        decimal saving = oldPrice - item.Price;

                        if (oldPrice != item.Price ||
                            oldItem.UnlockProbabilityId != item.UnlockProbabilityId ||
                            oldItem.Image != item.Image ||
                            item.IsAuction
                            )
                        {
                            if (oldPrice > item.Price)
                            {
                                oldItem.Saving = saving;
                                oldItem.SavingsPercentage = saving / oldPrice * 100;
                                oldItem.OldPrice = oldPrice;
                            }
                            else if (oldPrice < item.Price)
                            {
                                oldItem.OldPrice = 0;
                                oldItem.Saving = 0;
                                oldItem.SavingsPercentage = 0;
                            }

                            oldItem.Notified = null;
                            _mapper.Map(item, oldItem);
                            itemListToUpdate.Add(oldItem);
                            if ((oldPrice - item.Price) >= 5 || item.IsAuction)
                            {
                                ToNotify(oldItem);
                            }


                        }
                    }
                }
            });*/

            try
            {
               /* var savingTask = Task.Run(() =>
                {
                    using (var context = new ApplicationDbContext())
                    {
                        context.Items.AddRange(itemListToSave);
                        context.SaveChanges();
                    }
                });

                var updatingTask = Task.Run(() =>
                {
                    using (var context = new ApplicationDbContext())
                    {
                        context.Items.UpdateRange(itemListToUpdate);
                        context.SaveChanges();
                    }
                });

                Task.WhenAll(updatingTask, savingTask).Wait();*/
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }

        public void SetUnlockProbability(ref ItemCreateDto item)
        {
            /*var modelNumber = item.ModelNumber;
            var modelName = item.ModelName;
            var carrierId = item.PhoneCarrierId;

            if (item.Name.Contains("unlocked", StringComparison.OrdinalIgnoreCase)
                || item.Name.Contains("ulk", StringComparison.OrdinalIgnoreCase))
            {
                item.UnlockProbabilityId = (int)Enums.UnlockProbability.High;
            }
            else
            {
                using (var context = new ApplicationDbContext())
                {
                    string query = "EXEC CAN_BE_UNLOCKED_MODELNUMBER @modelNumber, @carrierId, @OutputResult OUTPUT";
                    var modelNameParameter = new SqlParameter("@modelName", modelName);
                    var modelNumberParameter = new SqlParameter("@modelNumber", modelNumber);
                    var carrierIdParameter = new SqlParameter("@carrierId", carrierId);
                    var outputResult = new SqlParameter("@OutputResult", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                    if (modelNumber != null)
                    {
                        context.Database.ExecuteSqlRaw(query, modelNumberParameter, carrierIdParameter, outputResult);
                        bool result = (bool)outputResult.Value;

                        if (result)
                        {
                            item.UnlockProbabilityId = (int)Enums.UnlockProbability.High;
                            return;
                        }

                    }
                    else if (modelName != null)
                    {
                        query = "EXEC CAN_BE_UNLOCKED_MODELNAME @modelName, @carrierId, @OutputResult OUTPUT";
                        context.Database.ExecuteSqlRaw(query, modelNameParameter, carrierIdParameter, outputResult);
                        bool result = (bool)outputResult.Value;

                        if (result)
                        {
                            item.UnlockProbabilityId = (int)Enums.UnlockProbability.Middle;
                            return;
                        }
                    }

                    item.UnlockProbabilityId = (int)Enums.UnlockProbability.Low;
                }
            }*/
        }

        public void TrySetModelNumberModelNameAndBrand(ref ItemCreateDto item)
        {
            /*using (var context = new ApplicationDbContext())
            {
                var possibleModelNumber = Regex.Match(
                    item.Name,
                    "((?:sm-)?[a-z]\\d{3,4}[a-z]{0,3}\\d?)|(xt\\d{4}(-(\\d{1,2}|[a-z]))?)|((?:lg|lm)-?[a-z]{1,2}\\d{3}([a-z]{1,2})?\\d?(?:\\([a-z]\\))?)",
                    RegexOptions.IgnoreCase
                    );

                if (possibleModelNumber.Value != string.Empty)
                {
                    var unlockable = context.UnlockablePhones.Where(x => x.ModelNumber.Contains(possibleModelNumber.Value))
                        .ProjectTo<UnlockableReadDto>(_configurationProvider)
                        .FirstOrDefault();

                    if (unlockable != null)
                    {
                        item.BrandId = unlockable.BrandId;
                        item.ModelName = unlockable.ModelName;
                        item.ModelNumber = unlockable.ModelNumber;
                    }

                }

                if (item.BrandId == null && item.ModelName == null && item.ModelNumber == null)
                {
                    #region set brand
                    foreach (var brand in Helper.BrandList)
                    {
                        bool isMatched = item.Name.IndexOf(brand.Name, StringComparison.OrdinalIgnoreCase) > -1;
                        if (isMatched)
                        {
                            item.BrandId = brand.Id;
                            break;
                        }

                    }
                    #endregion set brand

                    #region set modelName & phone Carrier
                    UnlockableReadDto? unlockable;
                    var possibleModelName = Regex.Match(
                    item.Name,
                    "((?<=samsung\\s+)(galaxy\\s+)?\\w+[+]?)|((?<=motorola\\s+)moto\\s+\\w+)|(lg\\s+\\w+( \\d)?)",
                    RegexOptions.IgnoreCase
                    );

                    if (possibleModelName.Value != string.Empty)
                    {

                        int phoneCarrierId = item.PhoneCarrierId ?? (int)Enums.PhoneCarrier.UNK;
                        unlockable = context.UnlockablePhones
                            .Include(x => x.UnlockablePhoneCarriers)
                            .Where(x => x.ModelName.Contains(possibleModelName.Value) &&
                            x.UnlockablePhoneCarriers.Any(u => u.PhoneCarrierId == phoneCarrierId))
                           .ProjectTo<UnlockableReadDto>(_configurationProvider)
                           .FirstOrDefault();

                        if (unlockable != null)
                        {
                            item.ModelName = unlockable.ModelName;
                            item.ModelNumber = unlockable.ModelNumber;
                        }
                        else
                        {
                            unlockable = context.UnlockablePhones
                            .Where(x => x.ModelName.Contains(possibleModelName.Value))
                           .ProjectTo<UnlockableReadDto>(_configurationProvider)
                           .FirstOrDefault();

                            if (unlockable != null)
                            {
                                item.ModelName = unlockable.ModelName;
                            }

                        }
                        #endregion set modelName & phone Carrier
                    }
                }
            }*/
        }

        public void UpdateStatus(ref ConcurrentBag<string> checkedList)
        {
            /*using (var context = new ApplicationDbContext())
            {
                var listId = checkedList.Select(x => x.Replace("https://www.ebay.com/itm/", ""));
                string query = "EXEC UPDATE_STATUS_EBAY @ListId, @OutputResult OUTPUT";
                var listIdParameter = new SqlParameter("@ListId", string.Join(',', listId));
                var outputResult = new SqlParameter("@OutputResult", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                context.Database.ExecuteSqlRaw(query, listIdParameter, outputResult);
                bool result = (bool)outputResult.Value;
                _logger.Information($"El resultado es: {result}");
            }

            _logger.Information($"{checkedList.Count} Items In Stock");
            checkedList.Clear();*/
        }

        private void ToNotify(Item item)
        {
            DateTime notified = item.Notified.HasValue ? (DateTime)item.Notified : DateTime.MinValue;
            DateTime itemEndDate = item.ItemEndDate.HasValue ? (DateTime)item.ItemEndDate : DateTime.MinValue;

            foreach (ConditionsToNotifyDto conditionsToNotify in Helper.ConditionsToNotifyList)
            {

                if (conditionsToNotify.ConditionId == item.ConditionId
                    && conditionsToNotify.MaxPrice >= item.Price
                    && CheckKeywords(conditionsToNotify.Keywords, item.Name)
                    && item.Notify
                    && notified.Date != DateTime.Now.Date

                    )
                {

                    if (item.TypeId == (int)Enums.Type.Phone && item.UnlockProbabilityId == (int)Enums.UnlockProbability.Low)
                    {
                        break;
                    }

                    if (item.IsAuction)
                    {
                        var diff = (itemEndDate - DateTime.Now).TotalHours;
                        if (diff > 2 || diff < 0 || item.BidCount > 3)
                        {
                            break;
                        }

                    }


                    itemToNotifyList.Add(item);
                    item.Notified = DateTime.Now;
                    break;
                }
            }
        }


        #region Private Methods

        private bool CheckKeywords(string keywords, string title)
        {
            var keywordList = keywords.Split(',');
            return keywordList.Any(keyword => title.IndexOf(keyword) > -1);
        }

        private async Task LoadBannedKeyword()
        {
            /*using (var context = new ApplicationDbContext())
            {
                var banedList = await context.Banned
                    .ProjectTo<BannedDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BannedKeywordList = banedList.ToHashSet<BannedDto>();
            }*/
        }

        private async Task LoadBlackList()
        {
           /* using (var context = new ApplicationDbContext())
            {
                var backList = await context.BlackLists
                    .ProjectTo<BlackListDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BlacklistedLinks = backList.ToHashSet<BlackListDto>();
            }*/
        }

        private async Task LoadBrands()
        {
            /*using (var context = new ApplicationDbContext())
            {
                var brandList = await context.Brands
                    .ProjectTo<BrandReadDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BrandList = brandList.ToHashSet<BrandReadDto>();
            }*/
        }

        private async Task LoadConditionsToNotify()
        {
           /* using (var context = new ApplicationDbContext())
            {
                var conditionsToNotifyList = await context.ConditionsToNotify
                    .ProjectTo<ConditionsToNotifyDto>(_configurationProvider)
                    .ToListAsync();

                Helper.ConditionsToNotifyList = conditionsToNotifyList.ToHashSet<ConditionsToNotifyDto>();
            }*/
        }

        private async Task LoadPhoneCarriers()
        {
           /* using (var context = new ApplicationDbContext())
            {
                var phoneCarrirerList = await context.PhoneCarriers
                    .ProjectTo<PhoneCarrierReadDto>(_configurationProvider)
                    .ToListAsync();

                Helper.PhoneCarrierList = phoneCarrirerList.ToHashSet<PhoneCarrierReadDto>();
            }*/
        }

        #endregion Private Methods

        #endregion Methods
    }


}