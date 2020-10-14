using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertService
    {
        Task<ReturnResult> LoadAdvertCategoryDataTable();
        Task<ReturnResult> LoadAdvertDataTableForSales(int id = 0);
        Task<ReturnResult> GetAdvertCategoryDetails(int id);
        Task<ReturnResult> DeleteAdvertCategory(IdCollectionViewModel model);
        Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model);
        Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model);
        Task<ReturnResult> LoadAdvertDataTable(int id = 0);
        Task<ReturnResult> LoadAdvertDataTableById(int id = 0);
        Task<ReturnResult> LoadAdvertDetails(int id = 0);
        Task<ReturnResult> ApproveORRejectAdvert(UserAdvertResult model);
    }
}
