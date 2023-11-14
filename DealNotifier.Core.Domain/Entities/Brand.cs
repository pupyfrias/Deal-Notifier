using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class Brand : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<UnlockabledPhone> UnlockabledPhones { get; set; }

    }
}