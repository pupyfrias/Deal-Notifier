using System.Collections.Concurrent;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IUnlockableUnlockToolServiceAsync
    {
        Task<UnlockablePhoneUnlockTool> CreateAsync(UnlockablePhoneUnlockTool entity);
    }
}