using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class Brand : AuditableEntity
    {
        public string Name { get; set; }
        public IEnumerable<UnlockabledPhone> UnlockabledPhones { get; set; }
    }
}