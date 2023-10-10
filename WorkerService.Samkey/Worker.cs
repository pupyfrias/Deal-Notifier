using AutoMapper;
using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;

namespace SamkeyDataSyncWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly SamkeyUrlConfig _samkeyUrlConfig;
        private IUnlockabledPhoneService _unlockabledPhoneService;
        private IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;
        private IUnlockabledPhonePhoneUnlockToolService _unlockabledPhonePhoneUnlockToolService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private Timer _timer;
        private IEnumerable<PhoneCarrierDto> phoneCarrierList = new List<PhoneCarrierDto>();

        public Worker(
            ILogger logger,
            IOptions<SamkeyUrlConfig> samkeyUrlConfig,
            IMapper mapper,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            _samkeyUrlConfig = samkeyUrlConfig.Value;
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
            _logger.Information("Starting ExecuteAsync.");
            TimerCallback callback = async (state) => await TimerElapsed();

            _timer = new Timer(callback, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            await Task.Delay(-1, stoppingToken);
            _logger.Information("ExecuteAsync finished.");
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

        private async Task ScrappingAsync()
        {
            string autoCompletePath = _samkeyUrlConfig.Paths.AutoComplete;
            string supportedPath = _samkeyUrlConfig.Paths.Supported;
            var autoCompleteData = new Dictionary<string, string> { { "query", "s" } };
            var autoCompleteResponse = await SendHttpPostAsync(autoCompletePath, autoCompleteData);

            var phoneList = JsonSerializer.Deserialize<List<string>>(autoCompleteResponse);

            try
            {
                _logger.Information(phoneList!.Count.ToString());
                foreach (var phone in phoneList)
                {
                    var splitPhone = phone.Split(" | ");

                    if (splitPhone.Length < 2) continue;

                    var modelNumber = splitPhone[0];
                    var modelName = splitPhone[1];

                    var supportedData = new Dictionary<string, string> { { "model", modelNumber }, { "getsupported", "true" } };
                    var supportedResponse = await SendHttpPostAsync(supportedPath, supportedData);

                    if (supportedResponse == "This model not supported") continue;

                    var carriers = Regex.Match(supportedResponse, "(?<=\"supportcarriers\\\":\\\")[^\"]*").Value;

                    carriers = carriers.Replace("VirginMobile", "Virgin Mobile")
                                       .Replace("carrier", "", StringComparison.OrdinalIgnoreCase)
                                       .Replace("carriers", "", StringComparison.OrdinalIgnoreCase);

                    var carrierList = Regex.Replace(carriers, "[\\(\\),: }]+", "|", RegexOptions.IgnoreCase)
                        .Split("|");

                    var possibleUnlockedPhone = await _unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone => unlockedPhone.ModelNumber.Equals(modelNumber));

                    if (possibleUnlockedPhone == null)
                    {
                        var unlockableCreateDto = new UnlockabledPhoneCreateRequest
                        {
                            BrandId = (int)Brand.Samsung,
                            ModelName = modelName,
                            ModelNumber = modelNumber
                        };

                        var newUnlockedPhone = await _unlockabledPhoneService.CreateAsync<UnlockabledPhoneCreateRequest, UnlockabledPhoneResponse>(unlockableCreateDto);

                        var unlockabledPhonePhoneUnlockTool = new UnlockabledPhonePhoneUnlockToolCreate
                        {
                            UnlockabledPhoneId = newUnlockedPhone.Id,
                            PhoneUnlockToolId = (int)UnlockTool.SamKey
                        };

                        await _unlockabledPhonePhoneUnlockToolService.CreateAsync(unlockabledPhonePhoneUnlockTool);

                        foreach (var carrier in carrierList)
                        {
                            
                            var phoneCarrier = phoneCarrierList.FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                           
                            if (phoneCarrier != null)
                            {
                                var unlockablePhoneCarrierDto = new UnlockabledPhonePhoneCarrierDto
                                {
                                    UnlockabledPhoneId = newUnlockedPhone.Id,
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
                    else
                    {
                        var unlockabledPhonePhoneUnlockToolDto = new UnlockabledPhonePhoneUnlockToolDto
                        {
                            UnlockabledPhoneId = possibleUnlockedPhone.Id,
                            PhoneUnlockToolId = (int)UnlockTool.SamKey
                        };


                        bool existsUnlockabledPhoneUnlockTool = await _unlockabledPhonePhoneUnlockToolService.ExistsAsync(unlockabledPhonePhoneUnlockToolDto);
                        if (!existsUnlockabledPhoneUnlockTool)
                        {
                            var unlockablePhoneUnlockToolCreate = _mapper.Map<UnlockabledPhonePhoneUnlockToolCreate>(unlockabledPhonePhoneUnlockToolDto);
                            await _unlockabledPhonePhoneUnlockToolService.CreateAsync(unlockablePhoneUnlockToolCreate);
                        }

                        


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
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        private async Task<string> SendHttpPostAsync(string path, Dictionary<string, string> data)
        {
            string responseBody = string.Empty;
            string urlBase = _samkeyUrlConfig.Base;
            string url = urlBase + path;

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Origin", urlBase);
            client.DefaultRequestHeaders.Add("Referer", urlBase);

            HttpContent content = new FormUrlEncodedContent(data);
            HttpResponseMessage response = await client.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                responseBody = await response.Content.ReadAsStringAsync();
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }

            return responseBody;
        }

        private async Task TimerElapsed()
        {
            try
            {
                _logger.Information($"Timer elapsed. Running Samkey Scraping Service Init. {DateTime.Now}");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                    _unlockabledPhonePhoneCarrierService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhonePhoneCarrierService>();
                    _unlockabledPhonePhoneUnlockToolService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhonePhoneUnlockToolService>();
                    var phoneCarrierService = scope.ServiceProvider.GetRequiredService<IPhoneCarrierService>();
                    phoneCarrierList = await phoneCarrierService.GetAllAsync<PhoneCarrierDto>();
                    await ScrappingAsync();

                }
                _logger.Information("Samkey Scraping Service completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}