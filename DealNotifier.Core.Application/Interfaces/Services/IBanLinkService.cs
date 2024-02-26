using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IBanLinkService : IAsyncService<BanLink>
    {
        Task CreateRangeAsync(IEnumerable<int> ids);
    }
}