using AutoMapper;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Services
{
    public class PhoneCarrierServiceAsync : GenericServiceAsync<PhoneCarrier>, IPhoneCarrierServiceAsync
    {
        public PhoneCarrierServiceAsync(IPhoneCarrierRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {

        }
    }
}
