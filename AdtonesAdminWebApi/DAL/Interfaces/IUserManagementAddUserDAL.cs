using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserManagementAddUserDAL
    {
        Task<bool> CheckIfUserExists(UserAddFormModel model);
        Task<bool> VerifyEmail(string code);
        Task<bool> CheckIfContactExists(Contacts model);
        Task<int> AddNewUser(UserAddFormModel model);
        Task<int> AddNewUserToOperator(UserAddFormModel model);
        Task<int> AddNewContact(Contacts model);
        Task<int> AddNewContactToOperator(Contacts model);
        Task<int> DeleteNewUser(int userId);
        Task<int> InsertManagerToSalesExec(int manId, int execId);
        Task<int> AddCompanyDetails(CompanyDetails company);
        Task<int> AddCompanyDetailsToOperator(CompanyDetails company);

        Task<int> RollbackUserDetails(int userId);
        Task<int> RollbackRemoteUserDetails(int userId, int contactId);
    }
}
