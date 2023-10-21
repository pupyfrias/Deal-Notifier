using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using HtmlAgilityPack;
using WorkerService.T_Unlock_WebScraping.Helpers;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using Brand = DealNotifier.Core.Application.Enums.Brand;
using ILogger = Serilog.ILogger;

namespace WorkerService.T_Unlock_WebScraping.Services
{
    public class TUnlockPhoneProcessService : ITUnlockPhoneProcessService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger _logger;

        public TUnlockPhoneProcessService(
            IServiceScopeFactory serviceScopeFactory, 
            ILogger logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        public async Task ProcessAsync(string pageHtml, Brand brand)
        {
            var htmlNodeCollection = HtmlNodeMapper.MapStringToHtmlNodeCollection(pageHtml);
            var tasks = htmlNodeCollection.Select(async htmlNode =>
            {
                await ProcessSinglePhoneAsync(htmlNode, brand);
            });

            await Task.WhenAll(tasks);
        }


       
        private async Task ProcessSinglePhoneAsync(HtmlNode htmlNode, Brand brand)
        {

            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                    var unlockedPhoneDetails = HtmlNodeMapper.MapHtmlNodeToPhoneDetails(htmlNode);

                    var possibleUnlockedPhone = await unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone =>
                        unlockedPhone.ModelNumber.Equals(unlockedPhoneDetails.ModelNumber));

                    if (possibleUnlockedPhone == null)
                    {
                        await unlockabledPhoneService.HandleNewUnlockedPhoneAsync(unlockedPhoneDetails, brand, UnlockTool.TUnlock);
                    }
                    else
                    {
                        await unlockabledPhoneService.HandleExistingUnlockedPhoneAsync(possibleUnlockedPhone,
                            unlockedPhoneDetails.Carriers, UnlockTool.TUnlock);
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