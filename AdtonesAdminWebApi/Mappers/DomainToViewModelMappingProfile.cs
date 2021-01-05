using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using AutoMapper;

namespace AdtonesAdminWebApi.Mappers
{
    public class DomainToViewModelMappingProfile : Profile
    {
        /// <summary>
        /// Gets the name of the profile.
        /// </summary>
        /// <value>The name of the profile.</value>
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        public DomainToViewModelMappingProfile()
        {
            CreateMap<CampaignProfilePreference, CampaignProfileDemographicsFormModel>();
            CreateMap<CampaignProfilePreference, CampaignProfileGeographicFormModel>();
        }

        /// <summary>
        /// Configures this instance.
        /// </summary>
        //protected override void Configure()
        //{
            //            Mapper.CreateMap<Category, CategoryFormModel>();
            //            Mapper.CreateMap<Expense, ExpenseFormModel>().ForMember(dest => dest.Category, opt => opt.Ignore());
            //Mapper.CreateMap<UserProfile, UserProfileFormModel>()
            //    .ForMember(dest => dest.UserProfileAdverts, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileCinemas, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileInternets, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileMobiles, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfilePresses, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileProductsServices, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileRadios, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileTimeSettings, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileTvs, src => src.UseDestinationValue());

            //Mapper.CreateMap<UserProfileAdvert, UserProfileAdvertFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileAdvertFormModel>();
            //Mapper.CreateMap<UserProfileAttitude, UserProfileAttitudeFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileAttitudeFormModel>();
            //Mapper.CreateMap<UserProfileCinema, UserProfileCinemaFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileCinemaFormModel>();
            //Mapper.CreateMap<UserProfileInternet, UserProfileInternetFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileInternetFormModel>();
            //Mapper.CreateMap<UserProfileMobile, UserProfileMobileFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileMobileFormModel>();
            //Mapper.CreateMap<UserProfilePress, UserProfilePressFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfilePressFormModel>();
            //Mapper.CreateMap<UserProfileProductsService, UserProfileProductsServiceFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileProductsServiceFormModel>();
            //Mapper.CreateMap<UserProfileRadio, UserProfileRadioFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileRadioFormModel>();
            //Mapper.CreateMap<UserProfileTimeSetting, UserProfileTimeSettingFormModel>();
            //Mapper.CreateMap<UserProfileTv, UserProfileTvFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileTvFormModel>();
            //Mapper.CreateMap<UserProfileCreditsReceived, UserProfileCreditsReceivedFormModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<BlockedNumber, BlockedNumberFormModel>();
            //Mapper.CreateMap<Contacts, ContactsFormModel>();
            //Mapper.CreateMap<CompanyDetails, CompanyDetailsFormModel>();

            //Mapper.CreateMap<User, UserFormModel>();

            //Mapper.CreateMap<CampaignProfile, CampaignProfileFormModel>();




            //Mapper.CreateMap<Client, ClientModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileAdvertFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileSkizaFormModel>();
            //Mapper.CreateMap<CampaignProfileAdvert, CampaignProfileAdvertFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileGeographicFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileAttitudeFormModel>();
            //Mapper.CreateMap<CampaignProfileAttitude, CampaignProfileAttitudeFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileCinemaFormModel>();
            //Mapper.CreateMap<CampaignProfileCinema, CampaignProfileCinemaFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileInternetFormModel>();
            //Mapper.CreateMap<CampaignProfileInternet, CampaignProfileInternetFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileMobileFormModel>();
            //Mapper.CreateMap<CampaignProfileMobile, CampaignProfileMobileFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfilePressFormModel>();
            //Mapper.CreateMap<CampaignProfilePress, CampaignProfilePressFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileProductsServiceFormModel>();
            //Mapper.CreateMap<CampaignProfileProductsService, CampaignProfileProductsServiceFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileRadioFormModel>();
            //Mapper.CreateMap<CampaignProfileRadio, CampaignProfileRadioFormModel>();
            //Mapper.CreateMap<CampaignProfileTimeSetting, CampaignProfileTimeSettingFormModel>();
            //Mapper.CreateMap<CampaignProfilePreference, CampaignProfileTvFormModel>();
            //Mapper.CreateMap<CampaignProfileTv, CampaignProfileTvFormModel>();
            //Mapper.CreateMap<CampaignProfileDemographics, CampaignProfileDemographicsFormModel>();
            //CreateMap<CampaignProfilePreference, CampaignProfileDemographicsFormModel>();
            //Mapper.CreateMap<CampaignAudit, CampaignAuditFormModel>();

            //Mapper.CreateMap<Billing, BillingFormModel>();
            //Mapper.CreateMap<Billing, SagepayFormModel>();
            //Mapper.CreateMap<Billing, MpesaFormModel>();

            //Mapper.CreateMap<BillingDetails, BillingDetailsFormModel>();

            //Mapper.CreateMap<PaymentMethod, PaymentMethodFormModel>();

            //Mapper.CreateMap<CampaignAdvert, CampaignAdvertFormModel>();

            //Mapper.CreateMap<UsersCredit, UsersCreditFormModel>();

