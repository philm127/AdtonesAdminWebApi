using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertService
    {
        Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model);
        Task<ReturnResult> UpdateAdvert(NewAdvertFormModel model);
        Task<ReturnResult> ApproveORRejectAdvert(UserAdvertResult model);
    }
}
