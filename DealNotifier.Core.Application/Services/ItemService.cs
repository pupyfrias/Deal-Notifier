using AutoMapper;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class ItemService : GenericService<Item, Guid>, IItemService
    {
        #region Constructor

        public ItemService(IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache, IItemRepository itemRepository) : base(itemRepository, mapper, httpContext, cache)
        {

        }

        #endregion Constructor

    }
}