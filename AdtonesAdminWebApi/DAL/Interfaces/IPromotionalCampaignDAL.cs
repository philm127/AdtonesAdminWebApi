using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IPromotionalCampaignDAL
    {
        Task<IEnumerable<string>> GetMsisdnCheckForExisting(int operatorId);
        Task<bool> GetPromoUserBatchIdCheckForExisting(PromotionalUserFormModel model);
        Task<int> UpdatePromotionalCampaignStatus(IdCollectionViewModel model);
        Task<IEnumerable<PromotionalCampaignResult>> GetPromoCampaignResultSet();
        Task<bool> GetPromoCampaignBatchIdCheckForExisting(PromotionalCampaignResult model);
        Task<int> AddPromotionalCampaign(PromotionalCampaignResult model);
        Task<int> AddPromotionalCampaignToOperator(PromotionalCampaignResult model);
        Task<int> AddPromotionalAdvertToOperator(PromotionalCampaignResult model);
        Task<int> AddPromotionalAdvert(PromotionalCampaignResult model);
        Task<IEnumerable<SharedSelectListViewModel>> GetPromoBatchIdList(int id);
    }
}
