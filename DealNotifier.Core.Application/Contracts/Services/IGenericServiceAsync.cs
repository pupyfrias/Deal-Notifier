using DealNotifier.Core.Application.Models;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IGenericServiceAsync<TEntity>
    {
        Task<TDestination> CreateAsync<TSource, TDestination>(TSource source);

        Task DeleteAsync<TKey>(TKey id);

        Task<bool> ExistsAsync<TKey>(TKey id);

        Task<List<TDestination>> GetAllAsync<TDestination>();
        Task<(List<TSource>, string)> GetAllWithETagAsync<TSource>();

        Task<PagedCollection<TDestination>> GetAllWithPaginationAsync<TDestination, TSpecification>(IRequestBase request) where TSpecification : ISpecification<TEntity>;
        Task<TDestination> GetByIdAsync<TKey, TDestination>(TKey id);

        Task UpdateAsync<TKey, TSource>(TKey id, TSource source);
    }
}