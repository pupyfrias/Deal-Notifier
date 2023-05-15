using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Repositories
{
    public interface IUnlockableUnlockToolRepositoryAsync
    {
        Task<UnlockableUnlockTool> CreateAsync(UnlockableUnlockTool entity);

    }
}
