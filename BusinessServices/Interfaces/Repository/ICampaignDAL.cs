﻿using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface ICampaignDAL
    {
        int[] GetOperatorFromPermissionForProv();
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0);
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetProv(int operatorId, int id = 0);

        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetById(int id);
        Task<CampaignProfile> GetCampaignProfileDetail(int id = 0);
        Task<CampaignProfileUpdate> GetCampaignProfileDetailUpdate(int campaignId);
        Task<int> ChangeCampaignProfileStatus(CampaignProfile model);
        Task<int> ChangeCampaignProfileStatusOperator(CampaignProfile model);
        Task<CampaignAdverts> GetCampaignAdvertDetailsById(int adId = 0, int campId = 0);
        Task<bool> CheckCampaignBillingExists(int campaignId);

        Task<bool> CheckCampaignNameExists(string campaignName, int userId);
        Task<int> UpdateCampaignMatch(CampaignProfile model);
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetBySalesExec(int id = 0);
        Task<int> UpdateCampaignMatchesforBilling(int id = 0, string constr = null);
        Task<int> UpdateCampaignCredit(BillingPaymentModel model, string constr);
        Task<int> InsertCampaignCategory(CampaignCategoryResult model);
    }
}
