using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class BanKeyword : AuditableEntity
    {
        public string Keyword { get; set; }
    }
}