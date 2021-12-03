using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign.ProfileModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICreateUpdateCampaignDAL
    {
        Task<CampaignProfileTimeSetting> GetProfileTimeSettingsByCampId(int id);
        // Task<int> AddProfileTimeSettings(CampaignProfileTimeSetting model, int countryId, int provCampaignId);

        Task<NewCampaignProfileFormModel> CreateNewCampaign(NewCampaignProfileFormModel model);
        Task<int> UpdateCampaignDetails(NewCampaignProfileFormModel model);

        Task<int> InsertNewClient(ClientViewModel model);
        Task<ClientViewModel> GetClientDetails(int clientId);
    }
}
