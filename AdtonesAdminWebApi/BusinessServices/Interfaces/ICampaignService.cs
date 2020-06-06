using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICampaignService
    {
        Task<ReturnResult> LoadCampaignCreditDataTable();
        Task<ReturnResult> LoadPromoCampaignDataTable();
        Task<ReturnResult> LoadCampaignDataTable(int id=0);
        Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model);
        Task<bool> ChangeCampaignStatus(int campaignId);
    }
}
