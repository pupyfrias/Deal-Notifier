﻿using Microsoft.EntityFrameworkCore;
using Serilog;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Application.Utils;
using WebScraping.Infrastructure.Persistence.DbContexts;
using WebScraping.Infrastructure.Persistence.Models;
using Emuns = WebScraping.Core.Application.Emuns;

namespace WebScraping
{
    class Program
    {
        private static ILogger _logger;
        static async Task Main(string[] args)
        {
            _logger = Logger.CreateLogger().ForContext<Program>();
            
            await LoadBlackList();
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
                _logger.Error(e.ToString());
            }
            finally
            {
                await UpdateItems();
                _logger.Information("Done");
            }

        }

        static async Task UpdateItems()
        {
            Task markingToInStock = Task.Run(async () =>
            {
                using (var context = new ApplicationDbContext())
                {
                    var ItemListToMarkInStock = await context.Items
                                            .Where(x => x.StatusId != (int)Emuns.Status.InStock &&
                                                   Helper.checkedItemList.Contains(x.Link))
                                            .ToListAsync();

                    Parallel.ForEach(ItemListToMarkInStock, (item) =>
                    {
                        item.StatusId = (int)Emuns.Status.InStock;
                    });
                    await context.SaveChangesAsync();
                }
            });

            Task markingToOutStock = Task.Run(async () =>
            {
                using (var context = new ApplicationDbContext())
                {
                    var ItemListToMarkOutStock = await context.Items
                                            .Where(x => x.StatusId == (int)Emuns.Status.InStock &&
                                                   !Helper.checkedItemList.Contains(x.Link))
                                            .ToListAsync();

                    Parallel.ForEach(ItemListToMarkOutStock, (item) =>
                    {
                        item.StatusId = (int)Emuns.Status.OutStock;
                    });
                    await context.SaveChangesAsync();
                }
            });

            await Task.WhenAll(markingToInStock, markingToOutStock);
            _logger.Information($"{Helper.checkedItemList.Count} Items In Stock");
            Console.Beep(500, 800);
        }
        

        static async Task LoadBlackList()
        {
            using (var context = new ApplicationDbContext())
            {
                string query = "Exec SP_GET_BLACK_LIST";
               // Helper.linkBlackList = await context.SpBlackList.FromSqlRaw(query).ToListAsync();
                _logger.Information("Blacklist loaded");
            }
        }

    }
}
