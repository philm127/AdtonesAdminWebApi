﻿using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using AdtonesAdminWebApi.Model;
using System;
using Dapper;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogonService _logonService;
        private readonly IUserManagementDAL _userDAL;
        private readonly IConnectionStringService _connService;
        ReturnResult result = new ReturnResult();

        
        public UserManagementService(IConfiguration configuration, ILogonService logonService, IUserManagementDAL userDAL, IConnectionStringService connService)
        {
            _configuration = configuration;
            _logonService = logonService;
            _userDAL = userDAL;
            _connService = connService;
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "GetContactForm"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        private async Task<int> AddContactInformation(Contacts contact)
        {
            var opConId = 0;
            try
            {
                    bool exists = await _userDAL.CheckIfContactExists(contact);
                    if (exists)
                    {
                        return -1;
                    }

                opConId = await _userDAL.AddNewContact(contact);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "AddContactInformation"
                };
                _logging.LogError();
                return 0;
            }
            return opConId;
        }


        public async Task<ReturnResult> UpdateContactForm(Contacts contact)
        {
            try
            {
                result.body = await _userDAL.UpdateContact(contact);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "UpdateContactForm"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "GetProfileForm"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "UpdateProfileForm"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "GetCompanyForm"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "UpdateCompanyDetails"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddCompanyDetails(CompanyDetails company)
        {
            var update_query = @"INSERT INTO CompanyDetails(CompanyName,Address,AdditionalAddress,Town,PostCodeCountryId)
                                                VALUES(@CompanyName,@Address,@AdditionalAddress,@Town,@PostCode,@CountryId)";

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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "AddCompanyDetails"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "UpdateContactForm"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddUser(User user, OperatorAdminFormModel model = null)
        {
            try
            {
                bool exists = await _userDAL.CheckIfUserExists(user);
                if (exists)
                {
                    result.body = "A user with that email already exists.";
                    result.result = 0;
                    return result;
                }
                int mainUserId = 0;
                int opUserId = 0;
                
                mainUserId = await _userDAL.AddNewUser(user);

                if (mainUserId > 0)
                {
                    user.AdtoneServerUserId = mainUserId;
                    opUserId = await _userDAL.AddNewUserToOperator(user);

                    var command1 = new Contacts();
                    int currencyId = 0;
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        currencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CountryId=@countryId;",
                                                                      new { countryId = user.CountryId });
                    }

                    if (currencyId == 0)
                    {
                        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                        {
                            currencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CurrencyCode='USD';");
                        }
                    }

                    command1.CurrencyId = currencyId;
                    command1.UserId = mainUserId;
                    command1.MobileNumber = model.MobileNumber;
                    command1.FixedLine = null;
                    command1.Email = user.Email;
                    command1.PhoneNumber = model.PhoneNumber;
                    command1.Address = model.Address;
                    command1.CountryId = user.CountryId;

                    var contId = await AddContactInformation(command1);
                    if (contId == 0)
                    {
                        result.result = 0;
                        result.error = "Contact was not added";
                        return result;
                    }
                    else if( contId == -1)
                    {
                        result.result = 0;
                        result.error = "A contact with this mobile phone already exists";
                        return result;
                    }
                    else
                    {
                        command1.AdtoneServerContactId = contId;
                        command1.UserId = opUserId;
                        var xx = await _userDAL.AddNewContactToOperator(command1);

                        // For scalability added an array of app config settings to retrieve specifically in this case for
                        // operator admin.
                        /// TODO: Sort out E-Mail settings ideally into one service for all.
                        var confSettings = new string[] { "OperatorAdminRegistrationEmailTemplete", "OperatorAdminUrl" };
                        // SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.PasswordHash, confSettings);
                        result.body = "Operator Admin registered for Operator " + model.FirstName + " " + model.LastName;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "AddUser"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "UpdateUserStatus"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Not sure if all users will have same defaults so left this here for now.
        /// If it transpires that they have similar or the same defaults then may move
        /// to have a General USER setup.
        /// TODO: Check what other user types have as defaults.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> AddOperatorAdminUser(OperatorAdminFormModel model)
        {

            try
            {

                // Not sure if all users will have same defaults so left this here for now.
                // If it transpires that they have similar or the same defaults then may move.
                /// TODO: Check what other user types have as defaults.
                var command = new User();

                command.Email = model.Email;
                command.FirstName = model.FirstName;
                command.LastName = model.LastName;
                command.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.Password);
                command.Organisation = model.Organisation;
                command.RoleId = (int)Enums.UserRole.OperatorAdmin;
                command.Activated = model.Activated;
                command.VerificationStatus = true;
                command.Outstandingdays = 0;
                command.OperatorId = model.OperatorId;
                command.IsMsisdnMatch = true;
                command.IsEmailVerfication = true;
                command.PhoneticAlphabet = null;
                command.IsMobileVerfication = true;
                command.OrganisationTypeId = null;
                command.UserMatchTableName = null;
                command.Permissions = model.Permissions;
                command.CountryId = model.CountryId;


                var body = await AddUser(command,model);
                if (body.result == 0)
                {
                    result.result = 0;
                    result.error = body.error;
                    return result;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "AddOperator-Adding"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Adding user failed";
            }
            return result;
        }


        public async Task<ReturnResult> GetOperatorAdmin(int userId)
        {
            try
            {
                result.body = await _userDAL.getOperatorAdmin(userId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "GetOperatorAdmin"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetUserById(int userId)
        {

            try
            {
                result.body = await _userDAL.GetUserById(userId);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "GetUserById"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }

        
    }
}
