using AutoMapper;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.Services;
using Catalog.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace Catalog.Application.Services.Items
{
    public class ItemTypeService : GenericService<ItemType>, IItemTypeService
    {
        public ItemTypeService(IItemTypeRepository repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache) : base(repository, mapper, httpContext, cache)
        {
        }
    }
}