using Catalog.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.UnlockabledPhone
{
    public class UnlockabledPhoneUpdateRequest : IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int BrandId { get; set; }
        public string? Comment { get; set; }
        [Required]
        public string ModelName { get; set; }
        [Required]
        public string ModelNumber { get; set; }
    }
}