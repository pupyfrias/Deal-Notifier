using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Concurrent;
using System.Data;
using System.Linq;
using WebScraping.Core.Application.Dtos;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Application.Interfaces.Services;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;

namespace WebScraping.Infrastructure.Persistence.Services
{
    public class ItemService : IItemService
    {
        #region Fields
        private readonly ILogger _logger;
        private readonly IConfigurationProvider _configurationProvider;
        #endregion Fields

        public ItemService(ILogger logger, IConfigurationProvider configurationProvider)
        {
            _logger = logger;
            _configurationProvider = configurationProvider;
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

                if (ToNotify(item))
                {
                    _logger.Information(item.Name);
                    _logger.Information(item.Link);
                }


                using (var context = new ApplicationDbContext())
                {
                    var oldItem = context.Items.FirstOrDefault(i => i.Link == item.Link);
                    if (oldItem == null)
                    {
                        itemListToSave.Add(item);
                    }
                    else
                    {
                        decimal oldPrice = oldItem.Price;
                        decimal saving = oldPrice - item.Price;
                        bool validate = false;

                        if (oldPrice > item.Price)
                        {
                            oldItem.Saving = saving;
                            oldItem.SavingsPercentage = (saving / oldPrice) * 100;
                            oldItem.Price = item.Price;
                            oldItem.OldPrice = oldPrice;
                            validate = true;
                        }
                        else if (oldPrice < item.Price)
                        {
                            oldItem.Price = item.Price;
                            oldItem.OldPrice = 0;
                            oldItem.Saving = 0;
                            oldItem.SavingsPercentage = 0;
                            validate = true;
                        }

                        if (item.Name != oldItem.Name || item.Image != oldItem.Image)
                        {
                            oldItem.Name = item.Name;
                            oldItem.Image = item.Image;
                            validate = true;
                        }

                        if (validate)
                        {
                            itemListToUpdate.Add(oldItem);
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


        public  void UpdateStatus(ref ConcurrentBag<string> checkedList)
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

            await Task.WhenAll(blackList, bannedKeywordList, conditionToNotifyList);
            _logger.Information("All needed data Loaded");
        }


        private bool ToNotify(Item item)
        {
           foreach(ConditionsToNotifyDto conditionsToNotify  in Helper.ConditionsToNotifyList)
            {

                if(conditionsToNotify.ConditionId == item.ConditionId && 
                    conditionsToNotify.MaxPrice >= item.Price &&
                    CheckKeywords(conditionsToNotify.Keywords, item.Name)
                    ) 
                {
                    return true;
                }
            }

           return false;

        }

        private bool CheckKeywords(string keywords, string title )
        {
            var keywordList = keywords.Split(',');
            return keywordList.Any( keyword => title.IndexOf(keyword) > -1);
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

        private  async Task LoadBannedKeyword()
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

        #endregion Methods
    }
}