using AutoMapper;
using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Interfaces;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Application.Interfaces.Services;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Application.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Web;

namespace DealNotifier.Core.Application.Services
{
    public class GenericService<TEntity, TKey> : IGenericService<TEntity, TKey>
    {
        #region Private Variable

        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<TEntity, TKey> _genericRepository;

        #endregion Private Variable

        #region Constructor

        public GenericService(IGenericRepository<TEntity, TKey> genericRepository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache)
        {
            _genericRepository = genericRepository;
            _mapper = mapper;
            _httpContext = httpContext;
            _cache = cache;
        }

        #endregion Constructor

        #region Public Methods

        #region Async
        public virtual async Task<TDestination> CreateAsync<TSource, TDestination>(TSource source)
        {
            var entity = _mapper.Map<TEntity>(source);
            var createdEntity = await _genericRepository.CreateAsync(entity);
            var mappedEntity = _mapper.Map<TDestination>(createdEntity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
            return mappedEntity;
        }

        public virtual async Task DeleteAsync(TKey id)
        {
            var entity = await GetByIdAsync(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            await _genericRepository.DeleteAsync(entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
        }

        public virtual async Task<bool> ExistsAsync(TKey id)
        {
            var entity = await GetByIdProjectedAsync<TEntity>(id);
            return entity != null;
        }

        public virtual async Task<IEnumerable<TDestination>> GetAllAsync<TDestination>()
        {
            return await _genericRepository.GetAllProjectedAsync<TDestination>();
        }

        public virtual async Task<PagedCollection<TDestination>> GetAllWithPaginationAsync<TDestination, TSpecification>(IPaginationBase request)
        where TSpecification : ISpecification<TEntity>
        {
            var type = typeof(TSpecification);
            object[] constructorArguments = new object[] { request };
            var spec = (TSpecification)Activator.CreateInstance(type, constructorArguments)!;

            if (spec.Skip % spec.Take != 0)
            {
                throw new BadRequestException($"The 'offset' value ({spec.Skip}) must be either zero or a multiple of the 'limit' value({spec.Take}).");
            }

            var total = await _genericRepository.GetTotalCountAsync(spec.Criteria);

            if (total < spec.Skip)
            {
                throw new BadRequestException($"The 'offset' value must be either zero or minimum to 'total' value ({total}) and multiple of the 'limit' value({spec.Take}).");
            }

            var items = await _genericRepository.GetAllProjectedAsync<TDestination>(spec);
            var href = _httpContext?.HttpContext?.Request.GetEncodedUrl()!;
            var next = GetNextURL(href, spec.Take, spec.Skip, total);
            var prev = GetPrevURL(href, spec.Take, spec.Skip);

            return new PagedCollection<TDestination>
            {
                Href = href,
                Items = items,
                Limit = spec.Take,
                Next = next,
                Offset = spec.Skip,
                Prev = prev,
                Total = total
            };
        }

        public async Task<(IEnumerable<TDestination>, string)> GetAllWithETagAsync<TDestination>()
        {
            string cacheKey = $"IEnumerable_{typeof(TEntity).FullName}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TDestination> mappedEntities))
            {
                mappedEntities = await _genericRepository.GetAllProjectedAsync<TDestination>();

                _cache.Set(cacheKey, mappedEntities, TimeSpan.FromHours(24));
            }

            var eTag = CacheUtility.GenerateETag(mappedEntities);
            return (mappedEntities, eTag);
        }

        public virtual async Task<TDestination> GetByIdProjectedAsync<TDestination>(TKey id)
        {
            var mappedEntity = await _genericRepository.GetByIdProjectedAsync<TDestination>(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id != null ? id : "No Key Provided");
            }

            return mappedEntity;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            var mappedEntity = await _genericRepository.GetByIdAsync(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id != null ? id : "No Key Provided");
            }

            return mappedEntity;
        }

        public virtual async Task UpdateAsync<TSource>(TKey id, TSource source) where TSource : IHasId<TKey>
        {

            if (!id!.Equals(source.Id))
            {
                throw new BadRequestException("The source ID does not match the provided ID");
            }


            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            _mapper.Map(source, entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
            await _genericRepository.UpdateAsync(entity);
        }
        #endregion Async

        #region Sync
        public virtual TDestination Create<TSource, TDestination>(TSource source)
        {
            var entity = _mapper.Map<TEntity>(source);
            var createdEntity = _genericRepository.Create(entity);
            var mappedEntity = _mapper.Map<TDestination>(createdEntity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
            return mappedEntity;
        }

        public virtual void Delete(TKey id)
        {
            var entity = GetById(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            _genericRepository.Delete(entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
        }

        public virtual bool Exists(TKey id)
        {
            var entity = GetByIdProjected<TEntity>(id);
            return entity != null;
        }

        public virtual IEnumerable<TDestination> GetAll<TDestination>()
        {
            return _genericRepository.GetAllProjected<TDestination>();
        }

        public virtual PagedCollection<TDestination> GetAllWithPagination<TDestination, TSpecification>(IPaginationBase request)
                where TSpecification : ISpecification<TEntity>
        {
            var type = typeof(TSpecification);
            object[] constructorArguments = new object[] { request };
            var spec = (TSpecification)Activator.CreateInstance(type, constructorArguments)!;

            if (spec.Skip % spec.Take != 0)
            {
                throw new BadRequestException($"The 'offset' value ({spec.Skip}) must be either zero or a multiple of the 'limit' value({spec.Take}).");
            }

            var total = _genericRepository.GetTotalCount(spec.Criteria);

            if (total < spec.Skip)
            {
                throw new BadRequestException($"The 'offset' value must be either zero or minimum to 'total' value ({total}) and multiple of the 'limit' value({spec.Take}).");
            }

            var items = _genericRepository.GetAllProjected<TDestination>(spec);
            var href = _httpContext?.HttpContext?.Request.GetEncodedUrl()!;
            var next = GetNextURL(href, spec.Take, spec.Skip, total);
            var prev = GetPrevURL(href, spec.Take, spec.Skip);

            return new PagedCollection<TDestination>
            {
                Href = href,
                Items = items,
                Limit = spec.Take,
                Next = next,
                Offset = spec.Skip,
                Prev = prev,
                Total = total
            };
        }

        public (IEnumerable<TDestination>, string) GetAllWithETag<TDestination>()
        {
            string cacheKey = $"IEnumerable_{typeof(TEntity).FullName}";

            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TDestination> mappedEntities))
            {
                mappedEntities = _genericRepository.GetAllProjected<TDestination>();

                _cache.Set(cacheKey, mappedEntities, TimeSpan.FromHours(24));
            }

            var eTag = CacheUtility.GenerateETag(mappedEntities);
            return (mappedEntities, eTag);
        }

        public virtual TDestination GetByIdProjected<TDestination>(TKey id)
        {
            var mappedEntity = _genericRepository.GetByIdProjected<TDestination>(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id != null ? id : "No Key Provided");
            }

            return mappedEntity;
        }

        public virtual TEntity GetById(TKey id)
        {
            var mappedEntity = _genericRepository.GetById(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id != null ? id : "No Key Provided");
            }

            return mappedEntity;
        }

        public virtual void Update<TSource>(TKey id, TSource source) where TSource : IHasId<TKey>
        {

            if (!id!.Equals(source.Id))
            {
                throw new BadRequestException("The source ID does not match the provided ID");
            }


            var entity = GetById(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            _mapper.Map(source, entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
            _genericRepository.Update(entity);
        }
        #endregion Sync



        #endregion Public Methods

        #region Private Methods

        private string? GetNextURL(string url, int limit, int offset, int total)
        {
            var newOffSet = limit + offset;
            if (newOffSet >= total)
            {
                return null;
            }

            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["offset"] = $"{newOffSet}";
            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        private string? GetPrevURL(string url, int limit, int offset)
        {
            var oldOffSet = offset - limit;
            if (oldOffSet < 0)
            {
                return null;
            }

            var uriBuilder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            if (oldOffSet == 0)
            {
                query.Remove("offset");
            }
            else
            {
                query["offset"] = $"{oldOffSet}";
            }

            uriBuilder.Query = query.ToString();
            return uriBuilder.ToString();
        }

        #endregion Private Methods
    }
}