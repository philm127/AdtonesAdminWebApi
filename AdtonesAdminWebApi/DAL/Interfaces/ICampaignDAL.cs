using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignDAL
    {
        Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSet(int id=0);
        
        Task<IEnumerable<CampaignCreditResult>> GetCampaignCreditResultSet();

        Task<CampaignProfile> GetCampaignProfileDetail(int id = 0);
        Task<int> ChangeCampaignProfileStatus(CampaignProfile model);
        Task<int> ChangeCampaignProfileStatusOperator(CampaignProfile model);
        Task<CampaignAdverts> GetCampaignAdvertDetailsByAdvertId(int Id);
        Task<bool> CheckCampaignBillingExists(int campaignId);


    }
}
