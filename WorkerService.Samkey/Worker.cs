using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Configs;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.RegularExpressions;
using Enums = DealNotifier.Core.Application.Enums;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;

namespace WorkerService.Samkey
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPhoneCarrierService _phoneCarrierService;
        private readonly SamkeyUrlConfig _samkeyUrlConfig;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockablePhoneCarrierService;
        private readonly IUnlockabledPhoneService _unlockabledPhoneService;
        private readonly IUnlockabledPhonePhoneUnlockToolService _unlockableUnlockToolService;
        private Timer _timer;
        private List<PhoneCarrierDto> phoneCarrierList = new List<PhoneCarrierDto>();

        public Worker(ILogger logger, IUnlockabledPhoneService unlockableService,
            IPhoneCarrierService phoneCarrierService,
            IUnlockabledPhonePhoneUnlockToolService unlockableUnlockToolService,
            IUnlockabledPhonePhoneCarrierService unlockablePhoneCarrierService,
            IOptions<SamkeyUrlConfig> samkeyUrlConfig,
            IMapper mapper
            )
        {
            _logger = logger;
            _unlockabledPhoneService = unlockableService;
            _phoneCarrierService = phoneCarrierService;
            _unlockableUnlockToolService = unlockableUnlockToolService;
            _unlockablePhoneCarrierService = unlockablePhoneCarrierService;
            _samkeyUrlConfig = samkeyUrlConfig.Value;
            _mapper = mapper;
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

        private async Task Scrapping()
        {
            string autoCompletepath = _samkeyUrlConfig.Paths.AutoComplete;
            string supportedpath = _samkeyUrlConfig.Paths.Supported;
            var autoCompleteData = new Dictionary<string, string> { { "query", "s" } };
            var autoCompleteResponse = await SendHttpPost(autoCompletepath, autoCompleteData);

            var phoneList = JsonSerializer.Deserialize<List<string>>(autoCompleteResponse);

            try
            {
                var counter = 0;
                _logger.Information(phoneList.Count.ToString());
                foreach (var phone in phoneList)
                {
                    var splitPhone = phone.Split(" | ");

                    if (splitPhone.Length < 2) continue;

                    var modelNumber = splitPhone[0];
                    var modelName = splitPhone[1];

                    var supportedData = new Dictionary<string, string> { { "model", modelNumber }, { "getsupported", "true" } };
                    var supportedResponse = await SendHttpPost(supportedpath, supportedData);

                    if (supportedResponse == "This model not supported") continue;

                    var carriers = Regex.Match(supportedResponse, "(?<=\"supportcarriers\\\":\\\")[^\"]*").Value;

                    carriers = carriers.Replace("VirginMobile", "Virgin Mobile")
                                       .Replace("carrier", "", StringComparison.OrdinalIgnoreCase)
                                       .Replace("carriers", "", StringComparison.OrdinalIgnoreCase);

                    var carrierList = Regex.Replace(carriers, "[\\(\\),: }]+", "|", RegexOptions.IgnoreCase)
                        .Split("|");

                    var possibleUnlockable = await _unlockabledPhoneService.GetByModelNumberAsync(modelNumber);

                    if (possibleUnlockable == null)
                    {
                        var unlockableCreateDto = new UnlockableCreateDto
                        {
                            BrandId = (int)Enums.Brand.Samsung,
                            ModelName = modelName,
                            ModelNumber = modelNumber
                        };

                        //var model = await _unlockableService.CreateAsync<UnlockableCreateDto>(unlockableCreateDto);

/*                        var unlockableUnlockTool = new UnlockabledPhonePhoneUnlockTool
                        {
                            UnlockabledPhoneId = model.Id,
                            PhoneUnlockToolId = (int)Enums.UnlockTool.SamKey
                        };*/
                        //await _unlockableUnlockToolService.CreateAsync(unlockableUnlockTool);

                        foreach (var carrier in carrierList)
                        {
                            var phoneCarrier = phoneCarrierList
                                .FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase) ||
                                pc.ShortName.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
                            if (phoneCarrier != null)
                            {
                                /*var unlockablePhoneCarrier = new UnlockabledPhonePhoneCarrier
                                {
                                    UnlockabledPhoneId = model.Id,
                                    PhoneCarrierId = phoneCarrier.Id,
                                };

                                await _unlockablePhoneCarrierService.CreateAsync(unlockablePhoneCarrier);*/
                            }
                        }
                    }
                    else
                    {
                        foreach (var carrier in carrierList)
                        {
                            var phoneCarrier = phoneCarrierList
                                .FirstOrDefault(pc => pc.Name.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase) ||
                                pc.ShortName.Contains(carrier.Trim(), StringComparison.OrdinalIgnoreCase));
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

                    /* counter++;
                     _logger.Information(counter.ToString());*/
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message, ex);
            }
        }

        private async Task<string> SendHttpPost(string path, Dictionary<string, string> data)
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

                phoneCarrierList = (List<PhoneCarrierDto>)await _phoneCarrierService.GetAllAsync<PhoneCarrierDto>();
                await Scrapping();
                _logger.Information("Samkey Scraping Service completed.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}