using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepository<TEntity, TKey>
    {
        #region Sync Methods
        TEntity Create(TEntity entity);

        void Delete(TEntity entity);

        List<TDestination> GetAllProjected<TDestination>();

        List<TDestination> GetAllProjected<TDestination>(ISpecification<TEntity> spec);

        TEntity? GetById(TKey id);

        TDestination? GetByIdProjected<TDestination>(TKey id);
        int GetTotalCount(Expression<Func<TEntity, bool>> predicate);
        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        void Update(TEntity entity);
        #endregion Sync Methods


        #region Async Methods
        Task<TEntity> CreateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<List<TDestination>> GetAllProjectedAsync<TDestination>();

        Task<List<TDestination>> GetAllProjectedAsync<TDestination>(ISpecification<TEntity> spec);

        Task<TEntity?> GetByIdAsync(TKey id);

        Task<TDestination?> GetByIdProjectedAsync<TDestination>(TKey id);
        Task<int> GetTotalCountAsync(Expression<Func<TEntity, bool>> predicate);

        Task UpdateAsync(TEntity entity);

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion Async Methods


    }
}