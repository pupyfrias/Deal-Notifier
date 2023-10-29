using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.PhoneUnlockTool
{
    public class PhoneUnlockToolCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}