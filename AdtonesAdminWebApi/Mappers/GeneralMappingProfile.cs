using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using AutoMapper;

namespace AdtonesAdminWebApi.Mappers
{
    public class GeneralMappingProfile : Profile
    {
        public GeneralMappingProfile()
        {
            CreateMap<UserCreditDetailsDto, AdvertiserCreditFormCommand>();
            
            CreateMap<UserPaymentCommand, CampaignCreditCommand>()
                .ForMember(dest =>
                    dest.TotalBudget,
                    opt => opt.MapFrom(src => src.Fundamount))
                .ForMember(dest =>
                    dest.TotalCredit,
                    opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest =>
                    dest.UserId,
                    opt => opt.MapFrom(src => src.AdvertiserId));

            CreateMap<CreditCardPaymentCommand, UserPaymentCommand>();
        }
    }
}
