using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class BanLink : AuditableEntity
    {
        public string Link { get; set; }
    }
}