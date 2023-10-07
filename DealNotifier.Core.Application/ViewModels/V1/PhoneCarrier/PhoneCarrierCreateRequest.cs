using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.V1
{
    public class PhoneCarrierCreateRequest
    {
        [Required]
        public string Name { get; set; }
        [MaxLength(3)]
        public string ShortName { get; set; }
    }
}