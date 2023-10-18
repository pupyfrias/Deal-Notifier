using DealNotifier.Core.Domain.Entities;
using Enums = DealNotifier.Core.Application.Enums;

namespace DealNotifier.Infrastructure.Persistence.Seeds
{
    public static class StatusSeed
    {
        public static List<StockStatus> Data { get;} = Enum.GetValues<Enums.StockStatus>()
                                                       .Select(e => new StockStatus { Id = (int)e, Name = e.ToString() })
                                                       .ToList();
    }
}