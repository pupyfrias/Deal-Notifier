using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhonePhoneCarrierService : IUnlockabledPhonePhoneCarrierService
    {
        #region Private Variable

        private readonly IUnlockabledPhonePhoneCarrierRepository _repository;
        private readonly IMapper _mapper;

        #endregion Private Variable

        public UnlockabledPhonePhoneCarrierService(
            IUnlockabledPhonePhoneCarrierRepository repository, 
            IMapper mapper
            )
        {
            _repository = repository;
            _mapper = mapper;
        }



        #region Async
        public async Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrierCreateRequest entity)
        {
            var mappedEntity = _mapper.Map<UnlockabledPhonePhoneCarrier>(entity);
            return await _repository.CreateAsync(mappedEntity);
        }

        public async Task CreateRangeAsync(int phoneCarrierId, PhoneCarrierUnlockabledPhoneCreateRequest request)
        {
            var unlockedPhoneList = request.UnlockabledPhones.Split(',').Select(s => int.Parse(s));
            var phoneUnlockToolUnlockablePhoneList = unlockedPhoneList.Select(unlockedPhoneId =>
            {
                return new UnlockabledPhonePhoneCarrier
                {
                    UnlockabledPhoneId = unlockedPhoneId,
                    PhoneCarrierId = phoneCarrierId
                };

            });


            await _repository.CreateRangeAsync(phoneUnlockToolUnlockablePhoneList);
        }

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrierDto entity)
        {
            var mappedEntity = _mapper.Map<UnlockabledPhonePhoneCarrier>(entity);
            return await _repository.ExistsAsync(mappedEntity);
        }
        #endregion Async

        #region Sync
        public bool Exists(UnlockabledPhonePhoneCarrierDto entity)
        {
            var mappedEntity = _mapper.Map<UnlockabledPhonePhoneCarrier>(entity);
            return _repository.Exists(mappedEntity);
        }
        #endregion Sync




    }
}