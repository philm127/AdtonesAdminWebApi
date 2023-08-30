using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IUserManagementAddUserService
    {
        Task<ReturnResult> AddCompanyDetails(CompanyDetails company);
        Task<ReturnResult> AddUser(UserAddFormModel model, bool isNewRegister = false);
        Task<ReturnResult> AddOperatorAdminUser(UserAddFormModel model);
    }
}
