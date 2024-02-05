using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class StockStatus : EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}