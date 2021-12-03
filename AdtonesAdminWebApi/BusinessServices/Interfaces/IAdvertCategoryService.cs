using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertCategoryService
    {
        Task<ReturnResult> GetAdvertCategoryDataTable();
        Task<ReturnResult> GetAdvertCategoryDetails(int id);
        Task<ReturnResult> DeleteAdvertCategory(IdCollectionViewModel model);
        Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model);
        Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model);
    }
}
