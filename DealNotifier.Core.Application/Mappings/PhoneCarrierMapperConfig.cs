using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.PhoneCarrier;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class PhoneCarrierMapperConfig : Profile
    {
        public PhoneCarrierMapperConfig()
        {
            CreateMap<PhoneCarrierDto, PhoneCarrier>().ReverseMap();
            CreateMap<PhoneCarrierCreateRequest, PhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneCarrierUpdateRequest, PhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneCarrier, PhoneCarrierResponse>().IgnoreAllSourceNullProperties();
        }
    }
}