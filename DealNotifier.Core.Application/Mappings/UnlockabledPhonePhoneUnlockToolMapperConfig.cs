using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneUnlockTool;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
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