using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class StatusSeed
    {
        public static List<StockStatus> data { get; set; } = new List<StockStatus>
        {
            new StockStatus
            {
                Id= (int) Enums.Status.InStock,
                Name= Enums.Status.InStock.ToString()
            },
            new StockStatus
            {
                Id= (int) Enums.Status.OutStock,
                Name= Enums.Status.OutStock.ToString()
            }
        };
    }
}