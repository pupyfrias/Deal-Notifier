using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class BanKeyword : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Keyword { get; set; }
    }
}