            //Mapper.CreateMap<UsersCreditPayment, UsersCreditPaymentFormModel>();

            //Mapper.CreateMap<Country, CountryFormModel>();
            //Mapper.CreateMap<CountryTax, CountryTaxFormModel>();

            //Mapper.CreateMap<Question, QuestionFormModel>();
            //Mapper.CreateMap<QuestionComment, QuestionCommentFormModel>();
            //Mapper.CreateMap<QuestionCommentImages, QuestionCommentImagesFormModel>();
            //Mapper.CreateMap<QuestionImages, QuestionImagesFormModel>();
            ////                .ForMember(dest => dest.Advert, src => src.UseDestinationValue())
            ////                .ForMember(dest => dest.CampaignProfile, src => src.UseDestinationValue());

            //Mapper.CreateMap<Advert, AdvertFormModel>();
            //Mapper.CreateMap<Advert, AdminAdvertFormModel>();
            ////                .ForMember(dest => dest.CampaignAdverts, src => src.UseDestinationValue());

            //Mapper.CreateMap<UserProfileFormModel, CreateOrUpdateProfileCommand>()
            //    .ForMember(dest => dest.UserProfileAttitudes, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileAdverts, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileCinemas, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileInternets, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileMobiles, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfilePresses, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileProductsServices, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileRadios, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileTimeSettings, src => src.UseDestinationValue())
            //    .ForMember(dest => dest.UserProfileTvs, src => src.UseDestinationValue());

            //Mapper.CreateMap<Operator, OperatorFormModel>();
            //Mapper.CreateMap<Area, AreaFormModel>();

            //Mapper.CreateMap<UserProfileAdvertsReceived2, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived3, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived4, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived5, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived6, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived7, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived8, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived9, UserProfileAdvertsReceivedFromModel>();
            //Mapper.CreateMap<UserProfileAdvertsReceived10, UserProfileAdvertsReceivedFromModel>();

            //Mapper.CreateMap<ProfileMatchInformation, ProfileMatchInformationFormModel>();

            //Mapper.CreateMap<UserProfilePreference, SkizaProfileFormModel>();
            //Mapper.CreateMap<Currency, CurrencyFormModel>();
            //Mapper.CreateMap<CampaignMetrics, CampaignDashboardChartResult>();

            //Mapper.CreateMap<ProfileMatchLabel, ProfileMatchLabelFormModel>();
            //Mapper.CreateMap<Reward, RewardFormModel>()
            //    .ForMember(dest => dest.Id, src => src.MapFrom(top => top.RewardId))
            //    .ForMember(dest => dest.Name, src => src.MapFrom(top => top.RewardName))
            //    .ForMember(dest => dest.Value, src => src.MapFrom(top => top.RewardValue))
            //    .ForMember(dest => dest.CreatedDate, src => src.MapFrom(top => top.AddedDate))
            //    .ForMember(dest => dest.UpdatedDate, src => src.MapFrom(top => top.UpdatedDate));

            //Mapper.CreateMap<UserProfileMobile, UserProfileDemographicFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileDemographicFormModel>();

            //Mapper.CreateMap<UserProfilePreference, UserProfileAdvertAdvertiserFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileSkizaProfileAdvertiserFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileMobileAdvertiserFormModel>();
            //Mapper.CreateMap<UserProfilePreference, UserProfileDemographicAdvertiserFormModel>();
            //Mapper.CreateMap<OperatorMaxAdvertsFormModel, CreateOrUpdateOperatorMaxAdvertCommand>();
            //Mapper.CreateMap<OperatorMaxAdvert, OperatorMaxAdvertsFormModel>()
            //    .ForMember(dest => dest.OperatorName, src => src.MapFrom(top => top.Operator.OperatorName))
            //    .ReverseMap();
            //Mapper.CreateMap<AdvertCategory, AdvertCategoryFormModel>();
            //Mapper.CreateMap<CopyRight, CopyRightFormModel>()
            //    .ForMember(dest => dest.Id, src => src.MapFrom(top => top.CopyRightId))
            //    .ForMember(dest => dest.Text, src => src.MapFrom(top => top.CopyRightText))
            //    .ForMember(dest => dest.Active, src => src.MapFrom(top => top.Active))
            //    .ForMember(dest => dest.UpdatedDate, src => src.MapFrom(top => top.UpdatedDate));
            //Mapper.CreateMap<OperatorConfigurationFormModel, CreateOrUpdateOperatorConfigurationCommand>();
            //Mapper.CreateMap<OperatorConfiguration, OperatorConfigurationFormModel>()
            //    .ForMember(dest => dest.OperatorName, src => src.MapFrom(top => top.Operator.OperatorName))
            //    .ReverseMap();
            //Mapper.CreateMap<PromotionalCampaign, CreateOrUpdatePromotionalCampaignCommand>().ReverseMap();
            //Mapper.CreateMap<PromotionalCampaign, ChangePromotionalCampaignStatusCommand>().ReverseMap();
            //Mapper.CreateMap<User, UserResult>()
            //    .ForMember(dest => dest.CountryId, src => src.MapFrom(top => top.Operator.CountryId));
       // }
    }
}