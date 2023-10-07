using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WebApi.Controllers.V1;

namespace DealNotifier.Core.Application.Services
{
    public class OnlineStoreServiceAsync : GenericServiceAsync<OnlineStore, int>, IOnlineStoreServiceAsync
    {
        public OnlineStoreServiceAsync(IOnlineStoreRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
        }
    }
}