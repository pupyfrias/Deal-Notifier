using AutoMapper;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Application.Contracts.Services;
using DealNotifier.Core.Application.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using DealNotifier.Core.Domain.Exceptions;

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


        public virtual async Task<TEntity> CreateAsync<TSource>(TSource source)
        {
            var entity = _mapper.Map<TEntity>(source);
            await _repository.CreateAsync(entity);
            CacheUtils.InvalidateCache<TEntity>(_cache);
            return entity;
        }

        public virtual async Task DeleteAsync<TKey>(TKey id)
        {
            var entity = await GetByIdAsync<TKey, TEntity>(id);

            if (entity is null)
            {
                throw new NotFoundException(typeof(TEntity).Name, id);
            }

            await _repository.DeleteAsync(entity);
            CacheUtils.InvalidateCache<TEntity>(_cache);
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

        public async Task<(List<TDestination>, string)> GetAllWithETagAsync<TDestination>()
        {
            string cacheKey = $"List_{typeof(TEntity).FullName}";

            if (!_cache.TryGetValue(cacheKey, out List<TDestination> mappedEntities))
            {
                mappedEntities = await _repository.GetAllAsync<TDestination>();

                _cache.Set(cacheKey, mappedEntities, TimeSpan.FromHours(24));
            }

            var eTag = CacheUtils.GenerateETag(mappedEntities);
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
            CacheUtils.InvalidateCache<TEntity>(_cache);
            await _repository.UpdateAsync(entity);
        }


        #endregion Public Methods
    }
}