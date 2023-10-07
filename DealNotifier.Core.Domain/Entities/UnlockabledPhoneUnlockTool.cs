namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockabledPhoneUnlockTool
    {
        public int UnlockablePhoneId { get; set; }
        public int PhoneUnlockToolId { get; set; }
        public UnlockabledPhone UnlockablePhone { get; set; }
        public PhoneUnlockTool PhoneUnlockTool { get; set; }
    }
}