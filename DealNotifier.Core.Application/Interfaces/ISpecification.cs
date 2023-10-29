using System.Linq.Expressions;

namespace Catalog.Application.Interfaces
{
    public interface ISpecification<TEntity>
    {
        Expression<Func<TEntity, bool>> Criteria { get; }
        Expression<Func<TEntity, object>> OrderBy { get; }
        int Skip { get; }
        int Take { get; }
        bool Descending { get; }
    }
}