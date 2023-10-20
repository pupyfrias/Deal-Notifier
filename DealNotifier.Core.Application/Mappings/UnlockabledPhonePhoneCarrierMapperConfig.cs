﻿using AutoMapper;
using DealNotifier.Core.Application.Extensions;
using DealNotifier.Core.Application.ViewModels.V1;
using DealNotifier.Core.Application.ViewModels.V1.UnlockabledPhonePhoneCarrier;
using DealNotifier.Core.Domain.Entities;

namespace DealNotifier.Core.Application.Mappings
{
    public class UnlockabledPhonePhoneCarrierMapperConfig : Profile
    {
        public UnlockabledPhonePhoneCarrierMapperConfig()
        {
            CreateMap<UnlockabledPhonePhoneCarrierCreate, UnlockabledPhonePhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhonePhoneCarrierUpdateRequest, UnlockabledPhonePhoneCarrier>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhonePhoneCarrier, UnlockabledPhonePhoneCarrierResponse>().IgnoreAllSourceNullProperties();
            CreateMap<UnlockabledPhonePhoneCarrierDto, UnlockabledPhonePhoneCarrierCreate>().IgnoreAllSourceNullProperties().ReverseMap();
            CreateMap<UnlockabledPhonePhoneCarrierDto, UnlockabledPhonePhoneCarrier>().IgnoreAllSourceNullProperties().ReverseMap();
        }
    }
}