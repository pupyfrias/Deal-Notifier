using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Data;
using System.Diagnostics;
using WebScraping.Core.Application.Dtos;
using WebScraping.Core.Application.Heplers;
using WebScraping.Core.Application.Mappings;
using WebScraping.Core.Application.Utils;
using WebScraping.Infrastructure.Persistence.DbContexts;
using WebScraping.Infrastructure.Persistence.Models;

namespace WebScraping
{
    public class Program
    {
        private static ILogger _logger;
        private static IMapper _mapperConfiguration;

        private static async Task Main(string[] args)
        {
            _logger = Logger.CreateLogger().ForContext<Program>();
            _mapperConfiguration = new MapperConfiguration(x => x.AddProfile(new AutomapperConfig())).CreateMapper();

            await LoadData();
            try
            {
                var amazonTask = Task.Run(() =>
                {
                    //Amazon.Run();
                });

                var theStoreTask = Task.Run(() =>
                {
                    //TheStore.Run();
                });

                var eBayTask = Task.Run(async () =>
                {
                    await Timer(Ebay.RunAsync);
                });

                await Task.WhenAll(amazonTask, theStoreTask, eBayTask);
            }
            catch (Exception e)
            {
                _logger.Error(e.ToString());
            }
            finally
            {
                await Timer(UpdateItems);
            }
        }




        private static void Timer(Action action)
        {
            var timer = new Stopwatch();
            timer.Start();
            action();
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            string time = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
            _logger.Information(time);

        }

        private static async Task Timer(Func<Task> action)
        {
            var timer = new Stopwatch();
            timer.Start();
            await action();
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            string time = "Time taken: " + timeTaken.ToString(@"m\:ss\.fff");
            _logger.Information(time);

        }


        private static async Task UpdateItems()
        {
            using (var context = new ApplicationDbContext())
            {
                var listId = Helper.CheckedList.Select(x => x.Replace("https://www.ebay.com/itm/", ""));
                string query = "EXEC UPDATE_STATUS_EBAY @ListId, @OutputResult OUTPUT";
                var listIdParameter = new SqlParameter("@ListId", string.Join(',', listId));
                var outputResult = new SqlParameter("@OutputResult", SqlDbType.Bit) { Direction = ParameterDirection.Output };

                await context.Database.ExecuteSqlRawAsync(query, listIdParameter, outputResult);
                bool result = (bool)outputResult.Value;
                _logger.Information($"El resultado es: {result}");
            }

            _logger.Information($"{Helper.CheckedList.Count} Items In Stock");
        }


        private static async Task LoadData()
        {
            Task blackList = Task.Run(async () =>
            {
                await LoadBlackList();
            });

            Task bannedKeywordList = Task.Run(async () =>
            {
                await LoadBannedKeyword();
            });

            await Task.WhenAll(blackList, bannedKeywordList);
            _logger.Information("All needed data Loaded");
        }


        private static async Task LoadBlackList()
        {
            using (var context = new ApplicationDbContext())
            {
                var backList = await context.BlackLists
                    .ProjectTo<BlackListDto>(_mapperConfiguration.ConfigurationProvider)
                    .ToListAsync();

                Helper.BlacklistedLinks = backList.ToHashSet<BlackListDto>();
            }
        }

        private static async Task LoadBannedKeyword()
        {
            using (var context = new ApplicationDbContext())
            {
                var banedList = await context.Banned
                    .ProjectTo<BannedDto>(_mapperConfiguration.ConfigurationProvider)
                    .ToListAsync();

                Helper.BannedKeywordList = banedList.ToHashSet<BannedDto>();
            }
        }
    }
}