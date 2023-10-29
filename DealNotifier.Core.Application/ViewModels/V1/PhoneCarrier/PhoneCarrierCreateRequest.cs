using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.PhoneCarrier
{
    public class PhoneCarrierCreateRequest
    {
        [Required]
        public string Name { get; set; }
        [MaxLength(3)]
        public string ShortName { get; set; }
    }
}