using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Interfaces.Services
{
    public interface ICacheDataService
    {
        ConcurrentBag<string> CheckedList { get; set; }
        HashSet<BanKeywordDto> BanKeywordList { get; set; }
        HashSet<BanLinkDto> BanLinkList { get; set; }
        HashSet<BrandDto> BrandList { get; set; }
        HashSet<NotificationCriteriaDto> NotificationCriteriaList { get; set; }
        HashSet<PhoneCarrierDto> PhoneCarrierList { get; set; }
    }
}