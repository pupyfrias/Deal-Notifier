using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneUnlockToolRepositoryAsync
    {
        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneUnlockTool> entities);
    }
}