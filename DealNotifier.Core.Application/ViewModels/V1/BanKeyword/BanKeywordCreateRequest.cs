using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1.BanKeyword
{
    public class BanKeywordCreateRequest
    {
        [Required]
        public string Keyword { get; set; }
    }
}