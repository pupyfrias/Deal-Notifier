using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
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