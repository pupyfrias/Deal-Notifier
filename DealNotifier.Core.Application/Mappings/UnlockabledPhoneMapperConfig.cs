using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.UnlockabledPhone;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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