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
using WebScraping.Core.Application.DTOs;
using WebScraping.Core.Application.DTOs.Email;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Services
{
    public class ItemService : IItemService
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IEmailServiceAsync _emailService;
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

        /// <summary>
        /// Save item's data
        /// </summary>
        /// <param name="items">Items ConcurrentBag</param>
        public void SaveOrUpdate(ref ConcurrentBag<Item> items)
        {
            var itemListToSave = new ConcurrentBag<Item>();
            var itemListToUpdate = new ConcurrentBag<Item>();

            Parallel.ForEach(items, item =>
            {
                using (var context = new ApplicationDbContext())
                {
                    var oldItem = context.Items.FirstOrDefault(i => i.Link == item.Link);
                    if (oldItem == null)
                    {
                        itemListToSave.Add(item);
                        ToNotify(item);
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

                Task.WhenAll(updatingTask, savingTask).Wait();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
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

            await Task.WhenAll(blackList, bannedKeywordList, conditionToNotifyList, brandList);
            _logger.Information("All needed data Loaded");
        }



        public async Task NotifyByEmail()
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

        private bool CheckKeywords(string keywords, string title)
        {
            var keywordList = keywords.Split(',');
            return keywordList.Any(keyword => title.IndexOf(keyword) > -1);
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

        private async Task LoadBrands()
        {
            using (var context = new ApplicationDbContext())
            {
                var brandList = await context.Brands
                    .ProjectTo<BrandDto>(_configurationProvider)
                    .ToListAsync();

                Helper.BrandList = brandList.ToHashSet<BrandDto>();
            }
        }

        #endregion Methods
    }
}