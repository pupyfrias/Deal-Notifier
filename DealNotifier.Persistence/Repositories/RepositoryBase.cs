using AutoMapper;
using AutoMapper.QueryableExtensions;
using DealNotifier.Core.Application.Interfaces;
using DealNotifier.Core.Application.Interfaces.Repositories;
using DealNotifier.Core.Domain.Common;
using DealNotifier.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DealNotifier.Persistence.Repositories
{
    public class RepositoryBase<TEntity> : IAsyncRepository<TEntity> where TEntity : EntityBase
    {
        #region Private Variables

        protected readonly ApplicationDbContext dbContext;
        private readonly IConfigurationProvider _configurationProvider;

        #endregion Private Variables

        #region Constructor

        public RepositoryBase(ApplicationDbContext context, IConfigurationProvider configurationProvider)
        {
            dbContext = context;
            _configurationProvider = configurationProvider;
        }

        #endregion Constructor

        #region Public Methods

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await dbContext.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<TDestination?> GetByIdProjectedAsync<TDestination>(int id)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();
            return await query.Where(x => x.Id.Equals(id))
                              .AsNoTracking()
                              .ProjectTo<TDestination>(_configurationProvider)
                              .FirstOrDefaultAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            var query = dbContext.Set<TEntity>().AsQueryable();
            return await query.Where(x => x.Id.Equals(id))
                              .FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalCountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (predicate == null)
            {
                return await dbContext.Set<TEntity>().CountAsync();
            }
            else
            {
                return await dbContext.Set<TEntity>().Where(predicate).CountAsync();
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            dbContext.Update(entity);
            await dbContext.SaveChangesAsync();
        }

        public async Task<List<TDestination>> GetAllProjectedAsync<TDestination>(ISpecification<TEntity> spec)
        {
            var query = dbContext.Set<TEntity>().AsQueryable().AsNoTracking();
            query = ApplySpecification(query, spec);

            return await query.ProjectTo<TDestination>(_configurationProvider).ToListAsync();
        }

        public async Task<List<TDestination>> GetAllProjectedAsync<TDestination>()
        {
            return await dbContext.Set<TEntity>()
                                   .AsNoTracking()
                                   .ProjectTo<TDestination>(_configurationProvider)
                                   .ToListAsync();
        }

        public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }


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