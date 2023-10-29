using Catalog.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.BanLink
{
    public class BanLinkUpdateRequest : IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Link { get; set; }
    }
}