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
        Task<ReturnResult>LoadDataTable();
        Task<ReturnResult> GetContactForm(int userId);
        Task<ReturnResult> GetProfileForm(int userId);
        Task<ReturnResult> GetCompanyForm(int userId);
        Task<ReturnResult> UpdateContactForm(Contacts contact);
        Task<ReturnResult> UpdateProfileForm(User profile);
        Task<ReturnResult> UpdateCompanyDetails(CompanyDetails company);
        Task<ReturnResult> ApproveORSuspendUser(AdvertiserUserResult result);
        Task<ReturnResult> AddContactInformation(Contacts contact);
        Task<ReturnResult> AddCompanyDetails(CompanyDetails company);
        Task<ReturnResult> AddUser(User user);
    }
}
