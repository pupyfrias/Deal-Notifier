using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Configs;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using Enums = DealNotifier.Core.Application.Enums;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;

namespace WorkerService.T_UnlokcDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private IEnumerable<PhoneCarrierDto> phoneCarrierList = new List<PhoneCarrierDto>();
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private IPhoneCarrierService _phoneCarrierService;
        private IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;
        private IUnlockabledPhonePhoneUnlockToolService _unlockabledPhonePhoneUnlockToolService;
        private IUnlockabledPhoneService _unlockabledPhoneService;
        private readonly TUnlockUrlConfig _tUnlockUrlConfig;
        private Timer _timer;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public Worker(
            ILogger logger,
            IOptions<TUnlockUrlConfig> tUnlockUrlConfig,
            IMapper mapper,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            _tUnlockUrlConfig = tUnlockUrlConfig.Value;
            _mapper = mapper;
            _serviceScopeFactory = serviceScopeFactory;
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

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                _unlockabledPhonePhoneCarrierService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhonePhoneCarrierService>();
                _unlockabledPhonePhoneUnlockToolService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhonePhoneUnlockToolService>();
                _phoneCarrierService = scope.ServiceProvider.GetRequiredService<IPhoneCarrierService>();


                phoneCarrierList = await _phoneCarrierService.GetAllAsync<PhoneCarrierDto>();
                await ScrappingAsync();
                _logger.Information("T-Unlock Scraping Service completed.");
            }
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

        private async Task ScrappingAsync()
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

                    var modelNumber = h7List[0].InnerText;
                    var modelName = h7List[1].InnerText;
                    var carrierList = h4.InnerText.Split(',');
                    

                    var possibleUnlockedPhone = await _unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone=> unlockedPhone.ModelNumber.Equals(modelNumber));

                    if (possibleUnlockedPhone == null)
                    {
                        var unlockableCreateDto = new UnlockabledPhoneCreateRequest
                        {
                            BrandId = GetBrandId(path),
                            ModelName = modelName,
                            ModelNumber = modelNumber
                        };

                        var newUnlockedPhone = await _unlockabledPhoneService.CreateAsync<UnlockabledPhoneCreateRequest, UnlockabledPhoneResponse>(unlockableCreateDto);

                        var unlockabledPhonePhoneUnlockTool = new UnlockabledPhonePhoneUnlockToolCreate
                        {
                            UnlockabledPhoneId = newUnlockedPhone.Id,
                            PhoneUnlockToolId = (int) Enums.UnlockTool.TUnlock
                        };
                        await _unlockabledPhonePhoneUnlockToolService.CreateAsync(unlockabledPhonePhoneUnlockTool);

                        foreach (var carrier in carrierList)
                        {
                            var phoneCarrier = phoneCarrierList.FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (phoneCarrier != null)
                            {
                                var unlockablePhoneCarrier = new UnlockabledPhonePhoneCarrierCreateRequest
                                {
                                    UnlockabledPhoneId = newUnlockedPhone.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                await _unlockabledPhonePhoneCarrierService.CreateAsync(unlockablePhoneCarrier);
                            }
                            else
                            {
                                _logger.Warning($"Carrier [{carrier}] no exists on DataBase");
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
                                var unlockablePhoneCarrierDto = new UnlockabledPhonePhoneCarrierDto
                                {
                                    UnlockabledPhoneId = possibleUnlockedPhone.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                bool existsUnlockabledPhonePhoneCarrier = await _unlockabledPhonePhoneCarrierService.ExistsAsync(unlockablePhoneCarrierDto);

                                if (!existsUnlockabledPhonePhoneCarrier)
                                {
                                    var unlockablePhoneCarrierCreate = _mapper.Map<UnlockabledPhonePhoneCarrierCreateRequest>(unlockablePhoneCarrierDto);
                                    await _unlockabledPhonePhoneCarrierService.CreateAsync(unlockablePhoneCarrierCreate);
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