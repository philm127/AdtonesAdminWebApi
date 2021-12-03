using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IPromotionalCampaignService
    {
        Task<ReturnResult> SavePromotionalUser(PromotionalUserFormModel model);
        Task<ReturnResult> AddPromotionalCampaign(PromotionalCampaignResult model);
        Task<ReturnResult> LoadPromoCampaignDataTable();
        Task<ReturnResult> UpdatePromotionalCampaignStatus(IdCollectionViewModel model);
        Task<ReturnResult> GetPromoBatchIdList(int operatorId);
    }
}
