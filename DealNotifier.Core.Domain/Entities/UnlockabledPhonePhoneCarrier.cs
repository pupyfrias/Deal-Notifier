namespace Catalog.Domain.Entities
{
    public class UnlockabledPhonePhoneCarrier
    {
        public int UnlockabledPhoneId { get; set; }
        public int PhoneCarrierId { get; set; }
        public UnlockabledPhone UnlockabledPhone { get; set; }
        public PhoneCarrier PhoneCarrier { get; set; }
    }
}