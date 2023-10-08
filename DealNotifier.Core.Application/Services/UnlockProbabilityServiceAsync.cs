using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class UnlockProbabilityServiceAsync : GenericServiceAsync<UnlockProbability, int>, IUnlockProbabilityServiceAsync
    {
        public UnlockProbabilityServiceAsync(IUnlockProbabilityRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
        }
    }
}