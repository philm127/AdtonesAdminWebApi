using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CRM.Models.Subscriber
{
    public class SubscriberProfile : Profile
    {
        public SubscriberProfile()
        {
            CreateMap<SubscriberListModel, SubscriberListDto>()
        .ForMember(dest =>
            dest.CountryName,
            opt => opt.MapFrom(src => src.Name))
        .ForMember(dest =>
            dest.Status,
            opt => opt.MapFrom(src => src.Activated))
        .ReverseMap();

            CreateMap<SubscriberModel, SubscriberDto>()
        .ForMember(dest =>
            dest.CountryName,
            opt => opt.MapFrom(src => src.Name))
        .ForMember(dest =>
            dest.Status,
            opt => opt.MapFrom(src => src.Activated))
        .ReverseMap();
        }
    }
}
