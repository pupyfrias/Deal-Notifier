using DataBase;
using Microsoft.Extensions.Logging;
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
        private static new ILogger _logger;
        static async Task Main(string[] args)
        {
            _logger = CreateLogger<Program>();

            LoadBlackList();
            try
            {
                var task1 = Task.Run(() =>
                {
                    var amazon = new Amazon();
                    amazon.Run();
                });

                var task2 = Task.Run(() =>
                {
                    var theStore = new TheStore();
                    theStore.Run();
                });

                var task3 = Task.Run(() =>
                {
                    var ebay = new Ebay();
                    ebay.Run();

                });

                await Task.WhenAll(task1, task2, task3);
                UpdateItems();
                _logger.LogInformation("Done");
            }
            catch (Exception e)
            {
                UpdateItems();
                _logger.LogError(e.ToString());
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
                        context.SP_UPDATE_STATUS(i.id, (int) Emuns.Status.InStock);
                    }
                    else
                    {
                        context.SP_UPDATE_STATUS(i.id, (int)Emuns.Status.OutStock);
                    }
                }
            });


            _logger.LogInformation($"{checkList.Count} Items In Stock");
            SystemSounds.Asterisk.Play();
        }

        static void LoadBlackList()
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                blackList = context.SP_GET_BLACK_LIST().ToList();
                _logger.LogInformation("Blacklist loaded");
                CreateLogDir();
            }
        }

    }
}
