using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICreateCheckSaveProfileModels
    {
        CampaignProfileDemographicsFormModel GetDemographicModel(int countryId, int profileId = 0);
        CampaignProfileGeographicFormModel GetGeographicModel(int countryId, int profileId = 0);

        CampaignProfileTimeSettingFormModel GetTimeSettingModel(int Id = 0);
        Task<CampaignProfileMobileFormModel> GetMobileModel(int countryId, int profileId = 0);
        Task<bool> SaveDemographicsWizard(CampaignProfileDemographicsFormModel model);
        Task<bool> SaveGeographicWizard(CampaignProfileGeographicFormModel model);

        Task<bool> SaveQuestionnaireWizard(CampaignProfileSkizaFormModel model);

        Task<CampaignProfileGeographicFormModel> GetGeographicData(int profileId, CampaignProfilePreference CampaignProfileGeograph);
        Task<CampaignProfileDemographicsFormModel> GetDemographicData(int profileId, CampaignProfilePreference CampaignProfileDemograph);
        Task<CampaignProfileTimeSettingFormModel> GetTimeSettingData(int campaignId);
        Task<CampaignProfileMobileFormModel> GetMobileData(int profileId, CampaignProfilePreference CampaignProfileMobile);

        Task<CampaignProfileSkizaFormModel> GetQuestionnaireModel(int countryId, int profileId = 0);
        Task<CampaignProfileSkizaFormModel> GetQuestionnaireData(int profileId, CampaignProfilePreference CampaignProfileSkiza);

        Task<CampaignProfileAdvertFormModel> GetAdvertProfileModel(int countryId, int profileId = 0);
        Task<CampaignProfileAdvertFormModel> GetAdvertProfileData(int profileId, CampaignProfilePreference CampaignProfileAdvert);

        // IList<TimeOfDay> GetTimes();
    }
}
