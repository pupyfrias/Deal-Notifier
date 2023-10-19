using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using WorkerService.T_Unlock_WebScraping.Interfaces;
using ILogger = Serilog.ILogger;

namespace WorkerService.T_Unlock_WebScraping.Services
{
    public class UnlockabledPhonePhoneCarrierTUnlock : IUnlockabledPhonePhoneCarrierTUnlock
    {
        private readonly ILogger _logger;
        private readonly IPhoneCarrierService _phoneCarrierService;
        private readonly IUnlockabledPhonePhoneCarrierService _unlockabledPhonePhoneCarrierService;
        private HashSet<PhoneCarrierDto> _phoneCarrierList;

        public UnlockabledPhonePhoneCarrierTUnlock(
            IPhoneCarrierService phoneCarrierService,
            IUnlockabledPhonePhoneCarrierService unlockabledPhonePhoneCarrierService,
            ILogger logger
            )
        {
            _phoneCarrierService = phoneCarrierService;
            _unlockabledPhonePhoneCarrierService = unlockabledPhonePhoneCarrierService;
            _logger = logger;
        }

        public async Task CreateMassive(int unlockedPhoneId, string carriers)
        {
            var phoneCarrierList = await FilterPhoneCarrierAsync(carriers);

            if (phoneCarrierList.Count > 0)
            {
                foreach (int phoneCarrierId in phoneCarrierList)
                {
                    await _unlockabledPhonePhoneCarrierService.CreateIfNotExists(unlockedPhoneId, phoneCarrierId);
                }
            }
            else
            {
                _logger.Warning($"Carriers [{carriers}] no exists on DataBase");
            }
        }

        private async Task<HashSet<int>> FilterPhoneCarrierAsync(string carriers)
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

                return carriers.Contains(firstPart, StringComparison.OrdinalIgnoreCase) ||
                        carriers.Contains(secondPart, StringComparison.OrdinalIgnoreCase) ||
                        carriers.Contains(phoneCarrier.ShortName, StringComparison.OrdinalIgnoreCase);
            })
                    .Select(phoneCarrier => phoneCarrier.Id)
                    .ToHashSet();
        }
    }
}