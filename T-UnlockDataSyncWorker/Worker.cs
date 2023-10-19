using AutoMapper;
using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Configs;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using Enums = DealNotifier.Core.Application.Enums;
using ILogger = Serilog.ILogger;
using Timer = System.Threading.Timer;

namespace WorkerService.T_Unlock_WebScraping
{
    public class Worker : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly IDataSynchronizerTUnlockService _dataSynchronizerTUnlockService;

        public Worker(
            ILogger logger,
            IServiceScopeFactory serviceScopeFactory
            )
        {
            _logger = logger;
            var scope = serviceScopeFactory.CreateScope();
            _dataSynchronizerTUnlockService = scope.ServiceProvider.GetRequiredService<IDataSynchronizerTUnlockService>();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.Information("Starting ExecuteAsync.");
                await _dataSynchronizerTUnlockService.InitializeAsync();
                _logger.Information("ExecuteAsync finished.");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
        }
    }
}