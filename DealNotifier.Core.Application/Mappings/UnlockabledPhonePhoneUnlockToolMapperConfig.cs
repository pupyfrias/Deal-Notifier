using AutoMapper;
using Catalog.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class UnlockabledPhonePhoneUnlockToolMapperConfig : Profile
    {
        public UnlockabledPhonePhoneUnlockToolMapperConfig()
        {

            CreateMap<UnlockabledPhonePhoneUnlockTool, UnlockabledPhonePhoneUnlockToolCreate>().ReverseMap();
            CreateMap<UnlockabledPhonePhoneUnlockToolDto, UnlockabledPhonePhoneUnlockToolCreate>().ReverseMap();
            CreateMap<UnlockabledPhonePhoneUnlockToolDto, UnlockabledPhonePhoneUnlockTool>().ReverseMap();
        }
    }
}