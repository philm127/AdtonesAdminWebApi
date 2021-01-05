using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using AutoMapper;

namespace AdtonesAdminWebApi.Mappers
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainModelMappings"; }
        }

        public ViewModelToDomainMappingProfile()
        {
            CreateMap<CampaignProfileDemographicsFormModel, CreateOrUpdateCampaignProfileDemographicsCommand>();
            CreateMap<CampaignProfileGeographicFormModel, CreateOrUpdateCampaignProfileGeographicCommand>();
            CreateMap<CampaignProfileTimeSettingFormModel, CreateOrUpdateCampaignProfileTimeSettingCommand>();
            CreateMap<CampaignProfileMobileFormModel, CreateOrUpdateCampaignProfileMobileCommand>();
        }
    }
}
