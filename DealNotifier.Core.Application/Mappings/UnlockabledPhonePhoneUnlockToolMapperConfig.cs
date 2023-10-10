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
            CreateMap<UnlockabledPhonePhoneUnlockToolCreate, UnlockabledPhonePhoneUnlockTool>().IgnoreAllSourceNullProperties();
            
        }
    }
}