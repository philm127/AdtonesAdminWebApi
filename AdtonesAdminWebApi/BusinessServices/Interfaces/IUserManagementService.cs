using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IUserManagementService
    {
        Task<ReturnResult> GetUserDetail(int userId);
        Task<ReturnResult> GetContactForm(int userId);
        Task<ReturnResult> GetProfileForm(int userId);
        Task<ReturnResult> GetCompanyForm(int userId);
        Task<ReturnResult> UpdateContactForm(Contacts contact);
        Task<ReturnResult> UpdateProfileForm(User profile);
        Task<ReturnResult> UpdateCompanyDetails(CompanyDetails company);
        Task<ReturnResult> UpdateUserStatus(AdvertiserDashboardResult result);
        Task<ReturnResult> AddCompanyDetails(CompanyDetails company);
        Task<ReturnResult> AddUser(UserAddFormModel model);
        Task<ReturnResult> AddOperatorAdminUser(UserAddFormModel model);
        Task<ReturnResult> UpdateUserPermission(IdCollectionViewModel model);
    }
}
