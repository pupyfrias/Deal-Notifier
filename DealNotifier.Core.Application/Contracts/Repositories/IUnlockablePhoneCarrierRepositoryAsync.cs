using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Repositories
{
    public interface IUnlockableUnlockToolRepositoryAsync
    {
        Task<UnlockablePhoneUnlockTool> CreateAsync(UnlockablePhoneUnlockTool entity);

    }
}
