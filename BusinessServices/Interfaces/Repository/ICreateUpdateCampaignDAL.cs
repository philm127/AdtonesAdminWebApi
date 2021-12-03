using Domain.ViewModels;
using Domain.ViewModels.CreateUpdateCampaign;
using Domain.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface ICreateUpdateCampaignDAL
    {
        Task<CampaignProfileTimeSetting> GetProfileTimeSettingsByCampId(int id);
        // Task<int> AddProfileTimeSettings(CampaignProfileTimeSetting model, int countryId, int provCampaignId);

        Task<NewCampaignProfileFormModel> CreateNewCampaign(NewCampaignProfileFormModel model);
        Task<int> UpdateCampaignDetails(NewCampaignProfileFormModel model);
        Task<NewAdvertFormModel> CreateNewCampaignAdvert(NewAdvertFormModel model);
        Task<CampaignAdvertFormModel> CreateNewIntoCampaignAdverts(CampaignAdvertFormModel model, int operatorId, int provAdId);

        Task<int> InsertNewClient(ClientViewModel model);
        Task<ClientViewModel> GetClientDetails(int clientId);
    }
}
