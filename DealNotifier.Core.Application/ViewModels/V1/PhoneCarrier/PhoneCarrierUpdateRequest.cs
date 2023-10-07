using DealNotifier.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.V1
{
    public class PhoneCarrierUpdateRequest: IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(3)]
        public string ShortName { get; set; }
    }
}