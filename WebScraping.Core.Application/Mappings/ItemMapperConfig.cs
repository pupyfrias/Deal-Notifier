using AutoMapper;
using WebScraping.Core.Application.Dtos.Item;
using WebScraping.Core.Domain.Entities;

namespace WebScraping.Core.Application.Mappings
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

        }
    }
}