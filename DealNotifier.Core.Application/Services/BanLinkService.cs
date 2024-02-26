using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class BanLinkService : ServiceBase<BanLink>, IBanLinkService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IBanLinkRepository _banRepository;

        public BanLinkService(
            IBanLinkRepository repository, 
            IMapper mapper,
            IHttpContextAccessor httpContext, 
            IMemoryCache cache,
            IItemRepository itemRepository,
            IBanLinkRepository banRepository
            
            ) : base(repository, mapper, httpContext, cache)
        {
            _itemRepository = itemRepository;
            _banRepository = banRepository;

        }


        public async Task CreateRangeAsync(IEnumerable<int> ids)
        {
           var banLinks =  _itemRepository.Where(item => ids.Contains(item.Id))
                                          .Select(item =>  new BanLink { Link = item.Link })
                                          .ToList();

           await _banRepository.CreateRangeAsync(banLinks);
           
        }   
    }
}