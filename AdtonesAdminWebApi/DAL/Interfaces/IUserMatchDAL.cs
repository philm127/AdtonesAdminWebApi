using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using AdtonesAdminWebApi.ViewModels.DTOs.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserMatchDAL
    {
        
        
        
        Task PrematchProcessForCampaign(int campaignId, string conn);
        Task<CampaignBudgetModel> GetBudgetAmounts(int campaignId, string conn);
        Task<int> UpdateBucketCount(int campaignId, string conn, int bucketCount);
        Task<int> GetProfileMatchInformationId(int countryId, string profileName);

        //Task<IEnumerable<ProfileMatchInfoDto>> GetProfileMatchInformation(int countryId);
        Task<IEnumerable<string>> GetProfileMatchLabels(int infoId);
        Task<CampaignProfilePreference> GetCampaignProfilePreference();
        Task<CampaignProfilePreference> GetCampaignProfilePreferenceDetailsByCampaignId(int campaignId);
        Task<int> GetCampaignProfilePreferenceId(int campaignId);
        Task<int> InsertProfilePreference(NewAdProfileMappingFormModel model);
        Task<int> UpdateGeographicProfile(CreateOrUpdateCampaignProfileGeographicCommand model, string constr);

        Task<int> UpdateDemographicProfile(CreateOrUpdateCampaignProfileDemographicsCommand mode, string constr);
        
        Task<CampaignProfileTimeSetting> GetCampaignTimeSettings(int campaignId);

        Task<int> InsertTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, string constr);
        Task<int> UpdateTimeSettingsProfile(CampaignProfileTimeSetting timeSettings, string constr);

        Task<int> UpdateMobileProfile(CreateOrUpdateCampaignProfileMobileCommand model, string constr);
        Task<int> UpdateQuestionnaireProfile(CreateOrUpdateCampaignProfileSkizaCommand model, string constr);
        Task<int> UpdateAdvertProfile(CreateOrUpdateCampaignProfileAdvertCommand model, string constr);
        
    }
}
