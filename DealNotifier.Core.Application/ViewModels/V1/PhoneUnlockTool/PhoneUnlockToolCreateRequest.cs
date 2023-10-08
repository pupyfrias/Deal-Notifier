using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.PhoneUnlockTool
{
    public class PhoneUnlockToolCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}