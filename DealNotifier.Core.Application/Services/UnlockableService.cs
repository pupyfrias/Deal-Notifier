using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockableService : GenericService<UnlockabledPhone, int>, IUnlockableService
    {
        #region Private Variable

        private readonly IUnlockableRepository _repository;

        #endregion Private Variable

        public UnlockableService(IUnlockableRepository repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
            _repository = repository;
        }

        public async Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber)
        {
            return await _repository.GetByModelNumberAsync(modelNumber);
        }
    }
}