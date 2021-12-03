using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignDAL
    {
        int[] GetOperatorFromPermissionForProv();
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0);
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetProv(int operatorId, int id = 0);
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetById(int id);
        // Task<CampaignProfile> GetCampaignProfileDetail(int id = 0);
        Task<CampaignProfileDto> GetCampaignProfileDetail(int campaignId);
        Task<int> ChangeCampaignProfileStatus(CampaignProfileDto model);
        Task<int> ChangeCampaignProfileStatusOperator(CampaignProfileDto model);
        Task<CampaignAdverts> GetCampaignAdvertDetailsById(int adId = 0, int campId = 0);
        Task<bool> CheckCampaignBillingExists(int campaignId);

        Task<bool> CheckCampaignNameExists(string campaignName, int userId);
        // Task<int> UpdateCampaignMatch(CampaignProfile model);
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetBySalesExec(int id = 0);
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetByAdvertiser(int id);

        Task<int> UpdateCampaignCredit(CampaignCreditCommand model, string constr);
        Task<int> InsertCampaignCategory(CampaignCategoryResult model);
    }
}
