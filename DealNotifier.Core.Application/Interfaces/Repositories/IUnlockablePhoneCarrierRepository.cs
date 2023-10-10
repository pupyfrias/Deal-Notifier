using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneUnlockToolRepository
    {
        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneUnlockTool> entities);
        Task CreateAsync(UnlockabledPhonePhoneUnlockTool entity);
    }
}