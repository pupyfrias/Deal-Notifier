using Microsoft.Data.SqlClient;

namespace DealNotifier.Core.Application.Interfaces.Repositories
{
    public interface IItemSyncRepository 
    {
        Task UpdateStockStatusAsync(string query, SqlParameter idListString, SqlParameter onlineStoreId, SqlParameter outputResult, SqlParameter errorMessage);

    }
}