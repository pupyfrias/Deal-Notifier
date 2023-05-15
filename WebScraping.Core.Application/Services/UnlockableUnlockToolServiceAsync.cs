using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Services
{
    public class UnlockableUnlockToolServiceAsync: IUnlockableUnlockToolServiceAsync
    {
        #region Private Variable
        private readonly IUnlockableUnlockToolRepositoryAsync _repository;
        #endregion Private Variable

        public UnlockableUnlockToolServiceAsync(IUnlockableUnlockToolRepositoryAsync repository)
        {
            _repository = repository;
        }

        public async Task<UnlockableUnlockTool> CreateAsync(UnlockableUnlockTool entity)
        {
           return await _repository.CreateAsync(entity);
        }
    }
}
