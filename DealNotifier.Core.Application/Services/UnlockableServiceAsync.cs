using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockableServiceAsync : GenericServiceAsync<UnlockabledPhone, int>, IUnlockableServiceAsync
    {
        #region Private Variable

        private readonly IUnlockableRepositoryAsync _repository;

        #endregion Private Variable

        public UnlockableServiceAsync(IUnlockableRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
            _repository = repository;
        }

        public async Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber)
        {
            return await _repository.GetByModelNumberAsync(modelNumber);
        }
    }
}