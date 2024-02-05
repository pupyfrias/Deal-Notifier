using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface IStockStatusService : IAsyncService<StockStatus>
    {
    }
}