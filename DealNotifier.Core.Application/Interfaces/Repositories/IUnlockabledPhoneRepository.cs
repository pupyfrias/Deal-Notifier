using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhoneRepository : IGenericRepository<UnlockabledPhone, int>
    {
        Task<bool> ExistsAsync(string modelName);
    }
}