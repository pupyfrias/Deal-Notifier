using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class StockStatus : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}