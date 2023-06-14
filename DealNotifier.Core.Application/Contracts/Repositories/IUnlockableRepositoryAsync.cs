using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Repositories
{
    public interface IUnlockableRepositoryAsync : IGenericRepositoryAsync<UnlockablePhone>
    {
        Task<UnlockablePhone?> GetByModelNumberAsync(string modelNumber);
    }
}
