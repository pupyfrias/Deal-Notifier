using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;
using WebApi.Controllers.V1;

namespace DealNotifier.Core.Application.Mappings
{
    public class PhoneCarrierMapperConfig : Profile
    {
        public PhoneCarrierMapperConfig()
        {
            CreateMap<PhoneCarrierReadDto, PhoneCarrier>().ReverseMap();
            CreateMap<PhoneCarrierCreateRequest, PhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneCarrierUpdateRequest, PhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneCarrier, PhoneCarrierResponse>().IgnoreAllSourceNullProperties();
        }
    }
}