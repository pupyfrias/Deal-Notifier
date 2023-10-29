using System.ComponentModel.DataAnnotations;

namespace Catalog.Application.ViewModels.V1.OnlineStore
{
    public class OnlineStoreCreateRequest
    {
        [Required]
        public string Name { get; set; }
    }
}