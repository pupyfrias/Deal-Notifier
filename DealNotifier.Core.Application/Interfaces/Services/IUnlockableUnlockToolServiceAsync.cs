using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockableUnlockToolServiceAsync
    {
        Task<UnlockabledPhoneUnlockTool> CreateAsync(UnlockabledPhoneUnlockTool entity);
    }
}