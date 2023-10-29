using System.Collections.Concurrent;
using Catalog.Application.Interfaces.Services;
using Catalog.Application.ViewModels.V1.PhoneCarrier;
using Catalog.Application.ViewModels.V1.Brand;
using Catalog.Application.ViewModels.V1.BanLink;
using Catalog.Application.ViewModels.V1.NotificationCriteria;
using Catalog.Application.ViewModels.V1.BanKeyword;

namespace Catalog.Application.Services
{
    public class CacheDataService : ICacheDataService
    {
        public ConcurrentBag<string> CheckedList { get; set; } = new ConcurrentBag<string>();
        public HashSet<BanKeywordDto> BanKeywordList { get; set; } = new HashSet<BanKeywordDto>();
        public HashSet<BanLinkDto> BanLinkList { get; set; } = new HashSet<BanLinkDto>();
        public HashSet<BrandDto> BrandList { get; set; } = new HashSet<BrandDto>();
        public HashSet<NotificationCriteriaDto> NotificationCriteriaList { get; set; } = new HashSet<NotificationCriteriaDto>();
        public HashSet<PhoneCarrierDto> PhoneCarrierList { get; set; } = new HashSet<PhoneCarrierDto>();
    }
}
