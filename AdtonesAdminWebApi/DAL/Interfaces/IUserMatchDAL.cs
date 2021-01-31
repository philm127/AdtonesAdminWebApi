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
        Task<int> UpdateGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model, List<string> conns);
        // Task<int> InsertMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, List<string> connString);
        Task<int> UpdateMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, List<string> connString);
        // Task<int> InsertDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand model);
        Task<int> UpdateDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand mode, List<string> conns);
        //Task<int> InsertMatchCampaignDemographic(CampaignProfileDemographicsFormModel model);
        Task<int> UpdateMatchCampaignDemographic(CreateOrUpdateCampaignProfileDemographicsCommand model, List<string> connString);
        Task<CampaignProfileTimeSetting> GetCampaignTimeSettings(int campaignId);

        Task<int> InsertTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, List<string> conns);
        Task<int> UpdateTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, List<string> conns);

        // Task<int> InsertMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model);
        Task<int> UpdateMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model, List<string> conns);
        Task<int> UpdateMatchCampaignMobile(CreateOrUpdateCampaignProfileMobileCommand model, List<string> connString);


        // Task<int> InsertQuestionnaireProfile(CampaignProfileSkizaFormModel model);
        Task<int> UpdateQuestionnaireProfile(CreateOrUpdateCampaignProfileSkizaCommand model, List<string> connString);
        Task<int> UpdateMatchCampaignQuestionnaire(CreateOrUpdateCampaignProfileSkizaCommand model, List<string> connString);


        // Task<int> InsertAdvertProfile(CampaignProfileAdvertFormModel model);
        Task<int> UpdateAdvertProfile(CreateOrUpdateCampaignProfileAdvertCommand model, List<string> conns);
        Task<int> UpdateMatchCampaignAdvert(CreateOrUpdateCampaignProfileAdvertCommand model, List<string> connString);
    }
}
