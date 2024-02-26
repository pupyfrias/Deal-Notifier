using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Application.ViewModels.V1;
using System.Collections.Concurrent;
using DealNotifier.Core.Application.Interfaces.Services;

namespace DealNotifier.Core.Application.Services
{
    public class CacheDataService : ICacheDataService
    {
        public ConcurrentBag<string> CheckedList { get; set; } 
        public HashSet<BanKeywordDto> BanKeywordList { get; set; } 
        public HashSet<BanLinkDto> BanLinkList { get; set; }
        public HashSet<BrandDto> BrandList { get; set; }
        public HashSet<NotificationCriteriaDto> NotificationCriteriaList { get; set; } 
        public HashSet<PhoneCarrierDto> PhoneCarrierList { get; set; }

        public CacheDataService()
        {
            PhoneCarrierList = new HashSet<PhoneCarrierDto>();
            BanKeywordList = new HashSet<BanKeywordDto>();
            BanLinkList = new HashSet<BanLinkDto>();
            BrandList = new HashSet<BrandDto>();
            NotificationCriteriaList = new HashSet<NotificationCriteriaDto>();
            CheckedList = new ConcurrentBag<string>();
            

        }
    }
}
