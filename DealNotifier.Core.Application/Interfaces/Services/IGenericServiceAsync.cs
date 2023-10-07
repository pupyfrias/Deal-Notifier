using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IGenericServiceAsync<TEntity, TKey>
    {
        Task<TEntity> CreateAsync<TSource>(TSource source);

        Task DeleteAsync(TKey id);

        Task<bool> ExistsAsync(TKey id);

        Task<List<TDestination>> GetAllAsync<TDestination>();

        Task<(List<TSource>, string)> GetAllWithETagAsync<TSource>();

        Task<PagedCollection<TDestination>> GetAllWithPaginationAsync<TDestination, TSpecification>(IPaginationBase request) where TSpecification : ISpecification<TEntity>;

        Task<TDestination> GetByIdProjectedAsync<TDestination>(TKey id);

        Task<TEntity> GetByIdAsync(TKey id);

        Task UpdateAsync<TSource>(TKey id, TSource source);
    }
}