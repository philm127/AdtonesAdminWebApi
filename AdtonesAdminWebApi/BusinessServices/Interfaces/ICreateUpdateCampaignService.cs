using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICreateUpdateCampaignService
    {
        Task<ReturnResult> CreateNewCampaign(NewCampaignProfileFormModel model);
        Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model);
    }
}
