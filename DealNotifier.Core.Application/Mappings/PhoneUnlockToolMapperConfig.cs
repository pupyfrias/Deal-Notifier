using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.PhoneUnlockTool;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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