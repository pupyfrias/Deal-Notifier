namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockablePhoneUnlockTool
    {
        public int UnlockablePhoneId { get; set; }
        public int UnlockToolId { get; set; }
        public UnlockablePhone Unlockable { get; set; }
        public UnlockTool UnlockTool { get; set; }

    }
}