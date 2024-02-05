using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockProbabilityService : IAsyncService<UnlockProbability>
    {
        Task SetUnlockProbabilityAsync(ItemDto itemCreate);
    }
}