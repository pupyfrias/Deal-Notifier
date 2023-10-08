using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhonePhoneCarrierServiceAsync : IUnlockabledPhonePhoneCarrierServiceAsync
    {
        #region Private Variable

        private readonly IUnlockabledPhonePhoneCarrierRepositoryAsync _repository;

        #endregion Private Variable

        public UnlockabledPhonePhoneCarrierServiceAsync(IUnlockabledPhonePhoneCarrierRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity)
        {
            return await _repository.CreateAsync(entity);
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

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity)
        {
            return await _repository.ExistsAsync(entity);
        }
    }
}