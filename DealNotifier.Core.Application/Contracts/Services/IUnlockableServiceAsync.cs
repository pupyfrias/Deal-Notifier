using System.Collections.Concurrent;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Contracts.Services
{
    public interface IUnlockableServiceAsync : IGenericServiceAsync<UnlockablePhone>
    {
        Task<UnlockablePhone?> GetByModelNumberAsync(string modelNumber);
    }
}