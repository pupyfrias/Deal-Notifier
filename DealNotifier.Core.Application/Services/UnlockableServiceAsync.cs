using AutoMapper;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockableServiceAsync : GenericServiceAsync<UnlockablePhone>, IUnlockableServiceAsync
    {
        #region Private Variable
        private readonly IUnlockableRepositoryAsync _repository;
        #endregion Private Variable

        public UnlockableServiceAsync(IUnlockableRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
            _repository = repository;
        }

        public async Task<UnlockablePhone?> GetByModelNumberAsync(string modelNumber)
        {
            return await _repository.GetByModelNumberAsync(modelNumber);
        }
    }
}
