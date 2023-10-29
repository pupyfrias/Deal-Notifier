using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class StockStatus : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}