using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class BanLink : AuditableEntity
    {
        public string Link { get; set; }
    }
}