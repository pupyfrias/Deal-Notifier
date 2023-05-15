using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using WebScraping.Core.Application.Contracts.Repositories;
using WebScraping.Core.Application.Contracts.Services;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Services
{
    public class PhoneCarrierServiceAsync: GenericServiceAsync<PhoneCarrier>, IPhoneCarrierServiceAsync
    {
        public PhoneCarrierServiceAsync(IPhoneCarrierRepositoryAsync repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
            
        }
    }
}
