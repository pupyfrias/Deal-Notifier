using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.Unlockable;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Configs;
using DealNotifier.Core.Domain.Entities;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Enums = DealNotifier.Core.Application.Enums;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;

namespace WorkerService.T_Unlock_WebScraping
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUnlockableService _unlockableService;
        private readonly IPhoneCarrierService _phoneCarrierService;
        private readonly IUnlockabledPhonePhoneUnlockToolService _unlockableUnlockToolService;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockablePhoneCarrierService;
        private Timer _timer;
        private IEnumerable<PhoneCarrierDto> phoneCarrierList = new List<PhoneCarrierDto>();
        private readonly TUnlockUrlConfig _tUnlockUrlConfig;

        public Worker(ILogger logger, IUnlockableService unlockableService,
            IPhoneCarrierService phoneCarrierService,
            IUnlockabledPhonePhoneUnlockToolService unlockableUnlockToolService,
            IUnlockabledPhonePhoneCarrierService unlockablePhoneCarrierService,
            IOptions<TUnlockUrlConfig> tUnlockUrlConfig,
            IMapper mapper
            )
        {
            _logger = logger;
            _unlockableService = unlockableService;
            _phoneCarrierService = phoneCarrierService;
            _unlockableUnlockToolService = unlockableUnlockToolService;
            _unlockablePhoneCarrierService = unlockablePhoneCarrierService;
            _tUnlockUrlConfig = tUnlockUrlConfig.Value;
            _mapper = mapper;
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

            phoneCarrierList =  await _phoneCarrierService.GetAllAsync<PhoneCarrierDto>();
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
            foreach (var path in _tUnlockUrlConfig.Paths)
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

                    var possibleUnlockable = await _unlockableService.GetByModelNumberAsync(modelNumbre);

                    if (possibleUnlockable == null)
                    {
                        var unlockableCreateDto = new UnlockableCreateDto
                        {
                            BrandId = GetBrandId(path),
                            ModelName = modelName,
                            ModelNumber = modelNumbre
                        };

                        /*var model = await _unlockableService.CreateAsync<UnlockableCreateDto>(unlockableCreateDto);*/

                       /* var unlockableUnlockTool = new UnlockabledPhonePhoneUnlockTool
                        {
                            UnlockabledPhoneId= model.Id,
                            PhoneUnlockToolId = (int)Enums.UnlockTool.TUnlock
                        };*/
                        //await _unlockableUnlockToolService.CreateAsync(unlockableUnlockTool);

                        foreach (var carrier in carrierList)
                        {/*
                            var phoneCarrier = phoneCarrierList.FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (phoneCarrier != null)
                            {
                                var unlockablePhoneCarrier = new UnlockabledPhonePhoneCarrier
                                {
                                    UnlockabledPhoneId = model.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                await _unlockablePhoneCarrierService.CreateAsync(unlockablePhoneCarrier);
                            }*/
                        }
                    }
                    else
                    {
                        foreach (var carrier in carrierList)
                        {
                            var phoneCarrier = phoneCarrierList.FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (phoneCarrier != null)
                            {
                                var unlockablePhoneCarrierDto = new UnlockabledPhonePhoneCarrierDto
                                {
                                    UnlockabledPhoneId = possibleUnlockable.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                bool exists = await _unlockablePhoneCarrierService.ExistsAsync(unlockablePhoneCarrierDto);

                                if (!exists)
                                {
                                    var unlockablePhoneCarrierCreate = _mapper.Map<UnlockabledPhonePhoneCarrierCreateRequest>(unlockablePhoneCarrierDto);
                                    await _unlockablePhoneCarrierService.CreateAsync(unlockablePhoneCarrierCreate);
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