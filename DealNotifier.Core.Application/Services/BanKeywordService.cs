﻿using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class BanKeywordService : ServiceBase<BanKeyword>, IBanKeywordService
    {
        public BanKeywordService(IBanKeywordRepository repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
        }
    }
}