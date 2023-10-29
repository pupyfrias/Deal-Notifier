using Catalog.Application.Interfaces;
using System.Linq.Expressions;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity>
    {
        #region Sync Methods
        TEntity Create(TEntity entity);

        void Delete(TEntity entity);

        List<TDestination> GetAllProjected<TDestination>();

        List<TDestination> GetAllProjected<TDestination>(ISpecification<TEntity> spec);

        TEntity? GetById(int id);

        TDestination? GetByIdProjected<TDestination>(int id);
        int GetTotalCount(Expression<Func<TEntity, bool>> predicate);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);

        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate);
        #endregion Sync Methods


        #region Async Methods
        Task<TEntity> CreateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<List<TDestination>> GetAllProjectedAsync<TDestination>();

        Task<List<TDestination>> GetAllProjectedAsync<TDestination>(ISpecification<TEntity> spec);

        Task<TEntity?> GetByIdAsync(int id);

        Task<TDestination?> GetByIdProjectedAsync<TDestination>(int id);
        Task<int> GetTotalCountAsync(Expression<Func<TEntity, bool>> predicate);

        Task UpdateAsync(TEntity entity);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion Async Methods


    }
}