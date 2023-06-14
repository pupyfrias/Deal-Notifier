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

        }
    }
}