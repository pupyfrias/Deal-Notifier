using System.Collections.Concurrent;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Application.Utils;
using WebScraping.Core.Domain.Entities;
using WebScraping.Infrastructure.Persistence.DbContexts;
using Condition = WebScraping.Core.Application.Emuns.Condition;

namespace WebScraping.Infrastructure.Persistence.Services
{
    public static class ItemService
    {
        #region Fields

        private static readonly HashSet <string> conditionList = new HashSet<string> { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored", "used" };
        #endregion Fields

        #region Methods

        /// <summary>
        /// Validate if the Item can be save.
        /// </summary>
        /// <returns>true if the Item is not in BlackList; otherwise, false.</returns>
        public static async Task<bool> CanBeSaved(this Item item)
        {
            bool isBanned = false;
            bool isInBlackList = false;

            Task bannedTask = Task.Run(() =>
            {
                isBanned = Helper.BannedKeywordList.Any(element => item.Name.IndexOf(element.Keyword, StringComparison.OrdinalIgnoreCase) >= 0);

            });

            Task blackListTask = Task.Run(() =>
            {
                isInBlackList = Helper.BlacklistedLinks.Any(x => x.Link == item.Link) ;
            });

            await Task.WhenAll(bannedTask, blackListTask);

            return !(isBanned || isInBlackList);
        }

        /// <summary>
        /// Set Item Condition
        /// </summary>
        public static void SetCondition(this Item item)
        {
            bool isNotNew =  conditionList.Any(condition =>  item.Name.IndexOf(condition, StringComparison.OrdinalIgnoreCase) >= 0);
            item.ConditionId = isNotNew ? (int)Condition.Used : (int)Condition.New;

        }

        /// <summary>
        /// Save item's data
        /// </summary>
        /// <param name="items">Items ConcurrentBag</param>
        public static void Save(this ConcurrentBag<Item> items)
        {
            var itemListToSave = new ConcurrentBag<Item>();
            var itemListToUpdate = new ConcurrentBag<Item>();

            Parallel.ForEach(items, item =>
            {
                Helper.CheckedList.Add(item.Link);
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
                Logger.CreateLogger().ForContext<ApplicationDbContext>().Error(ex, ex.Message);
            }
        }

        #endregion Methods
    }
}