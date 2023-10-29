using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.PhoneCarrier
{
    public class PhoneCarrierUnlockabledPhoneCreateRequest
    {
        [Required]
        public string UnlockabledPhones { get; set; }
    }
}