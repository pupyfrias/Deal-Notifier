using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Repositories
{
    public interface IUnlockableRepositoryAsync: IGenericRepositoryAsync<UnlockablePhone>
    {
        Task<UnlockablePhone?> GetByModelNumberAsync(string modelNumber);
    }
}
