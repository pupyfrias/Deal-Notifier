using DealNotifier.Core.Application.ViewModels.Common;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IGenericService<TEntity, TKey>
    {
        #region Async
        Task<TDestination> CreateAsync<TSource, TDestination>(TSource source);

        Task DeleteAsync(TKey id);

        Task<bool> ExistsAsync(TKey id);

        Task <IEnumerable <TDestination>> GetAllAsync<TDestination>();

        Task<(IEnumerable <TSource>, string)> GetAllWithETagAsync<TSource>();

        Task<PagedCollection<TDestination>> GetAllWithPaginationAsync<TDestination, TSpecification>(IPaginationBase request) where TSpecification : ISpecification<TEntity>;

        Task<TDestination> GetByIdProjectedAsync<TDestination>(TKey id);

        Task<TEntity> GetByIdAsync(TKey id);

        Task UpdateAsync<TSource>(TKey id, TSource source) where TSource : IHasId<TKey>;
        #endregion Async

        #region Sync
        TDestination Create<TSource, TDestination>(TSource source);

        void Delete(TKey id);

        bool Exists(TKey id);

        IEnumerable <TDestination> GetAll<TDestination>();

        (IEnumerable <TSource>, string) GetAllWithETag<TSource>();

        PagedCollection<TDestination> GetAllWithPagination<TDestination, TSpecification>(IPaginationBase request) where TSpecification : ISpecification<TEntity>;

        TDestination GetByIdProjected<TDestination>(TKey id);

        TEntity GetById(TKey id);

        void Update<TSource>(TKey id, TSource source) where TSource : IHasId<TKey>;
        #endregion Sync


    }
}