using System.Collections.Concurrent;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IUnlockableUnlockToolServiceAsync
    {
        Task<UnlockablePhoneUnlockTool> CreateAsync(UnlockablePhoneUnlockTool entity);
    }
}