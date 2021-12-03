using Domain.ViewModels;
using Domain.ViewModels.CreateUpdateCampaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ICreateUpdateCampaignService
    {
        Task<ReturnResult> CreateNewCampaign(NewCampaignProfileFormModel model);
        Task<ReturnResult> UpdateCampaignDetails(NewCampaignProfileFormModel model);
        Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model);
        
        Task<ReturnResult> CheckIfAdvertNameExists(NewAdvertFormModel model);
        Task<ReturnResult> CheckIfCampaignNameExists(NewCampaignProfileFormModel model);
        Task<ReturnResult> GetInitialData(int countryId,int advertiserId = 0);
        Task<ReturnResult> InsertProfileInformation(NewAdProfileMappingFormModel model);
        Task<ReturnResult> GetProfileData(int campaignId, int advertiserId = 0);
        Task<ReturnResult> GetCampaignData(int campaignId);
    }
}
