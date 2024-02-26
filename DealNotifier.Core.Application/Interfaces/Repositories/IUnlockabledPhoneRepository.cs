using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IUnlockabledPhoneRepository : IAsyncRepository<UnlockabledPhone>
    {
        Task<bool> ExistsAsync(string modelName);

        IEnumerable<UnlockabledPhone> Where(Func<UnlockabledPhone, bool> predate);


    }
}