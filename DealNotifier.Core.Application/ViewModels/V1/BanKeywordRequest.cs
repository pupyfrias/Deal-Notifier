using System.ComponentModel.DataAnnotations;

namespace DealNotifier.Core.Application.ViewModels.V1
{
    public class BanKeywordRequest
    {
        [Required]
        public string Keyword { get; set; }
    }
}