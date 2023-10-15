using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockabledPhoneService : GenericService<UnlockabledPhone>, IUnlockabledPhoneService
    {

        private readonly IUnlockabledPhoneRepository _repository;

        public UnlockabledPhoneService(IUnlockabledPhoneRepository repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
            _repository = repository;
        }

        public async Task<bool> ExistsAsync(string modelName)
        {
            return await _repository.ExistsAsync(modelName);
        }
    }
}