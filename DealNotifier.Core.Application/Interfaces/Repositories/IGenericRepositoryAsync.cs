using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IGenericRepositoryAsync<TEntity, TKey>
    {
        Task<TEntity> CreateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);

        Task<List<TDestination>> GetAllAsync<TDestination>();

        Task<List<TDestination>> GetAllAsync<TDestination>(ISpecification<TEntity> spec);

        Task<TDestination?> GetByIdProjectedAsync<TDestination>(TKey id);

        Task<TEntity?> GetByIdAsync(TKey id);

        Task<int> GetTotalCountAsync(Expression<Func<TEntity, bool>> criteria);

        Task UpdateAsync(TEntity entity);
    }
}