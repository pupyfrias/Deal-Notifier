using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Services
{
    public interface IUnlockProbabilityService : IGenericService<UnlockProbability>
    {
        Task SetUnlockProbabilityAsync(ItemCreateRequest itemCreate);
    }
}