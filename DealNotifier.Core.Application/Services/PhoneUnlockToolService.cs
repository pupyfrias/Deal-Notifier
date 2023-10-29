﻿using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Catalog.Application.Services
{
    public class PhoneUnlockToolService : GenericService<PhoneUnlockTool>, IPhoneUnlockToolService
    {
        public PhoneUnlockToolService(IPhoneUnlockToolRepository repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
        }
    }
}