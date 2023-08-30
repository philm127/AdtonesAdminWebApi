using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using AdtonesAdminWebApi.Model;
using System;
using Dapper;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using System.IO;
using AdtonesAdminWebApi.Services.Mailer;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogonService _logonService;
        private readonly IUserManagementDAL _userDAL;
        private readonly IConnectionStringService _connService;
        IHttpContextAccessor _httpAccessor;
        private readonly ISalesManagementDAL _salesManagement;
        private readonly ISendEmailMailer _mailer;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "UserManagementService";


        public UserManagementService(IConfiguration configuration, ILogonService logonService, IUserManagementDAL userDAL,
            IConnectionStringService connService, IHttpContextAccessor httpAccessor, ISalesManagementDAL salesManagement,
                                ISendEmailMailer mailer, ILoggingService logServ)
        {
            _configuration = configuration;
            _logonService = logonService;
            _userDAL = userDAL;
            _connService = connService;
            _httpAccessor = httpAccessor;
            _salesManagement = salesManagement;
            _mailer = mailer;
            _logServ = logServ;
        }


        public async Task<ReturnResult> GetUserDetail(int userId)
        {
            var details = new UserFullDetailsViewModel();
            var getcontact = await GetContactForm(userId);
            if (getcontact.result == 1)
            {
                details.Contacts = (Contacts)getcontact.body;
            }
            else
                return getcontact;

            var getprofile = await GetProfileForm(userId);
            if (getprofile.result == 1)
            {
                details.User = (User)getprofile.body;
            }
            else
                return getprofile;

            var getcompany = await GetCompanyForm(userId);
            if (getcompany.result == 1)
            {
                details.CompanyDetails = (CompanyDetails)getcompany.body;
            }
            else
                return getcompany;

            result.body = details;
            return result;

        }


        public async Task<ReturnResult> GetContactForm(int userId)
        {
            try
            {
                result.body = await _userDAL.getContactByUserId(userId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetContactForm";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateContactForm(Contacts contact)
        {
            try
            {
                result.body = await _userDAL.UpdateContact(contact);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateContactForm";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetProfileForm(int userId)
        {
            try
            {
                result.body = await _userDAL.GetUserById(userId);
                
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetProfileForm";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateProfileForm(User profile)
        {
            try
            {
                result.body = await _userDAL.UpdateUser(profile);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateProfileForm";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetCompanyForm(int userId)
        {
            try
            {
                result.body = await _userDAL.getCompanyDetails(userId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCompanyForm";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateCompanyDetails(CompanyDetails company)
        {
            var update_query = @"UPDATE CompanyDetails SET CompanyName=@CompanyName,Address=@Address,AdditionalAddress=@AdditionalAddress,
                                 Town=@Town,PostCode=@PostCode,CountryId=@CountryId WHERE Id=@Id";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query, company);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateCompanyDetails";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateUserPermission(IdCollectionViewModel model)
        {
            try
            {
                result.body = await _userDAL.UpdateUserPermission(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateUserPermission";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        

        public async Task<ReturnResult> UpdateUserStatus(AdvertiserDashboardResult user)
        {
            try
            {
                result.body = await _userDAL.UpdateUserStatus(user);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateUserStatus";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }

    }
}
