using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhonePhoneUnlockToolRepository
    {
        Task CreateRangeAsync(IEnumerable<UnlockabledPhonePhoneUnlockTool> entities);
        Task CreateAsync(UnlockabledPhonePhoneUnlockTool entity);

        Task<bool> ExistsAsync(UnlockabledPhonePhoneUnlockTool entity);


    }
}