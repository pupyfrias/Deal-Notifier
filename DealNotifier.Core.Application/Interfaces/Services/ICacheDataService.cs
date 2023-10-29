using System.Collections.Concurrent;
using Catalog.Application.ViewModels.V1.PhoneCarrier;
using Catalog.Application.ViewModels.V1.Brand;
using Catalog.Application.ViewModels.V1.BanLink;
using Catalog.Application.ViewModels.V1.NotificationCriteria;
using Catalog.Application.ViewModels.V1.BanKeyword;

namespace Catalog.Application.Interfaces.Services
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