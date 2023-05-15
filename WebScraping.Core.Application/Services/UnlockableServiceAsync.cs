using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Services
{
    public class UnlockableServiceAsync: GenericServiceAsync<Unlockable>, IUnlockableServiceAsync
    {
        #region Private Variable
        private readonly IUnlockableRepositoryAsync _repository;
        #endregion Private Variable

        public UnlockableServiceAsync(IUnlockableRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
            _repository = repository;
        }

        public async Task<Unlockable?> GetByModelNumberAsync(string modelNumber)
        {
            return await _repository.GetByModelNumberAsync(modelNumber);
        }
    }
}
