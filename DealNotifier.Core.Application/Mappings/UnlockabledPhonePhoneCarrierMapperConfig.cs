using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class UnlockabledPhonePhoneCarrierMapperConfig : Profile
    {
        public UnlockabledPhonePhoneCarrierMapperConfig()
        {
            CreateMap<UnlockabledPhonePhoneCarrierCreate, UnlockabledPhonePhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhonePhoneCarrierUpdateRequest, UnlockabledPhonePhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhonePhoneCarrier, UnlockabledPhonePhoneCarrierResponse>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhonePhoneCarrierDto, UnlockabledPhonePhoneCarrierCreate>().IgnoreAllSourceNullProperties().ReverseMap();
            CreateMap<UnlockabledPhonePhoneCarrierDto, UnlockabledPhonePhoneCarrier>().IgnoreAllSourceNullProperties().ReverseMap();
        }
    }
}