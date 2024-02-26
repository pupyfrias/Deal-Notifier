using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Interfaces.Services.Items;
using DealNotifier.Core.Application.ViewModels.V1.BanKeyword;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class BanKeywordService : ServiceBase<BanKeyword>, IBanKeywordService
    {
        private readonly IItemService _itemService;
        public BanKeywordService(
            IBanKeywordRepository repository,
            IMapper mapper, IHttpContextAccessor httpContext, 
            IMemoryCache cache,
            IItemService itemService
            ) : base(repository, mapper, httpContext, cache)
        {
            _itemService = itemService;
        }

        public async Task<BanKeywordResponse> BanAndRemoveItemsByKeywordAsync(BanKeywordCreateRequest request)
        {
            await _itemService.DeleteByKeyword(request.Keyword); 
            return await CreateAsync<BanKeywordCreateRequest, BanKeywordResponse>(request); 
        }

    }
}