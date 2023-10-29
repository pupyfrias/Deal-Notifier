using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.ItemType
{
    public class ItemTypeCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}