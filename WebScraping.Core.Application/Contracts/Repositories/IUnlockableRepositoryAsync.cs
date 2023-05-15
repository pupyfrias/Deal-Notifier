using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Repositories
{
    public interface IUnlockableRepositoryAsync: IGenericRepositoryAsync<Unlockable>
    {
        Task<Unlockable?> GetByModelNumberAsync(string modelNumber);
    }
}
