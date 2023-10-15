using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class StatusSeed
    {
        public static List<StockStatus> Data { get;} = new List<StockStatus>
        {
            new StockStatus
            {
                Id= (int) Enums.StockStatus.InStock,
                Name= Enums.StockStatus.InStock.ToString()
            },
            new StockStatus
            {
                Id= (int) Enums.StockStatus.OutStock,
                Name= Enums.StockStatus.OutStock.ToString()
            }
        };
    }
}