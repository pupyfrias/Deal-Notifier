using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.BanKeyword
{
    public class BanKeywordUpdateRequest
    {
        [Required]
        public int? Id { get; set; }
        [Required]
        public string Keyword { get; set; }
    }
}