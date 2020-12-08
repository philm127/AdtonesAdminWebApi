using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
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
    }
}
