using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICampaignService
    {
        Task<ReturnResult> GetAdminOpAdminCampaignList(int id);
        Task<ReturnResult> GetAdvertiserCamapaignTable();
        Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model);
        Task<bool> ChangeCampaignStatus(int campaignId);
    }
}
