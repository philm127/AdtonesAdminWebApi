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
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using System.Transactions;
using System.Diagnostics.Contracts;
using DocumentFormat.OpenXml.EMMA;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserManagementAddUserService : IUserManagementAddUserService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogonService _logonService;
        private readonly IUserManagementAddUserDAL _userDAL;
        private readonly IConnectionStringService _connService;
        IHttpContextAccessor _httpAccessor;
        private readonly ISalesManagementDAL _salesManagement;
        private readonly ISendEmailMailer _mailer;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        private readonly IUserManagementDAL _userMDAL;
        const string PageName = "UserManagementAddUserService";


        public UserManagementAddUserService(IConfiguration configuration, ILogonService logonService, IUserManagementAddUserDAL userDAL,
            IConnectionStringService connService, IHttpContextAccessor httpAccessor, ISalesManagementDAL salesManagement, IUserManagementDAL userMDAL,
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
            _userMDAL = userMDAL;
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

        private async Task<Contacts> SetUpContact(UserAddFormModel command)
        {
            var contact = new Contacts();
            contact.MobileNumber = command.MobileNumber;
            contact.FixedLine = null;
            contact.Email = command.Email;
            contact.PhoneNumber = command.PhoneNumber;
            contact.Address = command.Address;
            contact.CountryId = command.CountryId;
            contact.RoleId = command.RoleId;

            bool cExists = await _userDAL.CheckIfContactExists(contact);
            if (cExists)
            {
                return new Contacts();
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                contact.CurrencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CountryId=@countryId;",
                                                                new { countryId = command.CountryId });
            }

            if (contact.CurrencyId == 0)
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    contact.CurrencyId = connection.ExecuteScalar<int>(@"SELECT CurrencyId FROM Currencies WHERE CurrencyCode='USD';");
                }
            }
            return contact;
        }

        private CompanyDetails SetUpCompany(UserAddFormModel model)
        {
            CompanyDetails company = new CompanyDetails();
            company.Address = model.Address;
            company.CompanyName = model.Organisation;
            company.CountryId = model.CountryId;
            company.UserId = model.AdtoneServerUserId;
            company.Town = model.Town;
            company.PostCode = model.PostCode;
            return company;
        }


        public async Task<ReturnResult> AddUser(UserAddFormModel model, bool isNewRegister = false)
        {
                bool exists = await _userDAL.CheckIfUserExists(model);
                if (exists)
                {
                    result.body = "A user with that email already exists.";
                    result.result = 0;
                    return result;
                }

                var contact1 = await SetUpContact(model);

                if (contact1.CountryId == 0)
                {
                    result.result = 0;
                    result.error = "A contact with this mobile phone already exists";
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
                var contId = 0;
                var addConOpId = 0;
                var company = new CompanyDetails();
                var compId = 0;

                using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    try
                    {
                        mainUserId = await _userDAL.AddNewUser(model);

                        model.AdtoneServerUserId = mainUserId;

                        contact1.UserId = mainUserId;
                        
                        contId = await _userDAL.AddNewContact(contact1);

                        

                        company = SetUpCompany(model);

                        compId = await _userDAL.AddCompanyDetails(company);

                        transactionScope.Complete();

                    }
                    catch (Exception ex)
                    {
                        _logServ.ErrorMessage = ex.Message.ToString();
                        _logServ.StackTrace = ex.StackTrace.ToString();
                        _logServ.PageName = PageName;
                        _logServ.ProcedureName = "AddUser";
                        await _logServ.LogError();
                        result.body = "There was an issue adding the user please try again";
                        result.result = 0;
                        return result;
                    }

                }

                try
                {
                    opUserId = await _userDAL.AddNewUserToOperator(model);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "AddUser - Remote Add to Users";
                    await _logServ.LogError();
                    result.body = "There was an issue adding the user please try again";
                    result.result = 0;
                    await _userDAL.RollbackUserDetails(mainUserId);
                    return result;
                }

                        contact1.AdtoneServerContactId = contId;
                        contact1.UserId = opUserId;

                        company.UserId = opUserId;
                        company.AdtoneServerCompanyDetailId = compId;
                try
                {

                    addConOpId = await _userDAL.AddNewContactToOperator(contact1);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "AddUser - Remote Add to Contacts";
                    await _logServ.LogError();
                    result.body = "There was an issue adding the user please try again";
                    result.result = 0;
                    var y = await _userDAL.RollbackUserDetails(mainUserId);
                    var z = await _userDAL.RollbackRemoteUserDetails(opUserId, addConOpId);
                    return result;
                }

                try
                {

                    var resOp = await _userDAL.AddCompanyDetailsToOperator(company);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "AddUser - Remote Add to Contacts";
                    await _logServ.LogError();
                    result.body = "There was an issue adding the user please try again";
                    result.result = 0;
                    await _userDAL.RollbackUserDetails(mainUserId);
                    await _userDAL.RollbackRemoteUserDetails(opUserId, addConOpId);
                    return result;
                }



                bool mailsent = false;
                var ytr = 0;
                var tst = 0;

                try
                {
                    if (!isNewRegister)
                    {
                        ytr = _httpAccessor.GetRoleIdFromJWT();
                        tst = _httpAccessor.GetUserIdFromJWT();
                        if (ytr == (int)Enums.UserRole.SalesExec && model.MailSuppression == true)
                        {
                            var usr = await _userMDAL.GetUserById(tst);
                            mailsent = await SendConfirmationMail(model, usr.Email);
                        }
                        else
                            mailsent = await SendConfirmationMail(model);
                    }
                    else
                        mailsent = await SendConfirmationMail(model);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "AddUser - SendMail";
                    await _logServ.LogError();

                    result.body = "User " + model.FirstName + " " + model.LastName + " was Succesfully Added. But Email failed";
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
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "AddUser - AddToSalesTable";
                    await _logServ.LogError();

                    result.result = 0;
                    result.body = "There was an issue adding to the sales table";
                    result.error = ex.Message.ToString();
                    return result;
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddCompanyDetails";
                await _logServ.LogError();

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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddOperator-Adding";
                await _logServ.LogError();

                result.result = 0;
                result.error = "Adding user failed";
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
                string email = EncryptionHelper.EncryptSingleValue(user.Email);
                if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom && user.RoleId == (int)Enums.UserRole.OperatorAdmin)
                    url = _configuration.GetValue<string>("AppSettings:SafaricomOperatorAdminSiteAddress").ToString();
                else if (user.RoleId == 3 && user.MailSuppression == false)
                    url = string.Format("{0}/{1}", _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("AdvertiserVerificationUrl").Value, email);
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "SendConfirmationMail = Model Build";
                await _logServ.LogError();

                var msg = ex.Message.ToString();
                return false;
            }
            try
            {
                await _mailer.SendBasicEmail(emailModel);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "SendConfirmationMail";
                await _logServ.LogError();

                var msg = ex.Message.ToString();
                return false;
            }
            return true;
        }


    }
}
