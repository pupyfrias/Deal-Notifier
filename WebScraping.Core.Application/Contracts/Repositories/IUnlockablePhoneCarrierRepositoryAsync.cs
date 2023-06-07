using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Repositories
{
    public interface IUnlockableUnlockToolRepositoryAsync
    {
        Task<UnlockablePhoneUnlockTool> CreateAsync(UnlockablePhoneUnlockTool entity);

    }
}
