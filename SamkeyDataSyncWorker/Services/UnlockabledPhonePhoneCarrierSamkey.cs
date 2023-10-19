using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using SamkeyDataSyncWorker.Interfaces;
using ILogger = Serilog.ILogger;

namespace SamkeyDataSyncWorker.Services
{
    public class UnlockabledPhonePhoneCarrierSamkey : IUnlockabledPhonePhoneCarrierSamkey
    {
        private readonly ILogger _logger;
        private readonly IPhoneCarrierService _phoneCarrierService;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;
        private HashSet<PhoneCarrierDto> _phoneCarrierList;

        public UnlockabledPhonePhoneCarrierSamkey(
            IPhoneCarrierService phoneCarrierService,
            IUnlockabledPhonePhoneCarrierService unlockabledPhonePhoneCarrierService,
            ILogger logger
            )
        {
            _phoneCarrierService = phoneCarrierService;
            _unlockabledPhonePhoneCarrierService = unlockabledPhonePhoneCarrierService;
            _logger = logger;


        }

        public async Task CreateMassive(int unlockedPhoneId, string supportCarriers)
        {
            var phoneCarrierList = await FilterPhoneCarrierAsync(supportCarriers);

            if (phoneCarrierList.Count > 0)
            {
                foreach (int phoneCarrierId in phoneCarrierList)
                {
                    await _unlockabledPhonePhoneCarrierService.CreateIfNotExists(unlockedPhoneId, phoneCarrierId);
                }
            }
            else
            {
                _logger.Warning($"Carriers [{supportCarriers}] no exists on DataBase");
            }
        }

        private async Task<HashSet<int>> FilterPhoneCarrierAsync(string supportCarriers)
        {
            if (_phoneCarrierList == null)
            {
                var phoneCarriers = await _phoneCarrierService.GetAllAsync<PhoneCarrierDto>();
                _phoneCarrierList = phoneCarriers.ToHashSet();
            }

            return _phoneCarrierList.Where(phoneCarrier =>
            {
                var nameParts = phoneCarrier.Name.Split("|");
                var firstPart = nameParts.Length > 0 ? nameParts[0] : "EMPTY";
                var secondPart = nameParts.Length > 1 ? nameParts[1] : "EMPTY";

                return supportCarriers.Contains(firstPart, StringComparison.OrdinalIgnoreCase) ||
                        supportCarriers.Contains(secondPart, StringComparison.OrdinalIgnoreCase) ||
                        supportCarriers.Contains(phoneCarrier.ShortName, StringComparison.OrdinalIgnoreCase);
            })
                    .Select(phoneCarrier => phoneCarrier.Id)
                    .ToHashSet();
        }

    }
}
