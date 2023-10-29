using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.BanLink
{
    public class BanLinkCreateRequest
    {
        [Required]
        public string Link { get; set; }
    }
}