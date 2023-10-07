using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockableUnlockToolRepositoryAsync
    {
        Task<UnlockabledPhoneUnlockTool> CreateAsync(UnlockabledPhoneUnlockTool entity);
    }
}