using AutoMapper;
using DealNotifier.Core.Application.Contracts;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Exceptions;
using DealNotifier.Core.Application.Models;
using DealNotifier.Core.Application.Utilities;
using DealNotifier.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Web;

namespace DealNotifier.Core.Application.Services
{
    public class GenericServiceAsync<TEntity> : IGenericServiceAsync<TEntity>
    {
        #region Private Variable

        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IGenericRepositoryAsync<TEntity> _repository;

        #endregion Private Variable

        #region Constructor

        public GenericServiceAsync(IGenericRepositoryAsync<TEntity> repository, IMapper mapper, IHttpContextAccessor httpContext, IMemoryCache cache)
        {
            _repository = repository;
            _mapper = mapper;
            _httpContext = httpContext;
            _cache = cache;
        }

        #endregion Constructor

        #region Public Methods


        public virtual async Task<TDestination> CreateAsync<TSource, TDestination>(TSource source)
        {
            var entity = _mapper.Map<TEntity>(source);
            await _repository.CreateAsync(entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
            return _mapper.Map<TDestination>(entity);
        }

        public virtual async Task DeleteAsync<TKey>(TKey id)
        {
            var entity = await GetByIdAsync<TKey, TEntity>(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            await _repository.DeleteAsync(entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
        }

        public virtual async Task<bool> ExistsAsync<TKey>(TKey id)
        {
            var entity = await GetByIdAsync<TKey, TEntity>(id);
            return entity != null;
        }

        public virtual async Task<List<TDestination>> GetAllAsync<TDestination>()
        {
            return await _repository.GetAllAsync<TDestination>();
        }

        public virtual async Task<PagedCollection<TDestination>> GetAllWithPaginationAsync<TDestination, TSpecification>(IRequestBase request)
        where TSpecification : ISpecification<TEntity>
        {


            var type = typeof(TSpecification);
            object[] constructorArguments = new object[] { request };
            var spec = (TSpecification)Activator.CreateInstance(type, constructorArguments)!;


            if (spec.Skip % spec.Take != 0)
            {
                throw new BadRequestException($"The 'offset' value ({spec.Skip}) must be either zero or a multiple of the 'limit' value({spec.Take}).");
            }

            var total = await _repository.GetTotalCountAsync(spec.Criteria);

            if (total < spec.Skip)
            {
                throw new BadRequestException($"The 'offset' value must be either zero or minimum to 'total' value ({total}) and multiple of the 'limit' value({spec.Take}).");
            }

            var items = await _repository.GetAllAsync<TDestination>(spec);
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

        public async Task<(List<TDestination>, string)> GetAllWithETagAsync<TDestination>()
        {
            string cacheKey = $"List_{typeof(TEntity).FullName}";

            if (!_cache.TryGetValue(cacheKey, out List<TDestination> mappedEntities))
            {
                mappedEntities = await _repository.GetAllAsync<TDestination>();

                _cache.Set(cacheKey, mappedEntities, TimeSpan.FromHours(24));
            }

            var eTag = CacheUtility.GenerateETag(mappedEntities);
            return (mappedEntities, eTag);
        }
        public virtual async Task<TDestination> GetByIdAsync<TKey, TDestination>(TKey id)
        {
            var mappedEntity = await _repository.GetByIdAsync<TKey, TDestination>(id);
            if (mappedEntity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id != null ? id : "No Key Provided");
            }

            return mappedEntity;
        }

        public virtual async Task UpdateAsync<TKey, TSource>(TKey id, TSource source)
        {
            PropertyInfo? propertyInfo = typeof(TSource).GetProperties().FirstOrDefault(p => p.Name.ToLower() == "id");
            if (propertyInfo is null)
            {
                throw new ArgumentException("The entity does not have an Id property");
            }

            TKey? sourceId = (TKey?)propertyInfo.GetValue(source);

            if (!EqualityComparer<TKey>.Default.Equals(id, sourceId))
            {
                throw new BadRequestException("Invalid Id used in request");
            }

            var entity = await GetByIdAsync<TKey, TEntity>(id);

            if (entity == null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            _mapper.Map(source, entity);
            CacheUtility.InvalidateCache<TEntity>(_cache);
            await _repository.UpdateAsync(entity);
        }


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