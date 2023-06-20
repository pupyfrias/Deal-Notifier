using AutoMapper;
using DealNotifier.Core.Application.DTOs.Item;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class ItemMapperConfig : Profile
    {
        public ItemMapperConfig()
        {
            CreateMap<ItemCreateDto, Item>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ItemReadDto, Item>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ItemUpdateDto, Item>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<ItemResponseDto, Item>().ReverseMap();
            CreateMap<Item, BlackList>()
               .ForMember(dest => dest.Link, opts => opts.MapFrom(src => src.Link))
               .ForMember(dest => dest.Id, opt => opt.Ignore());

        }
    }
}