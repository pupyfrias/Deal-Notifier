using AutoMapper;
using WebScraping.Core.Application.Dtos.PhoneCarrier;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Mappings
{
    public class PhoneCarriermapperConfig : Profile
    {
        public PhoneCarriermapperConfig()
        {
            CreateMap<PhoneCarrierReadDto, PhoneCarrier>().ReverseMap();
            
        }
    }
}