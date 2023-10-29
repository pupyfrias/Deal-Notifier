using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.ItemType;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
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