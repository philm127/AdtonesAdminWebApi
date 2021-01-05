using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserMatchDAL
    {
        Task<int> AddCampaignData(NewCampaignProfileFormModel model, string conn);
        Task<int> UpdateMediaLocation(string conn, string media, int id);
        Task PrematchProcessForCampaign(int campaignId, string conn);
        Task<CampaignBudgetModel> GetBudgetAmounts(int campaignId, string conn);
        Task<int> UpdateBucketCount(int campaignId, string conn, int bucketCount);
        Task<int> GetProfileMatchInformationId(int countryId, string profileName);
        Task<IEnumerable<string>> GetProfileMatchLabels(int infoId);
        Task<CampaignProfilePreference> GetCampaignProfilePreference();
        Task<CampaignProfilePreference> GetCampaignProfilePreferenceDetailsByCampaignId(int campaignId);
        Task<int> GetCampaignProfilePreferenceId(int campaignId);
        Task<int> InsertGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model);
        Task<int> UpdateGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model);
        Task<int> InsertMatchCampaignGeographic(CampaignProfileGeographicFormModel model);
        Task<int> InsertDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model);
        Task<int> UpdateDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand mode);
        Task<int> InsertMatchCampaignDemographic(CampaignProfileDemographicsFormModel model);
        Task<int> UpdateMatchCampaignDemographic(CampaignProfileDemographicsFormModel model);
        Task<CampaignProfileTimeSetting> GetCampaignTimeSettings(int campaignId);

        Task<int> InsertTimeSettingsProfile(CampaignProfileTimeSetting timeSettings);
        Task<int> UpdateTimeSettingsProfile(CampaignProfileTimeSetting timeSettings);

        Task<int> InsertMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model);
        Task<int> UpdateMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model);
        Task<int> UpdateMatchCampaignMobile(CampaignProfileMobileFormModel model);


        Task<int> InsertQuestionnaireProfile(CampaignProfileSkizaFormModel model);
        Task<int> UpdateQuestionnaireProfile(CampaignProfileSkizaFormModel model);
        Task<int> UpdateMatchCampaignQuestionnaire(CampaignProfileSkizaFormModel model);


        Task<int> InsertAdvertProfile(CampaignProfileAdvertFormModel model);
        Task<int> UpdateAdvertProfile(CampaignProfileAdvertFormModel model);
        Task<int> UpdateMatchCampaignAdvert(CampaignProfileAdvertFormModel model);
    }
}
