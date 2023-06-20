using AutoMapper;
using DealNotifier.Core.Application.DTOs.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class PhoneCarrierMapperConfig : Profile
    {
        public PhoneCarrierMapperConfig()
        {
            CreateMap<PhoneCarrierReadDto, PhoneCarrier>().ReverseMap();

        }
    }
}