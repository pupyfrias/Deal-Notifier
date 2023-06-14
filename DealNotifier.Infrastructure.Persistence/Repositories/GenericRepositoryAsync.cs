using AutoMapper;
using AutoMapper.QueryableExtensions;
using DealNotifier.Infrastructure.Persistence.DbContexts;
using Microsoft.EntityFrameworkCore;
using DealNotifier.Core.Application.Contracts;
using DealNotifier.Core.Application.Contracts.Repositories;
using DealNotifier.Core.Domain.Contracts;

namespace DealNotifier.Infrastructure.Persistence.Repositories
{
    public class GenericRepositoryAsync<TEntity> : IGenericRepositoryAsync<TEntity> where TEntity : class, IAuditableEntity
    {
        #region Private Variable

        private readonly ApplicationDbContext _context;
        private readonly IConfigurationProvider _configurationProvider;

        #endregion Private Variable

        #region Constructor

        public GenericRepositoryAsync(ApplicationDbContext context, IConfigurationProvider configurationProvider)
        {
            _context = context;
            _configurationProvider = configurationProvider;
        }

        #endregion Constructor

        #region Public MethodsT


        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _context.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }

            return entity;
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<TDestination?> GetByIdAsync<TKey, TDestination>(TKey id)
        {
            var query = _context.Set<TEntity>().AsQueryable();
            return await query.Where(x => x.Id.Equals(id))
                              .ProjectTo<TDestination>(_configurationProvider)
                              .FirstOrDefaultAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Set<TEntity>().CountAsync();
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }


        public async Task<List<TDestination>> GetAllAsync<TDestination>(ISpecification<TEntity> spec)
        {
            var query = _context.Set<TEntity>().AsQueryable();

            if (spec != null)
            {
                query = ApplySpecification(query, spec);
            }


            return await query.ProjectTo<TDestination>(_configurationProvider)
                              .ToListAsync();
        }


        #endregion Public Methods

        #region Private Methods
        private IQueryable<TEntity> ApplySpecification(IQueryable<TEntity> query, ISpecification<TEntity> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
        #endregion Private Methods
    }
}