using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertService
    {
        Task<ReturnResult> LoadAdvertCategoryDataTable();
        Task<ReturnResult> LoadAdvertDataTable(int id = 0);
    }
}
