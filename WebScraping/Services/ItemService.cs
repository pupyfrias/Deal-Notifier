using DataBase;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebScraping.Heplers;
using Item = WebScraping.Entities.Item;

namespace WebScraping.Services
{
    public class ItemService
    {
        /// <summary>
        /// Save item's data
        /// </summary>
        /// <param name="items">Items List</param>
        public static void Save(ref HashSet<Item> items)
        {
            Parallel.ForEach(items, item =>
            {
                using (WebScrapingEntities context = new WebScrapingEntities())
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                    var data = context.Items.SingleOrDefault(i => i.LINK == item.Link);
                    Helper.checkList.Add(item.Link);

                    if (data != null)
                    {
                        decimal oldPrice = (decimal)data.PRICE;
                        decimal saving = 0, saving_percent = 0;
                        bool validate = false;


                        if (oldPrice > item.Price)
                        {
                            saving = oldPrice - item.Price;
                            saving_percent = saving / oldPrice * 100;
                            validate = true;
                        }
                        else if (oldPrice < item.Price)
                        {
                            oldPrice = 0;
                            validate = true;
                        }
                        else if (item.Name != data.NAME)
                        {
                            validate = true;
                        }


                        if (validate)
                        {
                            context.SP_UPDATE_PRICE(data.ID, item.Price, oldPrice, saving, saving_percent, item.Name, item.Image);
                        }


                    }
                    else if (item.Save)
                    {
                        context.SP_ADD(item.Name, item.Price, item.Link, item.Condition, item.Shop, item.Image, item.Type);

                    }

                    context.SaveChanges();
                }
            });
        }

    }
}