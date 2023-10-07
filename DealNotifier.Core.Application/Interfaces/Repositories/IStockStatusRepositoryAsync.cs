using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IStockStatusRepositoryAsync : IGenericRepositoryAsync<StockStatus, int>
    {
    }
}