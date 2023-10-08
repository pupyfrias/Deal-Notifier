using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier
{
    public class PhoneCarrierUnlockabledPhoneCreateRequest
    {
        [Required]
        public string UnlockabledPhones { get; set; }
    }
}