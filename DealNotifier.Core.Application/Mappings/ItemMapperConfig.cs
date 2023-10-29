using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.ViewModels.V1.Item;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappings
{
    public class ItemMapperConfig : Profile
    {
        public ItemMapperConfig()
        {
            CreateMap<ItemCreateRequest, Item>().IgnoreAllSourceNullProperties();
            CreateMap<ItemUpdateRequest, Item>().IgnoreAllSourceNullProperties();
            CreateMap<Item, ItemResponse>().IgnoreAllSourceNullProperties();

            CreateMap<Item, BanLink>()
               .ForMember(dest => dest.Link, opts => opts.MapFrom(src => src.Link))
               .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}