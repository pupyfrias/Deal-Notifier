using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Helpers;
using DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Interfaces;
using HtmlAgilityPack;
using Brand = DealNotifier.Core.Application.Enums.Brand;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.GlobalUnlockerDataSyncWorker.Services
{
    public class GlobalUnlockerPhoneProcessService : IGlobalUnlockerPhoneProcessService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public GlobalUnlockerPhoneProcessService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task ProcessAsync(string pageHtml, Brand brand)
        {
            var htmlNodeCollection = HtmlNodeMapper.MapStringToHtmlNodeCollection(pageHtml);

            foreach (var htmlNode in htmlNodeCollection)
            {
                await ProcessSinglePhoneAsync(htmlNode, brand);
            }
        }



        private async Task ProcessSinglePhoneAsync(HtmlNode htmlNode, Brand brand)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                    var unlockedPhoneDetails = HtmlNodeMapper.MapHtmlNodeToPhoneDetails(htmlNode);

                    if (unlockedPhoneDetails == null) return;
                    
                    var possibleUnlockedPhone = await unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone =>
                        unlockedPhone.ModelNumber.Equals(unlockedPhoneDetails.ModelNumber));

                    if (possibleUnlockedPhone == null)
                    {
                        await unlockabledPhoneService.HandleNewUnlockedPhoneAsync(unlockedPhoneDetails, brand, UnlockTool.GlobalUnlocker);
                    }
                    else
                    {
                        await unlockabledPhoneService.HandleExistingUnlockedPhoneAsync(possibleUnlockedPhone,
                            unlockedPhoneDetails.Carriers, UnlockTool.GlobalUnlocker);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Warning($"Error while processing brand: {brand}. Node: {htmlNode.OuterHtml}. Exception: {ex.Message}. InnerException => {ex.InnerException?.Message}");
            }

        }
    }
}