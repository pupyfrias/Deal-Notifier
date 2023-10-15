using DealNotifier.Core.Application.ViewModels.Common;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IGenericService<TEntity>
    {
        #region Async
        Task<TDestination> CreateAsync<TSource, TDestination>(TSource source);

        Task DeleteAsync(int id);

        Task<bool> ExistsAsync(int id);

        Task <IEnumerable <TDestination>> GetAllAsync<TDestination>();

        Task<(IEnumerable <TSource>, string)> GetAllWithETagAsync<TSource>();

        Task<PagedCollection<TDestination>> GetAllWithPaginationAsync<TDestination, TSpecification>(IPaginationBase request) where TSpecification : ISpecification<TEntity>;

        Task<TDestination> GetByIdProjectedAsync<TDestination>(int id);

        Task<TEntity> GetByIdAsync(int id);

        Task UpdateAsync<TSource>(int id, TSource source) where TSource : IHasId<int>;

        Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        #endregion Async

        #region Sync
        TDestination Create<TSource, TDestination>(TSource source);

        void Delete(int id);

        bool Exists(int id);

        IEnumerable <TDestination> GetAll<TDestination>();

        (IEnumerable <TSource>, string) GetAllWithETag<TSource>();

        PagedCollection<TDestination> GetAllWithPagination<TDestination, TSpecification>(IPaginationBase request) where TSpecification : ISpecification<TEntity>;

        TDestination GetByIdProjected<TDestination>(int id);

        TEntity GetById(int id);

        void Update<TSource>(int id, TSource source) where TSource : IHasId<int>;

        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
        #endregion Sync


    }
}