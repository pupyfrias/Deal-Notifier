using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class Brand : EntityBase
    {
        public string Name { get; set; }
        public IEnumerable<UnlockabledPhone> UnlockabledPhones { get; set; }
        public IEnumerable<Item> Items { get; set; }

    }
}