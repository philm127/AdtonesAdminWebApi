using AdtonesAdminWebApi.CrmApp.Application.Users.Users.Dto;
using AdtonesAdminWebApi.CrmApp.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.CrmApp.Application.Users.Users.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
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