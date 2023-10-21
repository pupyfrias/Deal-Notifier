using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IUnlockProbabilityService : IGenericService<UnlockProbability>
    {
        Task SetUnlockProbabilityAsync(ItemCreateRequest itemCreate);
    }
}