using DealNotifier.Core.Application.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Controllers.V1
{
    public class PhoneUnlockToolUpdateRequest: IHasId<int>
    {
        [Required]
        public  int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}