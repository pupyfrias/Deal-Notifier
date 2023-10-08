using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool
{
    public class PhoneUnlockToolUnlockablePhoneCreateRequest
    {
        [Required]
        public string UnlockablePhone { get; set; }
    }
}