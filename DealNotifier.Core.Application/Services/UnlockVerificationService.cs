using DealNotifier.Core.Application.Constants;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;
using System.Text.RegularExpressions;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockVerificationService: IUnlockVerificationService
    {
        private readonly IUnlockabledPhoneRepository _unlockabledPhoneRepository;
        private readonly IUnlockabledPhonePhoneCarrierRepository _unlockabledPhonePhoneCarrierRepository;
        private readonly ICacheDataService _cacheDataService;

        public UnlockVerificationService(
            IUnlockabledPhoneRepository unlockabledPhoneRepository,
            IUnlockabledPhonePhoneCarrierRepository unlockabledPhonePhoneCarrierRepository,
            ICacheDataService cacheDataService)
        {
            _unlockabledPhoneRepository = unlockabledPhoneRepository;
            _unlockabledPhonePhoneCarrierRepository = unlockabledPhonePhoneCarrierRepository;
            _cacheDataService = cacheDataService;
        }

        public async Task<bool> CanBeUnlockedBasedOnModelNameAsync(ItemDto itemCreate)
        {

            var possibleModelName = Regex.Match(itemCreate.Title, RegExPattern.ModelName, RegexOptions.IgnoreCase).Value;

            if (possibleModelName != null)
            {
                var possibleUnlockedPhones = _unlockabledPhoneRepository.Where(item => item.ModelName.Contains(possibleModelName, StringComparison.OrdinalIgnoreCase)).ToList();

                if (possibleUnlockedPhones.Any())
                {
                    var carrierId = TryGetPhoneCarrierId(itemCreate.Title) ?? (int)Enums.PhoneCarrier.UNK;
                    return ExistsUnlockabledPhonePhoneCarrier(possibleUnlockedPhones, carrierId);
                    
                }
            }

            return false;
        }

        public bool CanBeUnlockedBasedOnUnlockabledPhoneId(ItemDto itemCreate)
        {
            if (itemCreate.UnlockabledPhoneId != null)
            {
                var carrierId = TryGetPhoneCarrierId(itemCreate.Title) ?? (int) Enums.PhoneCarrier.UNK;
                return ExistsUnlockabledPhonePhoneCarrier((int)itemCreate.UnlockabledPhoneId, carrierId);
            }

            return false;
        }

        private int? TryGetPhoneCarrierId(string name)
        {
            var matchedPhoneCarrier = _cacheDataService.PhoneCarrierList.FirstOrDefault(carrier =>
            {
                var PhoneCarrierNameAndShortNameList = carrier.Name.Split('|').ToList();

                return PhoneCarrierNameAndShortNameList.Exists(element => name.Contains(element,
                    StringComparison.OrdinalIgnoreCase));
            });

            return matchedPhoneCarrier?.Id ?? null;
        }

        private bool ExistsUnlockabledPhonePhoneCarrier(int unlockabledPhoneId, int carrierId)
        {
            return  _unlockabledPhonePhoneCarrierRepository.Any(x=> x.UnlockabledPhoneId.Equals(unlockabledPhoneId) 
            && (x.PhoneCarrierId.Equals(carrierId) || x.PhoneCarrierId.Equals((int) Enums.PhoneCarrier.ALL)));
        }

        private bool ExistsUnlockabledPhonePhoneCarrier(List<UnlockabledPhone> unlockabledPhones, int carrierId)
        {
            var unlockabledPhoneIds = unlockabledPhones.Select(x => x.Id);

            return  _unlockabledPhonePhoneCarrierRepository.Any(x=> unlockabledPhoneIds.Contains(x.UnlockabledPhoneId)
            && (x.PhoneCarrierId.Equals(carrierId) || x.PhoneCarrierId.Equals((int) Enums.PhoneCarrier.ALL))
            );
        }
    }

}