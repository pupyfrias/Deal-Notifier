using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
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
        private static readonly string[] conditionList = { "renovado", "renewed", "reacondicionado", "refurbished", "restaurado", "restored" };
        private static readonly string[] bannedWordList = { "tracfone", "total wireless", "net10", "simple mobile", "straight talk", "family mobile" };
        private static readonly string[] includeList = { };//{ "huawei", "lg ", "moto", "xiaomi", "iphone", "samsung" };

        #endregion Fields
        #region Methods

        /// <summary>
        /// Validate if the Item can be save.
        /// </summary>
        /// <returns>true if the Iten is not in BlackList; otherwise, false.</returns>
        public static async Task<bool> CanBeSave(this Item item)
        {
            bool isBanned = false;
            bool isInBlackList = false;

            Task bannedTask = Task.Run(() =>
            {
                foreach (string bannedWord in bannedWordList)
                {
                    if (item.Name.ToLower().Contains(bannedWord))
                    {
                        isBanned = true;
                        break;
                    }
                }

            });

            Task blackListTask = Task.Run(() =>
            {
                isInBlackList = Helper.linkBlackList.FirstOrDefault(x => x.Link == item.Link) == null ? false : true;
            });

            await Task.WhenAll(bannedTask, blackListTask);


            if (isBanned || isInBlackList)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// Set Item Condition
        /// </summary>
        public static void SetCondition(this Item item)
        {
            foreach (string condition in conditionList)
            {
                if (item.Name.ToLower().Contains(condition))
                {
                    item.ConditionId = (int)Condition.Used;
                    break;
                }
            }

            item.ConditionId = (int)Condition.New;
        }



        /// <summary>
        /// Save item's data
        /// </summary>
        /// <param name="items">Items List</param>
        public static async Task Save(this HashSet<Item> items)
        {
            foreach (Item newItem in items)
            //Parallel.ForEach(items, newItem =>
            {
                using (var context = new ApplicationDbContext())
                {
                    var oldItem = default(Item);
                    try
                    {
                        oldItem = context.Items.FirstOrDefault(i => i.Link == newItem.Link);
                        Helper.checkedItemList.Add(newItem.Link);

                        if (oldItem == null)
                        {
                            context.Add(newItem);
                        }
                        else
                        {
                            decimal oldPrice = oldItem.Price;
                            decimal saving = oldPrice - newItem.Price;
                            bool validate = false;


                            if (oldPrice > newItem.Price)
                            {
                                oldItem.Saving = saving;
                                oldItem.SavingsPercentage = (saving / oldPrice) * 100;
                                oldItem.Price = newItem.Price;
                                oldItem.OldPrice = oldPrice;
                                validate = true;
                            }
                            else if (oldPrice < newItem.Price)
                            {
                                oldItem.Price = newItem.Price;
                                oldItem.OldPrice = 0;
                                oldItem.Saving = 0;
                                oldItem.SavingsPercentage = 0;
                                validate = true;
                            }

                            if (newItem.Name != oldItem.Name || newItem.Image != oldItem.Image)
                            {
                                oldItem.Name = oldItem.Name;
                                oldItem.Image = oldItem.Image;
                                validate = true;
                            }


                            if (validate)
                            {
                                context.Update(oldItem);
                            }
                        }

                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Logger.CreateLogger().ForContext<ApplicationDbContext>().Warning($"URL --> {oldItem?.Link ?? newItem.Link}\n{ex.Message} \n");
                    }

                }
            }


        }
        #endregion Methods
    }


}