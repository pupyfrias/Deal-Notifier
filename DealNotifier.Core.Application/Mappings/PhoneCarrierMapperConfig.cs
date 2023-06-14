using AutoMapper;
using DealNotifier.Core.Application.DTOs.PhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class PhoneCarriermapperConfig : Profile
    {
        public PhoneCarriermapperConfig()
        {
            CreateMap<PhoneCarrierReadDto, PhoneCarrier>().ReverseMap();

        }
    }
}