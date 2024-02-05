using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Helpers;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.Interfaces;
using DealNotifier.Infrastructure.SamkeyDataSyncWorker.ViewModels;
using Brand = DealNotifier.Core.Application.Enums.Brand;
using ILogger = Serilog.ILogger;

namespace DealNotifier.Infrastructure.SamkeyDataSyncWorker.Services
{
    public class SamkeyPhoneProcessService : ISamkeyPhoneProcessService
    {
        private readonly ISamkeyFetchService _samkeyFetchService;
        private readonly IUnlockabledPhoneService _unlockabledPhoneService;
        private readonly ILogger _logger;
        public SamkeyPhoneProcessService(
            ISamkeyFetchService samkeyFetchService,
            IUnlockabledPhoneService unlockabledPhoneService,
            ILogger logger
            )
        {
            _samkeyFetchService = samkeyFetchService;
            _unlockabledPhoneService = unlockabledPhoneService;
            _logger = logger;
        }


        public async Task ProcessAsync(List<string> phoneList)
        {
            foreach (var phone in phoneList)
            {
                await ProcessSinglePhone(phone);
            }
        }


        private async Task ProcessSinglePhone(string phoneModel)
        {
            try
            {
                var unlockedPhoneDetail = HelperSamkey.MapStringToUnlockedPhoneDetails(phoneModel);
                if (unlockedPhoneDetail == null) return;


                PhoneDetailsResponse? phoneDetails = await _samkeyFetchService.GetPhoneDetailsAsync(unlockedPhoneDetail.ModelNumber);

                if (phoneDetails == null) return;

                var possibleUnlockedPhone = await _unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone =>
                unlockedPhone.ModelNumber.Equals(unlockedPhoneDetail.ModelNumber));


                if (possibleUnlockedPhone == null)
                {
                    unlockedPhoneDetail.Carriers = phoneDetails.SupportCarriers;
                    await _unlockabledPhoneService.HandleNewUnlockedPhoneAsync(unlockedPhoneDetail, Brand.Samsung,
                        UnlockTool.SamKey);
                }
                else
                {
                    await _unlockabledPhoneService.HandleExistingUnlockedPhoneAsync(possibleUnlockedPhone,
                        phoneDetails.SupportCarriers, UnlockTool.SamKey);
                }
            }
            catch (Exception ex)
            {

                _logger.Warning($"Exception when processing {phoneModel}. Message: {ex.Message} InnerException: {ex.InnerException?.Message}");
            }

        }
    }
}
