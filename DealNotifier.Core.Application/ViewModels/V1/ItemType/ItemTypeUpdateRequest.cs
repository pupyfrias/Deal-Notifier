using DealNotifier.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.ItemType
{
    public class ItemTypeUpdateRequest: IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}