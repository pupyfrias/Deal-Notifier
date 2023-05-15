using HtmlAgilityPack;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Application.Dtos.PhoneCarrier;
using WebScraping.Core.Application.Dtos.Unlockable;
using WebScraping.Core.Domain.Entities;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;
using Enums = WebScraping.Core.Application.Enums;
using WebScraping.Core.Domain.Configs;
using Microsoft.Extensions.Options;


namespace WorkerService.T_Unlock_WebScraping
{
    public class Worker : BackgroundService
    {

        private readonly ILogger _logger;
        private readonly IUnlockableServiceAsync _unlockableServiceAsync;
        private readonly IPhoneCarrierServiceAsync _phoneCarrierServiceAsync;
        private readonly IUnlockableUnlockToolServiceAsync _unlockableUnlockToolServiceAsync;
        private readonly IUnlockablePhoneCarrierServiceAsync _unlockablePhoneCarrierServiceAsync;
        private Timer _timer;
        private List<PhoneCarrierReadDto> phoneCarrierList = new List<PhoneCarrierReadDto>();
        private readonly TUnlockUrlConfig _tUnlockUrlConfig;

        public Worker(ILogger logger, IUnlockableServiceAsync unlockableServiceAsync,
            IPhoneCarrierServiceAsync phoneCarrierServiceAsync,
            IUnlockableUnlockToolServiceAsync unlockableUnlockToolServiceAsync,
            IUnlockablePhoneCarrierServiceAsync unlockablePhoneCarrierServiceAsync,
            IOptions<TUnlockUrlConfig> tUnlockUrlConfig
            )
        {
            _logger = logger;
            _unlockableServiceAsync = unlockableServiceAsync;
            _phoneCarrierServiceAsync = phoneCarrierServiceAsync;
            _unlockableUnlockToolServiceAsync = unlockableUnlockToolServiceAsync;
            _unlockablePhoneCarrierServiceAsync = unlockablePhoneCarrierServiceAsync;
            _tUnlockUrlConfig = tUnlockUrlConfig.Value;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information("Starting ExecuteAsync.");
                TimerCallback callback = async (state) => await TimerElapsed();

                _timer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromHours(1));
                await Task.Delay(-1, stoppingToken);
                _logger.Information("ExecuteAsync finished.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
           
        }

        /*protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information("Starting ExecuteAsync.");

                TimerCallback callback = async (state) =>
                {
                    var lastExecutionTimeEnvVar = Environment.GetEnvironmentVariable("LastExecutionTime");

                    DateTime lastExecutionTime;
                    if (lastExecutionTimeEnvVar == null || !DateTime.TryParse(lastExecutionTimeEnvVar, out lastExecutionTime))
                    {
                        lastExecutionTime = DateTime.MinValue;
                    }

                    if ((DateTime.Now - lastExecutionTime).TotalDays >= 7)
                    {
                        await TimerElapsed();
                        Environment.SetEnvironmentVariable("LastExecutionTime", DateTime.Now.ToString());
                    }
                };

                // Set the timer to check every hour (or every day, or whatever interval you prefer)
                _timer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromHours(1));

                await Task.Delay(-1, stoppingToken);
                _logger.Information("ExecuteAsync finished.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }*/



        private async Task TimerElapsed()
        {
            _logger.Information($"Timer elapsed. Running T-Unlock Scraping Service Init. {DateTime.Now}");

            phoneCarrierList = await _phoneCarrierServiceAsync.GetAllAsync<PhoneCarrierReadDto>();
            await Scrapping();
            _logger.Information("T-Unlock Scraping Service completed.");

           
           
        }


        private int GetBrandId(string path)
        {
            Dictionary<string, int> keyValues = new Dictionary<string, int>
            {
                { "samsung", (int) Enums.Brand.Samsung },
                { "motorola", (int) Enums.Brand.Motorola },
                { "lg", (int) Enums.Brand.LG },
            };

            return keyValues.TryGetValue(path, out int brandId) ? brandId : (int)Enums.Brand.Unknown;
        }


        private async Task Scrapping()
        {
            foreach(var path in _tUnlockUrlConfig.Paths)
            {
                string urlBase = _tUnlockUrlConfig.Base;
                string url = urlBase + path;
                _logger.Information(url);
                var httpClient = new HttpClient();
                var html = await httpClient.GetStringAsync(url);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(html);

                var divs = htmlDocument.DocumentNode.SelectNodes("//div[@class='MyCRD']");

                foreach (var div in divs)
                {
                    var thead = div.SelectSingleNode(".//thead");
                    var h7List = thead.Descendants("h7").ToList();
                    var h4 = div.SelectSingleNode(".//h4");

                    var modelNumbre = h7List[0].InnerText;
                    var modelName = h7List[1].InnerText;
                    var carrierList = h4.InnerText.Split(',');


                    var possibleUnlockable = await _unlockableServiceAsync.GetByModelNumberAsync(modelNumbre);

                    if (possibleUnlockable == null)
                    {
                        var unlockableCreateDto = new UnlockableCreateDto
                        {
                            BrandId = GetBrandId(path),
                            ModelName = modelName,
                            ModelNumber = modelNumbre
                        };


                        var model = await _unlockableServiceAsync.CreateAsync(unlockableCreateDto);

                        var unlockableUnlockTool = new UnlockableUnlockTool
                        {
                            UnlockableId = model.Id,
                            UnlockToolId = (int)Enums.UnlockTool.TUnlock
                        };
                        await _unlockableUnlockToolServiceAsync.CreateAsync(unlockableUnlockTool);

                        foreach (var carrier in carrierList)
                        {
                            var phoneCarrier = phoneCarrierList.FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (phoneCarrier != null)
                            {
                                var unlockablePhoneCarrier = new UnlockablePhoneCarrier
                                {
                                    UnlockableId = model.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                await _unlockablePhoneCarrierServiceAsync.CreateAsync(unlockablePhoneCarrier);
                            }
                        }
                    }
                    else
                    {
                        foreach (var carrier in carrierList)
                        {
                            var phoneCarrier = phoneCarrierList.FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (phoneCarrier != null)
                            {
                                var unlockablePhoneCarrier = new UnlockablePhoneCarrier
                                {
                                    UnlockableId = possibleUnlockable.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                bool exists = await _unlockablePhoneCarrierServiceAsync.ExistsAsync(unlockablePhoneCarrier);

                                if (!exists)
                                {
                                    await _unlockablePhoneCarrierServiceAsync.CreateAsync(unlockablePhoneCarrier);
                                }
                            }
                            else
                            {
                                _logger.Warning($"Carrier [{carrier}] no exists on DataBase");
                            }
                        }
                    }
                }
            }
        }
       
    }
}