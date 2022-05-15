using DataBase;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using WebScraping.Classes;


namespace WebScraping
{
   class Program : Method
    {
        static async Task Main(string[] args)
        {
            LoadBlackList();

            try
            {
                var task1 = Task.Run(() =>
                {
                     Selenium("amazon");
                });

                var task2 = Task.Run(() =>
                {
                    Selenium("thestore");
                });

                var task3 = Task.Run(() =>
                {
                    Selenium("eBay");

                });

                await Task.WhenAll(task1, task2, task3);
                UpdateItems();

                SystemSounds.Asterisk.Play();
                WriteLogs("Done\a");
            }
            catch (Exception e)
            {
                UpdateItems();
                WriteLogs($"\nERROR ----> {e}");
            }

        }

        static void UpdateItems()
        {
            var listItems = new List<SP_GetAllLinks_Result>();

            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                listItems = context.SP_GetAllLinks().ToList();
            }

            Parallel.ForEach(listItems, (i) =>
            {
                using (WebScrapingEntities context = new WebScrapingEntities())
                {
                    context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                    if (checkList.Contains(i.link))
                    {
                        context.SP_UPDATE_STATUS(i.id, 1);
                    }
                    else
                    {
                        context.SP_UPDATE_STATUS(i.id, 2);
                    }
                }
            });
        }

        static void LoadBlackList()
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                blackList = context.SP_GET_BLACK_LIST().ToList();
                Console.WriteLine("Loaded black list\n");
                CreateLogDir();
            }
        }


    }
}
