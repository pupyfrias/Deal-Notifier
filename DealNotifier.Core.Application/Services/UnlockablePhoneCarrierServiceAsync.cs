using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockablePhoneCarrierServiceAsync : IUnlockablePhoneCarrierServiceAsync
    {
        #region Private Variable

        private readonly IUnlockabledPhonePhoneCarrierRepositoryAsync _repository;

        #endregion Private Variable

        public UnlockablePhoneCarrierServiceAsync(IUnlockabledPhonePhoneCarrierRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<UnlockabledPhonePhoneCarrier> CreateAsync(UnlockabledPhonePhoneCarrier entity)
        {
            return await _repository.CreateAsync(entity);
        }

        public async Task<bool> ExistsAsync(UnlockabledPhonePhoneCarrier entity)
        {
            return await _repository.ExistsAsync(entity);
        }
    }
}