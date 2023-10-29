using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhoneRepository : IGenericRepository<UnlockabledPhone>
    {
        Task<bool> ExistsAsync(string modelName);
    }
}