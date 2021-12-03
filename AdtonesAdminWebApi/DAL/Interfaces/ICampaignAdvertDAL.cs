using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICampaignAdvertDAL
    {
        Task<CampaignAdvertFormModel> CreateNewCampaignAdvert(CampaignAdvertFormModel model, int operatorId, int provAdId);
        Task<CampaignAdvertFormModel> CreateOnUpdateCampaignAdvert(CampaignAdvertFormModel model, int operatorId);
        Task<int> GetAdvertIdByCampaignId(int campaignId);
        Task<int> GetCampaignIdByAdvertId(int advertId);
    }
}
