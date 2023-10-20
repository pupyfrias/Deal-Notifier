using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using SamkeyDataSyncWorker.Helpers;
using SamkeyDataSyncWorker.Interfaces;
using SamkeyDataSyncWorker.ViewModels;
using Brand = DealNotifier.Core.Application.Enums.Brand;
using ILogger = Serilog.ILogger;

namespace SamkeyDataSyncWorker.Services
{
    public class ProcessPhoneSamkey : IProcessPhoneSamkey
    {
        private readonly IFetchSamkey _fetchSamkey;
        private readonly IUnlockabledPhoneService _unlockabledPhoneService;
        private readonly ILogger _logger;
        public ProcessPhoneSamkey(
            IFetchSamkey fetchSamkey,
            IUnlockabledPhoneService unlockabledPhoneService,
            ILogger logger
            )
        {
            _fetchSamkey = fetchSamkey;
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


                PhoneDetailsResponse? phoneDetails = await _fetchSamkey.GetPhoneDetailsAsync(unlockedPhoneDetail.ModelNumber);

                if (phoneDetails == null) return;

                var possibleUnlockedPhone = await _unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone =>
                unlockedPhone.ModelNumber.Equals(unlockedPhoneDetail.ModelNumber));


                if (possibleUnlockedPhone == null)
                {
                    await _unlockabledPhoneService.HandleNewUnlockedPhoneAsync(unlockedPhoneDetail, Brand.Samsung,
                        UnlockTool.SamKey);
                }
                else
                {
                    await _unlockabledPhoneService.HandleExistingUnlockedPhoneAsync(possibleUnlockedPhone,
                        phoneDetails.SupportCarriers, UnlockTool.SamKey);
                }
            }
            catch(Exception ex)
            {

                _logger.Warning($"Exception when processing {phoneModel}. Message: {ex.Message} InnerException: {ex.InnerException?.Message}");
            }
           
        }
    }
}
