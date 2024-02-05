using DealNotifier.Core.Application.ViewModels.Common;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IService<TEntity>
    {
        TDestination Create<TSource, TDestination>(TSource source);

        void Delete(int id);

        bool Exists(int id);

        IEnumerable<TDestination> GetAll<TDestination>();

        (IEnumerable<TSource>, string) GetAllWithETag<TSource>();

        PagedCollection<TDestination> GetAllWithPagination<TDestination, TSpecification>(IPaginationBase request) where TSpecification : ISpecification<TEntity>;

        TDestination GetByIdProjected<TDestination>(int id);

        TEntity GetById(int id);

        void Update<TSource>(int id, TSource source) where TSource : IHasId<int>;

        TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    } 
}