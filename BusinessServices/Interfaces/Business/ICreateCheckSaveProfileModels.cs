﻿using Domain.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ICreateCheckSaveProfileModels
    {
        
        CampaignProfileGeographicFormModel GetGeographicModel(int countryId, int profileId = 0);
        Task<CampaignProfileGeographicFormModel> GetGeographicData(int profileId, CampaignProfilePreference CampaignProfileGeograph);
        Task<bool> SaveGeographicWizard(CampaignProfileGeographicFormModel model, string constr);


        CampaignProfileDemographicsFormModel GetDemographicModel(int countryId, int profileId = 0);
        Task<CampaignProfileDemographicsFormModel> GetDemographicData(int profileId, CampaignProfilePreference CampaignProfileDemograph);
        Task<bool> SaveDemographicsWizard(CampaignProfileDemographicsFormModel model, string constr);

        
        CampaignProfileTimeSettingFormModel GetTimeSettingModel(int Id = 0);
        Task<CampaignProfileTimeSettingFormModel> GetTimeSettingData(int campaignId);
        Task<bool> SaveTimeSettingsWizard(CampaignProfileTimeSettingFormModel model, string constr);



        Task<CampaignProfileMobileFormModel> GetMobileModel(int countryId, int profileId = 0);
        Task<CampaignProfileMobileFormModel> GetMobileData(int profileId, CampaignProfilePreference CampaignProfileMobile);
        Task<bool> SaveMobileWizard(CampaignProfileMobileFormModel model, string constr);


        Task<CampaignProfileSkizaFormModel> GetQuestionnaireModel(int countryId, int profileId = 0);
        Task<CampaignProfileSkizaFormModel> GetQuestionnaireData(int profileId, CampaignProfilePreference CampaignProfileSkiza);
        Task<bool> SaveQuestionnaireWizard(CampaignProfileSkizaFormModel model, string constr);


        Task<CampaignProfileAdvertFormModel> GetAdvertProfileModel(int countryId, int profileId = 0);
        Task<CampaignProfileAdvertFormModel> GetAdvertProfileData(int profileId, CampaignProfilePreference CampaignProfileAdvert);
        Task<bool> SaveAdvertsWizard(CampaignProfileAdvertFormModel model, string constr);

        // IList<TimeOfDay> GetTimes();
    }
}
