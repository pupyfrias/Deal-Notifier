using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using HtmlAgilityPack;
using WorkerService.T_Unlock_WebScraping.Helpers;
using WorkerService.T_Unlock_WebScraping.Interfaces;

namespace WorkerService.T_Unlock_WebScraping.Services
{
    public class ProcessPhoneTUnlock : IProcessPhoneTUnlock
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ProcessPhoneTUnlock(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
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

        private async Task<UnlockabledPhoneResponse> CreateUnlockabledPhone(string modelName, string modelNumber, int brandId)
        {
            var unlockableCreateDto = new UnlockabledPhoneCreateRequest
            {
                BrandId = brandId,
                ModelName = modelName,
                ModelNumber = modelNumber
            };

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                return await unlockabledPhoneService.CreateAsync<UnlockabledPhoneCreateRequest, UnlockabledPhoneResponse>(unlockableCreateDto);
            }
        }

        private async Task ProcessSinglePhoneAsync(HtmlNode htmlNode, Brand brand)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var unlockabledPhoneService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhoneService>();
                var unlockabledPhonePhoneUnlockToolService = scope.ServiceProvider.GetRequiredService<IUnlockabledPhonePhoneUnlockToolService>();
                var unlockabledPhonePhoneCarrierTUnlock = scope.ServiceProvider.GetRequiredService<IUnlockabledPhonePhoneCarrierTUnlock>();

                var phoneDetailsTUnlock = HtmlNodeMapper.MapHtmlNodeToPhoneDetails(htmlNode);

                var possibleUnlockedPhone = await unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone => unlockedPhone.ModelNumber.Equals(phoneDetailsTUnlock.ModelNumber));

                if (possibleUnlockedPhone == null)
                {
                    var newUnlockedPhone = await CreateUnlockabledPhone(phoneDetailsTUnlock.ModelName, phoneDetailsTUnlock.ModelNumber, (int)brand);
                    await unlockabledPhonePhoneUnlockToolService.CreateAsync(newUnlockedPhone.Id, (int)UnlockTool.TUnlock);
                    await unlockabledPhonePhoneCarrierTUnlock.CreateMassive(newUnlockedPhone.Id, phoneDetailsTUnlock.Carriers);
                }
                else
                {
                    await unlockabledPhonePhoneUnlockToolService.CreateIfNotExists(possibleUnlockedPhone.Id, (int)UnlockTool.TUnlock);
                    await unlockabledPhonePhoneCarrierTUnlock.CreateMassive(possibleUnlockedPhone.Id, phoneDetailsTUnlock.Carriers);
                }
            }
        }
    }
}