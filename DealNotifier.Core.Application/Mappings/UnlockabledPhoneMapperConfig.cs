using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhone;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class UnlockabledPhoneMapperConfig : Profile
    {
        public UnlockabledPhoneMapperConfig()
        {
            CreateMap<UnlockabledPhoneCreateRequest, UnlockabledPhone>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhoneUpdateRequest, UnlockabledPhone>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhone, UnlockabledPhoneResponse>().IgnoreAllSourceNullProperties();
        }
    }
}