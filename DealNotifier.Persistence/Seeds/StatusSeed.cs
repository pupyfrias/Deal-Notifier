
using Catalog.Domain.Entities;
using Enums = Catalog.Application.Enums;


namespace Catalog.Persistence.Seeds
{
    public static class StatusSeed
    {
        public static List<StockStatus> Data { get; } = Enum.GetValues<Enums.StockStatus>()
                                                       .Select(e => new StockStatus { Id = (int)e, Name = e.ToString() })
                                                       .ToList();
    }
}