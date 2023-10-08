using DealNotifier.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.BanLink
{
    public class BanLinkUpdateRequest: IHasId<int>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Link { get; set; }
    }
}