using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignMatchDAL
    {
        Task<int> UpdateMediaLocation(string conn, string media, int id);
        Task<int> AddCampaignMatchData(NewCampaignProfileFormModel model, string conn);
        Task<int> UpdateCampaignMatchData(NewCampaignProfileFormModel model, string conn);
        Task<int> UpdateMatchCampaignGeographic(CreateOrUpdateCampaignProfileGeographicCommand model, string constr);
        Task<int> UpdateMatchCampaignDemographic(CreateOrUpdateCampaignProfileDemographicsCommand model, string constr);
        Task<int> UpdateMatchCampaignMobile(CreateOrUpdateCampaignProfileMobileCommand model, string constr);
        Task<int> UpdateMatchCampaignQuestionnaire(CreateOrUpdateCampaignProfileSkizaCommand model, string constr);
        Task<int> UpdateMatchCampaignAdvert(CreateOrUpdateCampaignProfileAdvertCommand model, string constr);
        Task<int> UpdateCampaignMatch(int campaignProfileId, int operatorId, int status);
        Task<int> UpdateCampaignMatchCredit(CampaignCreditCommand model, string constr);
    }
}
