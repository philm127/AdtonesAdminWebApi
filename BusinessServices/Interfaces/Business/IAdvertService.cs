using Domain.ViewModels;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IAdvertService
    {
        Task<ReturnResult> GetAdvertCategoryDataTable();
        Task<ReturnResult> GetAdvertDataTableForSales(int id = 0);
        Task<ReturnResult> GetAdvertCategoryDetails(int id);
        Task<ReturnResult> DeleteAdvertCategory(IdCollectionViewModel model);
        Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model);
        Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model);
        Task<ReturnResult> GetAdvertDataTable(int id = 0);
        Task<ReturnResult> GetAdvertDataTableById(int id = 0);
        Task<ReturnResult> GetAdvertDetails(int id = 0);
        Task<ReturnResult> ApproveORRejectAdvert(UserAdvertResult model);

        Task<ReturnResult> GetAdvertDetailsByCampaignId(int id = 0);
    }
}
