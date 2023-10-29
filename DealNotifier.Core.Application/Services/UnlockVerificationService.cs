using Catalog.Application.Constants;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;
using System.Text.RegularExpressions;

namespace Catalog.Application.Services
{
    public class UnlockVerificationService : IUnlockVerificationService
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

        public async Task<bool> CanBeUnlockedBasedOnModelNameAsync(ItemCreateRequest itemCreate)
        {
            var possibleModelName = Regex.Match(itemCreate.Name, RegExPattern.ModelName, RegexOptions.IgnoreCase).Value;

            if (possibleModelName != null)
            {
                var possibleUnlockedPhone = await _unlockabledPhoneRepository.FirstOrDefaultAsync(item => item.ModelName!.Contains(possibleModelName));

                if (possibleUnlockedPhone != null)
                {
                    var carrierId = TryGetPhoneCarrierId(itemCreate.Name);
                    if (carrierId != null)
                    {
                        return await ExistsUnlockabledPhonePhoneCarrier(possibleUnlockedPhone.Id, (int)carrierId);
                    }
                }
            }

            return false;
        }

        public async Task<bool> CanBeUnlockedBasedOnUnlockabledPhoneIdAsync(ItemCreateRequest itemCreate)
        {
            if (itemCreate.UnlockabledPhoneId != null)
            {
                var carrierId = TryGetPhoneCarrierId(itemCreate.Name);
                if (carrierId != null)
                {
                    return await ExistsUnlockabledPhonePhoneCarrier((int)itemCreate.UnlockabledPhoneId, (int)carrierId);
                }
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

        private async Task<bool> ExistsUnlockabledPhonePhoneCarrier(int unlockabledPhoneId, int carrierId)
        {
            var unlockabledPhonePhoneCarrier = new UnlockabledPhonePhoneCarrier
            {
                PhoneCarrierId = carrierId,
                UnlockabledPhoneId = unlockabledPhoneId
            };

            return await _unlockabledPhonePhoneCarrierRepository.ExistsAsync(unlockabledPhonePhoneCarrier);
        }
    }

}