using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using Serilog;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhonePhoneCarrierService : IUnlockabledPhonePhoneCarrierService
    {
        #region Private Variable

        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IPhoneCarrierService _phoneCarrierService;
        private readonly IUnlockabledPhonePhoneCarrierRepository _unlockabledPhonePhoneCarrierRepository;
        private HashSet<PhoneCarrierDto> _phoneCarrierList;

        #endregion Private Variable

        public UnlockabledPhonePhoneCarrierService(
            IUnlockabledPhonePhoneCarrierRepository unlockabledPhonePhoneCarrierRepository,
            IMapper mapper,
            ILogger logger,
            IPhoneCarrierService phoneCarrierService
            )
        {
            _unlockabledPhonePhoneCarrierRepository = unlockabledPhonePhoneCarrierRepository;
            _mapper = mapper;
            _logger = logger;
            _phoneCarrierService = phoneCarrierService; 
        }

        public async Task CreateMassiveAsync(int unlockedPhoneId, string carriers)
        {
            var phoneCarrierList = await FilterPhoneCarrierAsync(carriers);
            if (phoneCarrierList.Count > 0)
            {
                foreach (int phoneCarrierId in phoneCarrierList)
                {
                    await CreateIfNotExists(unlockedPhoneId, phoneCarrierId);
                }
            }
            else
            {
                _logger.Warning($"Carriers [{carriers}] no exists on DataBase");
            }
        }

        private async Task CreateAsync(UnlockabledPhonePhoneCarrierCreate entity)
        {
            var mappedEntity = _mapper.Map<UnlockabledPhonePhoneCarrier>(entity);
            await _unlockabledPhonePhoneCarrierRepository.CreateAsync(mappedEntity);
        }
        private async Task CreateIfNotExists(int unlockabledPhoneId, int phoneCarrierId)
        {
            var unlockablePhoneCarrierCreate = new UnlockabledPhonePhoneCarrierCreate
            {
                UnlockabledPhoneId = unlockabledPhoneId,
                PhoneCarrierId = phoneCarrierId
            };

            var existsUnlockabledPhonePhoneCarrier = await ExistsAsync(unlockablePhoneCarrierCreate);

            if (!existsUnlockabledPhonePhoneCarrier)
            {
     
                await CreateAsync(unlockablePhoneCarrierCreate);
            }
        }

        private async Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrierCreate entity)
        {
            var mappedEntity = _mapper.Map<UnlockabledPhonePhoneCarrier>(entity);
            return await _unlockabledPhonePhoneCarrierRepository.ExistsAsync(mappedEntity);
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