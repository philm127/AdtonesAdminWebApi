using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign
{
    public class NewAdProfileMappingFormModel
    {
        public CampaignProfileDemographicsFormModel CampaignProfileDemographicsmodel { get; set; }
        public CampaignProfileAdvertFormModel CampaignProfileAd { get; set; }
        public CampaignProfileMobileFormModel CampaignProfileMobileFormModel { get; set; }

        public CampaignProfileGeographicFormModel CampaignProfileGeographicModel { get; set; }
        public CampaignProfileTimeSettingFormModel CampaignProfileTimeSettingModel { get; set; }

        public CampaignProfileSkizaFormModel CampaignProfileSkizaFormModel { get; set; }
    }
}
