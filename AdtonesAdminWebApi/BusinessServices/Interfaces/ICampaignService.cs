using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICampaignService
    {
        Task<ReturnResult> GetAdminOpAdminCampaignList();
        Task<ReturnResult> GetAdvertiserCamapaignTable();
        Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model);
        Task<bool> ChangeCampaignStatus(IdCollectionViewModel model);
        Task<ReturnResult> GetSalesCampaignList(int id);
        Task<ReturnResult> GetCampaignById(int id);
    }
}
