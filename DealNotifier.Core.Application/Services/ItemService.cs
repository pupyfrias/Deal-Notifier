using AutoMapper;
using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Interfaces;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace DealNotifier.Core.Application.Services
{
    public class ItemService : GenericService<Item>, IItemService
    {

        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;

        #region Constructor

        public ItemService(IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache memmoryCache, IItemRepository itemRepository) : base(itemRepository, mapper, httpContext, memmoryCache)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
            _memoryCache = memmoryCache;
        }

        public async Task DeleteAsync(Guid id)
        {

            var entity = await _itemRepository.GetByPublicIdAsync(id);

            if (entity is null)
            {
                throw new NotFoundException("Item", id);
            }

            await _itemRepository.DeleteAsync(entity);
            CacheUtility.InvalidateCache<Item>(_memoryCache);
        
    }

        public async Task<TDestination?> GetByPublicIdProjectedAsync<TDestination>(Guid id)
        {
            var mappedEntity = await _itemRepository.GetByPublicIdProjected<TDestination>(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException("Item", id );
            }

            return mappedEntity;
        }

        #endregion Constructor


        public async Task UpdateAsync<TSource>(Guid id, TSource source) where TSource : IHasId<Guid>
        {
            if (!id!.Equals(source.Id))
            {
                throw new BadRequestException("The source ID does not match the provided ID");
            }


            var entity = await _itemRepository.GetByPublicIdAsync(id);

            if (entity == null)
            {
                throw new NotFoundException("Item", id);
            }

            _mapper.Map(source, entity);
            await _itemRepository.UpdateAsync(entity);
            CacheUtility.InvalidateCache<Item>(_memoryCache);
        }

    }
}