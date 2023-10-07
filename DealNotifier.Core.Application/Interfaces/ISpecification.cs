using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces
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