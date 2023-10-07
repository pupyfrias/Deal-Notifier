using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.V1
{
    public class PhoneUnlockToolCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}