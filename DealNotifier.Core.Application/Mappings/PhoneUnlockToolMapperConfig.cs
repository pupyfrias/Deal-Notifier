using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Domain.Entities;
using WebApi.Controllers.V1;

namespace DealNotifier.Core.Application.Mappings
{
    public class PhoneUnlockToolMapperConfig : Profile
    {
        public PhoneUnlockToolMapperConfig()
        {
            CreateMap<PhoneUnlockToolCreateRequest, PhoneUnlockTool>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneUnlockToolUpdateRequest, PhoneUnlockTool>().IgnoreAllSourceNullProperties();
            CreateMap<PhoneUnlockTool, PhoneUnlockToolResponse>().IgnoreAllSourceNullProperties();
        }
    }
}