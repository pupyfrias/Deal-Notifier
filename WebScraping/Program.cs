using DataBase;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Threading.Tasks;
using WebScraping.Heplers;
using WebScraping.Models;
using WebScraping.Services;
using WebScraping.Utils;

namespace WebScraping
{
    class Program : ItemService
    {
        private static ILogger _logger;
        static async Task Main(string[] args)
        {
            _logger = LogConsole.CreateLogger<Program>();

            LoadBlackList();
            try
            {
                var amazonTask = Task.Run(() =>
                {
                    Amazon.Run();
                });

                var theStoreTask = Task.Run(() =>
                {
                    TheStore.Run();
                });

                var eBayTask = Task.Run(() =>
                {
                   Ebay.Run();
                });

                await Task.WhenAll(amazonTask, theStoreTask, eBayTask);
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                LogFile.Write<Program>(e.ToString());
            }
            finally
            {
                UpdateItems();
                _logger.LogInformation("Done");
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

                    if (Helper.checkList.Contains(i.link))
                    {
                        context.SP_UPDATE_STATUS(i.id, (int) Emuns.Status.InStock);
                    }
                    else
                    {
                        context.SP_UPDATE_STATUS(i.id, (int)Emuns.Status.OutStock);
                    }
                }
            });


            _logger.LogInformation($"{Helper.checkList.Count} Items In Stock");
            SystemSounds.Asterisk.Play();
        }

        static void LoadBlackList()
        {
            using (WebScrapingEntities context = new WebScrapingEntities())
            {
                context.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
                Helper.linkBlackList = context.SP_GET_BLACK_LIST().ToList();
                _logger.LogInformation("Blacklist loaded");
            }
        }

    }
}
