using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockableServiceAsync : IGenericServiceAsync<UnlockabledPhone, int>
    {
        Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber);
    }
}