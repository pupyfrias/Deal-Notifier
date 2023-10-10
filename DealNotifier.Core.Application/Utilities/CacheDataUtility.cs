using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1;
using System.Collections.Concurrent;

namespace DealNotifier.Core.Application.Utilities
{
    public static class CacheDataUtility
    {
        public static ConcurrentBag<string> CheckedList { get; set; } = new ConcurrentBag<string>();
        public static HashSet<BanKeywordDto> BanKeywordList { get; set; } = new HashSet<BanKeywordDto>();
        public static HashSet<BanLinkDto> BanLinkList { get; set; } = new HashSet<BanLinkDto>();
        public static HashSet<BrandDto> BrandList { get; set; } = new HashSet<BrandDto>();
        public static HashSet<NotificationCriteriaDto> NotificationCriteriaList { get; set; } = new HashSet<NotificationCriteriaDto>();
        public static HashSet<PhoneCarrierDto> PhoneCarrierList { get; set; } = new HashSet<PhoneCarrierDto>();
    }
}
