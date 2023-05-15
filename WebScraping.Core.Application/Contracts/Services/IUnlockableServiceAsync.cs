using System.Collections.Concurrent;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IUnlockableServiceAsync: IGenericServiceAsync<Unlockable>
    {
        Task<Unlockable?> GetByModelNumberAsync(string modelNumber);
    }
}