using AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.Dto;
using AdtonesAdminWebApi.CrmApp.Core.Entities;
using AutoMapper;


namespace AdtonesAdminWebApi.CrmApp.Application.Users.Subscribers.MappingProfiles
{
    public class SubscriberMappingProfile : Profile
    {
        public SubscriberMappingProfile()
        {
            CreateMap<User, SubscriberDto>()
        .ForMember(dest =>
            dest.CountryName,
            opt => opt.MapFrom(src => src.Name))
        .ForMember(dest =>
            dest.Status,
            opt => opt.MapFrom(src => src.Activated))
        .ReverseMap();

        //    CreateMap<User, UserDto>()
        //.ForMember(dest =>
        //    dest.CountryName,
        //    opt => opt.MapFrom(src => src.Name))
        //.ForMember(dest =>
        //    dest.Status,
        //    opt => opt.MapFrom(src => src.Activated))
        //.ReverseMap();
        }
    }
}