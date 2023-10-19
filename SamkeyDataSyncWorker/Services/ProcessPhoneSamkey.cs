using DealNotifier.Core.Application.Enums;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using SamkeyDataSyncWorker.Interfaces;
using SamkeyDataSyncWorker.ViewModels;

namespace SamkeyDataSyncWorker.Services
{
    public class ProcessPhoneSamkey : IProcessPhoneSamkey
    {
        private readonly IFetchSamkey _fetchSamkey;
        private readonly IUnlockabledPhonePhoneCarrierSamkey _unlockabledPhonePhoneCarrierSamkey;
        private readonly IUnlockabledPhonePhoneUnlockToolService _unlockabledPhonePhoneUnlockToolService;
        private readonly IUnlockabledPhoneService _unlockabledPhoneService;
        public ProcessPhoneSamkey(
            IFetchSamkey fetchSamkey,
            IUnlockabledPhoneService unlockabledPhoneService,
            IUnlockabledPhonePhoneCarrierSamkey unlockabledPhonePhoneCarrierSamkey,
            IUnlockabledPhonePhoneUnlockToolService unlockabledPhonePhoneUnlockToolService
            )
        {
            _fetchSamkey = fetchSamkey;
            _unlockabledPhonePhoneCarrierSamkey = unlockabledPhonePhoneCarrierSamkey;
            _unlockabledPhonePhoneUnlockToolService = unlockabledPhonePhoneUnlockToolService;
            _unlockabledPhoneService = unlockabledPhoneService;
        }


        public async Task ProcessAsync(List<string> phoneList)
        {
            foreach (var phone in phoneList)
            {
                await ProcessSinglePhone(phone);
            }
        }


        private async Task<UnlockabledPhoneResponse> CreateUnlockabledPhone(string modelName, string modelNumber)
        {
            var unlockableCreateDto = new UnlockabledPhoneCreateRequest
            {
                BrandId = (int)Brand.Samsung,
                ModelName = modelName,
                ModelNumber = modelNumber
            };

            return await _unlockabledPhoneService.CreateAsync<UnlockabledPhoneCreateRequest, UnlockabledPhoneResponse>(unlockableCreateDto);
        }

        private async Task ProcessSinglePhone(string phoneModel)
        {
            var splitPhone = SplitPhoneModel(phoneModel);
            if (splitPhone == null) return;

            var (modelNumber, modelName) = splitPhone.Value;

            DetailsPhoneResponse? detailsPhone = await _fetchSamkey.GetDetailsPhoneAsync(modelNumber);

            if (detailsPhone == null) return;

            var possibleUnlockedPhone = await _unlockabledPhoneService.FirstOrDefaultAsync(unlockedPhone => unlockedPhone.ModelNumber.Equals(modelNumber));

            if (possibleUnlockedPhone == null)
            {
                var newUnlockedPhone = await CreateUnlockabledPhone(modelName, modelNumber);
                await _unlockabledPhonePhoneUnlockToolService.CreateAsync(newUnlockedPhone.Id, (int)UnlockTool.SamKey);
                await _unlockabledPhonePhoneCarrierSamkey.CreateMassive(newUnlockedPhone.Id, detailsPhone.SupportCarriers);
            }
            else
            {
                await _unlockabledPhonePhoneUnlockToolService.CreateIfNotExists(possibleUnlockedPhone.Id, (int)UnlockTool.SamKey);
                await _unlockabledPhonePhoneCarrierSamkey.CreateMassive(possibleUnlockedPhone.Id, detailsPhone.SupportCarriers);
            }
        }

        private (string, string)? SplitPhoneModel(string phoneModel)
        {
            var splitPhone = phoneModel.Split(" | ");
            if (splitPhone.Length < 2) return null;

            return (splitPhone[0], splitPhone[1]);
        }
    }
}
