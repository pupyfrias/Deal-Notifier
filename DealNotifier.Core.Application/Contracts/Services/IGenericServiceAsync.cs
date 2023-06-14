namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IGenericServiceAsync<TEntity>
    {
        Task<TEntity> CreateAsync<TSource>(TSource source);

        Task DeleteAsync<TKey>(TKey id);

        Task<bool> ExistsAsync<TKey>(TKey id);

        Task<List<TDestination>> GetAllAsync<TDestination>();

        Task<(List<TSource>, string)> GetAllWithETagAsync<TSource>();

        Task<TDestination> GetByIdAsync<TKey, TDestination>(TKey id);

        Task UpdateAsync<TKey, TSource>(TKey id, TSource source);
    }
}