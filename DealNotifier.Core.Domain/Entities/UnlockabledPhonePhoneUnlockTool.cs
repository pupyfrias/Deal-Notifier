namespace DealNotifier.Core.Domain.Entities
{
    public class UnlockabledPhonePhoneUnlockTool
    {
        public int UnlockabledPhoneId { get; set; }
        public int PhoneUnlockToolId { get; set; }
        public UnlockabledPhone UnlockabledPhone { get; set; }
        public PhoneUnlockTool PhoneUnlockTool { get; set; }
    }
}