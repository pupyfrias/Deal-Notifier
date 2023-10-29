using AutoMapper;
using AutoMapper.QueryableExtensions;
using Catalog.Application.Interfaces;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Contracts;
using Catalog.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Catalog.Persistence.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class, IAuditableEntity
    {
        #region Private Variables

        private readonly ApplicationDbContext _dbContext;
        private readonly IConfigurationProvider _configurationProvider;

        #endregion Private Variables

        #region Constructor

        public GenericRepository(ApplicationDbContext context, IConfigurationProvider configurationProvider)
        {
            _dbContext = context;
            _configurationProvider = configurationProvider;
        }

        #endregion Constructor

        #region Public Methods

        #region Async

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<TDestination?> GetByIdProjectedAsync<TDestination>(int id)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            return await query.Where(x => x.Id.Equals(id))
                              .ProjectTo<TDestination>(_configurationProvider)
                              .FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            return await query.Where(x => x.Id.Equals(id))
                              .FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return await _dbContext.Set<TEntity>().CountAsync();
            }
            else
            {
                return await _dbContext.Set<TEntity>().Where(predicate).CountAsync();
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbContext.Update(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<TDestination>> GetAllProjectedAsync<TDestination>(ISpecification<TEntity> spec)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            query = ApplySpecification(query, spec);

            var queryString = query.ProjectTo<TDestination>(_configurationProvider).ToQueryString(); //DELETE THIS

            return await query.ProjectTo<TDestination>(_configurationProvider).ToListAsync();
        }

        public async Task<List<TDestination>> GetAllProjectedAsync<TDestination>()
        {
            return await _dbContext.Set<TEntity>().ProjectTo<TDestination>(_configurationProvider).ToListAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        #endregion Async

        #region Sync

        public TEntity Create(TEntity entity)
        {
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public void Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public TDestination? GetByIdProjected<TDestination>(int id)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            return query.Where(x => x.Id.Equals(id))
                              .ProjectTo<TDestination>(_configurationProvider)
                              .FirstOrDefault();
        }

        public TEntity? GetById(int id)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            return query.Where(x => x.Id.Equals(id))
                              .FirstOrDefault();
        }

        public int GetTotalCount(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return _dbContext.Set<TEntity>().Count();
            }
            else
            {
                return _dbContext.Set<TEntity>().Where(predicate).Count();
            }
        }

        public void Update(TEntity entity)
        {
            _dbContext.Update(entity);
            _dbContext.SaveChanges();
        }

        public List<TDestination> GetAllProjected<TDestination>(ISpecification<TEntity> spec)
        {
            var query = _dbContext.Set<TEntity>().AsQueryable();
            query = ApplySpecification(query, spec);

            var queryString = query.ProjectTo<TDestination>(_configurationProvider).ToQueryString(); //DELETE THIS

            return query.ProjectTo<TDestination>(_configurationProvider).ToList();
        }

        public List<TDestination> GetAllProjected<TDestination>()
        {
            return _dbContext.Set<TEntity>().ProjectTo<TDestination>(_configurationProvider).ToList();
        }

        public TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().FirstOrDefault(predicate);
        }


        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbContext.Set<TEntity>().Where(predicate);
        }

        #endregion Sync

        #endregion Public Methods

        #region Private Methods

        private IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query, ISpecification<TEntity> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.Descending)
            {
                query = query.OrderByDescending(spec.OrderBy);
            }
            else
            {
                query = query.OrderBy(spec.OrderBy);
            }

            return query.Skip(spec.Skip).Take(spec.Take);
        }

        #endregion Private Methods
    }
}