using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.ItemType;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class ItemTypeMapperConfig : Profile
    {
        public ItemTypeMapperConfig()
        {
            CreateMap<ItemTypeCreateRequest, ItemType>().IgnoreAllSourceNullProperties();
            CreateMap<ItemTypeUpdateRequest, ItemType>().IgnoreAllSourceNullProperties();
            CreateMap<ItemType, ItemTypeResponse>().IgnoreAllSourceNullProperties();
        }
    }
}