using Catalog.Domain.Common;

namespace Catalog.Domain.Entities
{
    public class BanKeyword : AuditableEntity
    {
        public string Keyword { get; set; }
    }
}