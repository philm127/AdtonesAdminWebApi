using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertCategoryService
    {
        Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model);
    }
}
