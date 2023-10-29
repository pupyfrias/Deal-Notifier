using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.PhoneUnlockTool
{
    public class PhoneUnlockToolUnlockablePhoneCreateRequest
    {
        [Required]
        public string UnlockablePhones { get; set; }
    }
}