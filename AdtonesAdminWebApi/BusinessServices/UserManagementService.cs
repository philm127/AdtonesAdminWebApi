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

        
        public UserManagementService(IConfiguration configuration, ILogonService logonService, IUserManagementDAL userDAL, 
            IConnectionStringService connService, IHttpContextAccessor httpAccessor, ISalesManagementDAL salesManagement, ISendEmailMailer mailer)
        {
            _configuration = configuration;
            _logonService = logonService;
            _userDAL = userDAL;
            _connService = connService;
            _httpAccessor = httpAccessor;
            _salesManagement = salesManagement;
            _mailer = mailer;
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
            if (company.Id == 0)
                return await AddCompanyDetails(company);

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
            var update_query = @"INSERT INTO CompanyDetails(CompanyName,Address,AdditionalAddress,Town,PostCode,CountryId,UserId)
                                                VALUES(@CompanyName,@Address,@AdditionalAddress,@Town,@PostCode,@CountryId,@UserId)";

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


        private UserAddFormModel SetUpAdvertiserModel(UserAddFormModel command)
        {
            if (!command.MailSuppression)
            {
                command.Activated = 0;
                command.Outstandingdays = 0;
                command.VerificationStatus = false;
                command.IsMsisdnMatch = false;
                command.IsEmailVerfication = false;
                command.PhoneticAlphabet = null;
                command.IsMobileVerfication = false;
            }
            return command;
        }


        public async Task<ReturnResult> AddUser(UserAddFormModel model)
        {
            try
            {
                bool exists = await _userDAL.CheckIfUserExists(model);
                if (exists)
                {
                    result.body = "A user with that email already exists.";
                    result.result = 0;
                    return result;
                }
                model.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.Password);
                model.VerificationStatus = true;
                model.Outstandingdays = 0;
                model.IsMsisdnMatch = true;
                model.IsEmailVerfication = true;
                model.PhoneticAlphabet = null;
                model.IsMobileVerfication = true;
                model.UserMatchTableName = null;

                if (model.RoleId == 3 && !model.MailSuppression)
                    model = SetUpAdvertiserModel(model);


                int mainUserId = 0;
                int opUserId = 0;

                
                mainUserId = await _userDAL.AddNewUser(model);

                if (mainUserId > 0)
                {
                    model.AdtoneServerUserId = mainUserId;
                    

                    var command1 = new Contacts();
                    int currencyId = 0;
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        currencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CountryId=@countryId;",
                                                                      new { countryId = model.CountryId });
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
                    command1.Email = model.Email;
                    command1.PhoneNumber = model.PhoneNumber;
                    command1.Address = model.Address;
                    command1.CountryId = model.CountryId;

                    var contId = await AddContactInformation(command1);
                    if (contId == 0)
                    {
                        await _userDAL.DeleteNewUser(mainUserId);
                        result.result = 0;
                        result.error = "Contact was not added";
                        return result;
                    }
                    else if (contId == -1)
                    {
                        await _userDAL.DeleteNewUser(mainUserId);
                        result.result = 0;
                        result.error = "A contact with this mobile phone already exists";
                        return result;
                    }
                    else
                    {
                        // Only run if successfully added new user to contacts
                        opUserId = await _userDAL.AddNewUserToOperator(model);
                        command1.AdtoneServerContactId = contId;
                        command1.UserId = opUserId;
                        var xx = await _userDAL.AddNewContactToOperator(command1);

                        CompanyDetails company = new CompanyDetails();
                        company.Address = model.Address;
                        company.CompanyName = model.Organisation;
                        company.CountryId = model.CountryId;
                        company.UserId = mainUserId;

                        var res = await AddCompanyDetails(company);

                        bool mailsent = false;

                        var ytr = _httpAccessor.GetRoleIdFromJWT();
                        var tst = _httpAccessor.GetUserIdFromJWT();
                        try
                        {
                            if (ytr == (int)Enums.UserRole.SalesExec && model.MailSuppression == true)
                            {
                                var usr = await _userDAL.GetUserById(tst);
                                mailsent = await SendConfirmationMail(model, usr.Email);
                            }
                            else
                                mailsent = await SendConfirmationMail(model);
                        }
                        catch (Exception ex)
                        {
                            var _loggingA = new ErrorLogging()
                            {
                                ErrorMessage = ex.Message.ToString(),
                                StackTrace = ex.StackTrace.ToString(),
                                PageName = "UserManagementService",
                                ProcedureName = "AddUser - SendMail"
                            };
                            _loggingA.LogError();
                        }

                        result.body = "User " + model.FirstName + " " + model.LastName + " was Succesfully Added";
                        try
                        {
                            if (ytr == (int)Enums.UserRole.SalesManager && model.RoleId == (int)Enums.UserRole.SalesExec)
                            {
                                var x = await _userDAL.InsertManagerToSalesExec(tst, mainUserId);
                            }
                            else if (ytr == (int)Enums.UserRole.SalesExec)
                            {
                                var x = await _salesManagement.InsertNewAdvertiserToSalesExec(tst, mainUserId, model.MailSuppression);
                            }
                        }
                        catch (Exception ex)
                        {
                            var _loggingC = new ErrorLogging()
                            {
                                ErrorMessage = ex.Message.ToString(),
                                StackTrace = ex.StackTrace.ToString(),
                                PageName = "UserManagementService",
                                ProcedureName = "AddUser - AddToSalesTable"
                            };
                            _loggingC.LogError();
                            result.result = 0;
                            result.body = "There was an issue adding to the sales table";
                            result.error = ex.Message.ToString();
                        }
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
        public async Task<ReturnResult> AddOperatorAdminUser(UserAddFormModel model)
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
                command.OperatorId = model.OperatorId.GetValueOrDefault();
                command.IsMsisdnMatch = true;
                command.IsEmailVerfication = true;
                command.PhoneticAlphabet = null;
                command.IsMobileVerfication = true;
                command.OrganisationTypeId = null;
                command.UserMatchTableName = null;
                command.Permissions = model.Permissions;
                command.CountryId = model.CountryId;


                var body = await AddUser(model);
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


        private async Task<bool> SendConfirmationMail(UserAddFormModel user, string alt_email = null)
        {
            string url = string.Empty;
            string template = string.Empty;
            SendEmailModel emailModel = new SendEmailModel();

            try
            {
                if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom && user.RoleId == (int)Enums.UserRole.OperatorAdmin)
                    url = _configuration.GetValue<string>("AppSettings:SafaricomOperatorAdminSiteAddress").ToString();
                else if (user.RoleId == 3 && user.MailSuppression == false)
                    url = string.Format("{0}?activationCode={1}", _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("AdvertiserVerificationUrl").ToString(), user.Email);
                else
                    url = _configuration.GetValue<string>("AppSettings:siteAddress").ToString();

                url = "<a href='" + url + "'>" + url + " </a>";
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                if (user.RoleId == 3 && user.MailSuppression == false)
                    template = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("AdvertiserVerificationEmailTemplate").Value;
                else
                    template = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("OperatorAdminRegistrationEmailTemplete").Value;

                var path = Path.Combine(otherpath, template);
                string emailContent = string.Empty;
                using (var reader = new StreamReader(path))
                {
                    emailContent = reader.ReadToEnd();
                }
                if (user.RoleId == 3 && user.MailSuppression == false)
                    emailContent = string.Format(emailContent, url);
                else
                    emailContent = string.Format(emailContent, user.FirstName, user.LastName, url, user.Email, user.Password);


                emailModel.Body = emailContent.Replace("\n", "<br/>");
                if (alt_email != null)
                    emailModel.SingleTo = alt_email;
                else
                    emailModel.SingleTo = user.Email;
                if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom && user.RoleId == (int)Enums.UserRole.OperatorAdmin)
                    emailModel.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomSiteEmailAddress").Value;
                else
                    emailModel.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;

                emailModel.Subject = "Email Verification";
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "SendConfirmationMail = Model Build"
                };
                _logging.LogError();

                var msg = ex.Message.ToString();
                return false;
            }
            try
            {
                await _mailer.SendBasicEmail(emailModel);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "SendConfirmationMail"
                };
                _logging.LogError();

                var msg = ex.Message.ToString();
                return false;
            }
            return true;
        }

        
    }
}
