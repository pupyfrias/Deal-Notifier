using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class BanLink : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Link { get; set; }
    }
}