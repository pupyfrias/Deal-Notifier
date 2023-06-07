using System.Collections.Concurrent;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Contracts.Services
{
    public interface IUnlockableServiceAsync: IGenericServiceAsync<UnlockablePhone>
    {
        Task<UnlockablePhone?> GetByModelNumberAsync(string modelNumber);
    }
}