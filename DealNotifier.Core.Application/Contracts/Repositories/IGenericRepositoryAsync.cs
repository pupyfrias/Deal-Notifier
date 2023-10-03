using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Contracts.Repositories
{
    public interface IGenericRepositoryAsync<TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<List<TDestination>> GetAllAsync<TDestination>();
        Task<List<TDestination>> GetAllAsync<TDestination>(ISpecification<TEntity> spec);

        Task<TDestination?> GetByIdAsync<TKey, TDestination>(TKey id);

        Task<int> GetTotalCountAsync(Expression<Func<TEntity, bool>> criteria);

        Task UpdateAsync(TEntity entity);

    }
}