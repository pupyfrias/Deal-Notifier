using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockableService : IGenericService<UnlockabledPhone, int>
    {
        Task<UnlockabledPhone?> GetByModelNumberAsync(string modelNumber);
    }
}