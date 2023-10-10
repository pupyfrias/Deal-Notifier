using DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockabledPhonePhoneUnlockToolService
    {
        Task CreateRangeAsync(int PhoneUnlockToolId, PhoneUnlockToolUnlockablePhoneCreateRequest request);
        Task CreateAsync(UnlockabledPhonePhoneUnlockToolCreate model);
    }
}