using Domain.ViewModels;
using Domain.ViewModels.CreateUpdateCampaign;
using Domain.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository
{
    public interface ICampaignMatchesRepository
    {
        Task<int> AddCampaignMatchData(NewCampaignProfileFormModel model, string conn);
        Task<int> UpdateCampaignMatchData(NewCampaignProfileFormModel model, string conn);
        Task<int> UpdateMediaLocation(string conn, string media, int id);
        Task PrematchProcessForCampaign(int campaignId, string conn);
        Task<CampaignBudgetModel> GetBudgetAmounts(int campaignId, string conn);
        Task<int> UpdateBucketCount(int campaignId, string conn, int bucketCount);
        Task<int> GetProfileMatchInformationId(int countryId, string profileName);
        Task<IEnumerable<string>> GetProfileMatchLabels(int infoId);
        Task<CampaignProfilePreference> GetCampaignProfilePreference();
        Task<CampaignProfilePreference> GetCampaignProfilePreferenceDetailsByCampaignId(int campaignId);
        Task<int> GetCampaignProfilePreferenceId(int campaignId);
        Task<int> InsertProfilePreference(NewAdProfileMappingFormModel model);
        // Task<int> InsertGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model);
        Task<int> UpdateGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model, string constr);
        // Task<int> InsertMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, string constr);
        Task<int> UpdateMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, string constr);
        // Task<int> InsertDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model);
        Task<int> UpdateDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand mode, string constr);
        //Task<int> InsertMatchCampaignDemographic(CampaignProfileDemographicsFormModel model);
        Task<int> UpdateMatchCampaignDemographic(CreateOrUpdateCampaignProfileDemographicsCommand model, string constr);
        Task<CampaignProfileTimeSetting> GetCampaignTimeSettings(int campaignId);

        Task<int> InsertTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, string constr);
        Task<int> UpdateTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, string constr);

        // Task<int> InsertMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model);
        Task<int> UpdateMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model, string constr);
        Task<int> UpdateMatchCampaignMobile(CreateOrUpdateCampaignProfileMobileCommand model, string constr);


        // Task<int> InsertQuestionnaireProfile(CampaignProfileSkizaFormModel model);
        Task<int> UpdateQuestionnaireProfile(CreateOrUpdateCampaignProfileSkizaCommand model, string constr);
        Task<int> UpdateMatchCampaignQuestionnaire(CreateOrUpdateCampaignProfileSkizaCommand model, string constr);


        // Task<int> InsertAdvertProfile(CampaignProfileAdvertFormModel model);
        Task<int> UpdateAdvertProfile(CreateOrUpdateCampaignProfileAdvertCommand model, string constr);
        Task<int> UpdateMatchCampaignAdvert(CreateOrUpdateCampaignProfileAdvertCommand model, string constr);
    }
}
