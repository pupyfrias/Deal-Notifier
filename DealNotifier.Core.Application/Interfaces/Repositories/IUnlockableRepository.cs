using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockableRepository : IGenericRepository<UnlockabledPhone, int>
    {
        Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber);
    }
}