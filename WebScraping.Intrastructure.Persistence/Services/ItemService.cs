using Microsoft.Extensions.Logging;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Application.Utils;
using WebScraping.Core.Domain.Entities;
using WebScraping.Intrastructure.Persistence.DbContexts;
using Condition = WebScraping.Core.Application.Emuns.Condition;


namespace WebScraping.Intrastructure.Persistence.Services
{
    public class ItemService
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
        public static async Task<bool> CanBeSave(Item item)
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
        public static void SetCondition(ref Item item)
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
        public static void Save(ref HashSet<Item> items)
        {
            Parallel.ForEach(items, newItem =>
            {
                using (var context = new ApplicationDbContext())
                {
                    try
                    {
                        var oldItem = context.Items.FirstOrDefault(i => i.Link == newItem.Link);
                        Helper.checkedItemList.Add(newItem.Link);

                        if (oldItem == null)
                        {
                            context.Add(newItem);
                        }
                        else
                        {
                            decimal oldPrice = oldItem.Price;
                            decimal saving = 0;
                            bool validate = false;


                            if (oldPrice > newItem.Price)
                            {
                                oldItem.Saving = oldPrice - newItem.Price;
                                oldItem.SavingsPercentage = saving / oldPrice * 100;
                                validate = true;
                            }
                            else if (oldPrice < newItem.Price)
                            {
                                oldItem.OldPrice = 0;
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
                                newItem.LastModified = DateTime.Now;
                            }
                        }

                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        LogConsole.CreateLogger<ItemService>().LogWarning(ex.Message);
                    }
                    
                }
            });


        }
        #endregion Methods
    }


}