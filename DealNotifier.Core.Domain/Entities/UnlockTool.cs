using DealNotifier.Core.Domain.Common;

namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockTool : AuditableEntity<int>
    {
        public override int Id { get; set; }
        public string Name { get; set; }
        public List<UnlockablePhoneUnlockTool> UnlockableUnlockTools { get; set; }
    }
}