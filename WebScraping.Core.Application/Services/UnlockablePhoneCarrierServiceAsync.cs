using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Services
{
    public class UnlockablePhoneCarrierServiceAsync: IUnlockablePhoneCarrierServiceAsync
    {
        #region Private Variable
        private readonly IUnlockablePhoneCarrierRepositoryAsync _repository;
        #endregion Private Variable

        public UnlockablePhoneCarrierServiceAsync(IUnlockablePhoneCarrierRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<UnlockablePhonePhoneCarrier> CreateAsync(UnlockablePhonePhoneCarrier entity)
        {
           return await _repository.CreateAsync(entity);
        }

        public async Task<bool> ExistsAsync(UnlockablePhonePhoneCarrier entity)
        {
            return await _repository.ExistsAsync(entity);
        }
    }
}
