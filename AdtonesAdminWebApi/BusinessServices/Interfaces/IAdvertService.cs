using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertService
    {
        Task<ReturnResult> GetAdvertDataTableForSales(int id = 0);
        Task<ReturnResult> GetAdvertDataTableForAdvertiser(int id);
        Task<ReturnResult> GetAdvertDataTableForAdvertiserSummary(int id);
        Task<ReturnResult> GetAdvertDataTable(int id = 0);
        Task<ReturnResult> GetAdvertDataTableById(int id = 0);
        Task<ReturnResult> GetAdvertDetails(int id = 0);
        Task<ReturnResult> CheckIfAdvertNameExists(NewAdvertFormModel model);
        Task<ReturnResult> CreateNewCampaign_Advert(NewAdvertFormModel model);
        Task<ReturnResult> UpdateAdvert(NewAdvertFormModel model);
        Task<ReturnResult> ApproveORRejectAdvert(UserAdvertResult model);

        Task<ReturnResult> GetAdvertDetailsByCampaignId(int id = 0);
    }
}
