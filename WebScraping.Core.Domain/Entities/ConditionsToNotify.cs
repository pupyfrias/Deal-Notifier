using WebScraping.Core.Domain.Common;

namespace WebScraping.Core.Domain.Entities
{
    public class ConditionsToNotify : AuditableBaseEntity
    {
        public int Id { get; set; }
        public string Keywords { get; set; }
        public decimal MaxPrice { get; set; }
        public int ConditionId { get; set; }
        public Condition Condition { get; set; }
    }
}