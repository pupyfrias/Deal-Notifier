using DealNotifier.Core.Domain.Entities;
using System.Linq.Expressions;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhoneRepository : IGenericRepository<UnlockabledPhone, int>
    {
       Task<UnlockabledPhone?> FirstOrDefaultAsync(Expression<Func<UnlockabledPhone, bool>> predicate);
    }
}