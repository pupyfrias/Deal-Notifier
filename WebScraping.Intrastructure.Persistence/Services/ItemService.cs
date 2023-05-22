using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Concurrent;
using System.Data;
using System.Text;
using WebScraping.Core.Application.Constants;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Application.Dtos;
using WebScraping.Core.Application.Dtos.PhoneCarrier;
using WebScraping.Core.Application.DTOs;
using WebScraping.Core.Application.DTOs.Email;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;
using System.Linq.Dynamic.Core;
using WebScraping.Core.Application.Dtos.Unlockable;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using WebScraping.Core.Application.Dtos.Item;
using System;

namespace WebScraping.Infrastructure.Persistence.Services
{
    public class ItemService : IItemService
    {
        #region Fields

        private readonly IConfigurationProvider _configurationProvider;
        private readonly IEmailServiceAsync _emailService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public ConcurrentBag<Item> itemToNotifyList { get; set; } = new ConcurrentBag<Item>();

        #endregion Fields

        public ItemService(ILogger logger, IConfigurationProvider configurationProvider, IEmailServiceAsync emailService, IMapper mapper)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
            _emailService = emailService;
            _mapper = mapper;
        }

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
                    stringBuilder.AppendFormat(
                        @"<div style=""margin: 50px 20px;border-radius: 10px;box-shadow: 0px 0px 20px 0px rgba(0, 0, 0, 0.4);padding: 10px;""> 
                        <h2 style='margin:0;'>{0}</h2> 
                        <p style ='font-size:large; margin:0;'>US$ {1}</p> 
                        <a href='{2}'>
                            <img src='{3}' style=""width: 540px;height: 720px;object-fit: cover;""/>
                        </a> 
                    </div>", item.Name, item.Price, item.Link, item.Image);
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
        public void SaveOrUpdate(ref ConcurrentBag<ItemCreateDto> items)
        {
            var itemListToSave = new ConcurrentBag<ItemCreateDto>();
            var itemListToUpdate = new ConcurrentBag<Item>();

            Parallel.ForEach(items, item =>
            {
                using (var context = new ApplicationDbContext())
                {
                    var oldItem = context.Items.FirstOrDefault(i => i.Link == item.Link);
                    if (oldItem == null)
                    {
                        itemListToSave.Add(item);
                        ToNotify(_mapper.Map<Item>(item));
                    }
                    else
                    {
                        decimal oldPrice = oldItem.Price;
                        decimal saving = oldPrice - item.Price;

                        if (oldPrice != item.Price || true)
                        {
                            if (oldPrice > item.Price)
                            {
                                oldItem.Saving = saving;
                                oldItem.SavingsPercentage = (saving / oldPrice) * 100;
                                oldItem.OldPrice = oldPrice;

                            }
                            else if (oldPrice < item.Price)
                            {
                                oldItem.OldPrice = 0;
                                oldItem.Saving = 0;
                                oldItem.SavingsPercentage = 0;
                            }

                            oldItem.Price = item.Price;
                            _mapper.Map(item, oldItem);
                            itemListToUpdate.Add(oldItem);
                            ToNotify(oldItem);
                        }


                    }
                }
            
            });

            try
            {
                var savingTask = Task.Run(() =>
                {
                    using (var context = new ApplicationDbContext())
                    {
                        context.Items.AddRange(_mapper.Map<List<Item>>(itemListToSave.ToList()));
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

                Task.WhenAll(updatingTask, savingTask).Wait();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }

        public bool TrySetModelNumberModelNameAndBrand(ref ItemCreateDto item)
        {
            using (var context = new ApplicationDbContext())
            {

                var possibleModelNumber = Regex.Match(
                    item.Name,
                    "((?:sm-)?[a-z]\\d{3,4}[a-z]{0,3}\\d?)|(xt\\d{4}(-(\\d{1,2}|[a-z]))?)|((?:lg|lm)-?[a-z]{1,2}\\d{3}([a-z]{1,2})?\\d?(?:\\([a-z]\\))?)",
                    RegexOptions.IgnoreCase
                    );

                if(possibleModelNumber.Value != string.Empty)
                {
                    _logger.Information(possibleModelNumber.Value);
                    var unlockable = context.Unlockables.Where(x => x.ModelNumber.Contains(possibleModelNumber.Value))
                        .ProjectTo<UnlockableReadDto>(_configurationProvider)
                        .FirstOrDefault();


                    if (unlockable != null)
                    {
                        item.BrandId = unlockable.BrandId;
                        item.ModelName = unlockable.ModelName;
                        item.ModelNumber = unlockable.ModelNumber;
                        return true;
                    }
                }

                return false;


            }
        }

        public void UpdateStatus(ref ConcurrentBag<string> checkedList)
        {
            using (var context = new ApplicationDbContext())
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
            checkedList.Clear();
        }
        private void ToNotify(Item item)
        {
            foreach (ConditionsToNotifyDto conditionsToNotify in Helper.ConditionsToNotifyList)
            {
                if (conditionsToNotify.ConditionId == item.ConditionId &&
                    conditionsToNotify.MaxPrice >= item.Price &&
                    CheckKeywords(conditionsToNotify.Keywords, item.Name) &&
                    item.Notify
                    )
                {
                    itemToNotifyList.Add(item);
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
            using (var context = new ApplicationDbContext())
            {
                var banedList = await context.Banned
                    .ProjectTo<BannedDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BannedKeywordList = banedList.ToHashSet<BannedDto>();
            }
        }

        private async Task LoadBlackList()
        {
            using (var context = new ApplicationDbContext())
            {
                var backList = await context.BlackLists
                    .ProjectTo<BlackListDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BlacklistedLinks = backList.ToHashSet<BlackListDto>();
            }
        }
        private async Task LoadBrands()
        {
            using (var context = new ApplicationDbContext())
            {
                var brandList = await context.Brands
                    .ProjectTo<BrandReadDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BrandList = brandList.ToHashSet<BrandReadDto>();
            }
        }

        private async Task LoadConditionsToNotify()
        {
            using (var context = new ApplicationDbContext())
            {
                var conditionsToNotifyList = await context.ConditionsToNotify
                    .ProjectTo<ConditionsToNotifyDto>(_configurationProvider)
                    .ToListAsync();

                Helper.ConditionsToNotifyList = conditionsToNotifyList.ToHashSet<ConditionsToNotifyDto>();
            }
        }
        private async Task LoadPhoneCarriers()
        {
            using (var context = new ApplicationDbContext())
            {
                var phoneCarrirerList = await context.PhoneCarriers
                    .ProjectTo<PhoneCarrierReadDto>(_configurationProvider)
                    .ToListAsync();

                Helper.PhoneCarrierList = phoneCarrirerList.ToHashSet<PhoneCarrierReadDto>();
            }
        }
        #endregion Private Methods

        #endregion Methods
    }
}