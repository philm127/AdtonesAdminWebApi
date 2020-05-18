using AdtonesAdminWebApi.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net.Mail;
using Microsoft.AspNetCore.Hosting;
using AdtonesAdminWebApi.Services;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class LogonService : ILogonService
    {
        private readonly IConfiguration _configuration;
        private readonly AuthSettings _appSettings;
        private IWebHostEnvironment _env;

        ReturnResult result = new ReturnResult();

        private const int PASSWORD_HISTORY_LIMIT = 8;

        public LogonService(IConfiguration configuration, IOptions<AuthSettings> appSettings, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
            _env = env;
        }


        public async Task<ReturnResult> Login(User userForm)
        {
            User user = new User();
            try
            {
                int usererror = 0;

                var login_query = @"SELECT UserId,RoleId,Email,FirstName,LastName,PasswordHash,Activated,
                                    OperatorId, Organisation, DateCreated, VerificationStatus
                                    FROM Users WHERE LOWER(Email)=@email AND Activated !=3; ";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    user = await connection.QueryFirstOrDefaultAsync<User>(login_query,new { email = userForm.Email.ToLower() });
                }
                if (user != null)
                {
                    if (user.VerificationStatus == false)
                    {
                        // ModelState.AddModelError("", "Please verify your email account.");
                        result.result = 0;
                        result.error = "Please verify your email account.";
                        usererror = -1;
                    }
                    else if (user.Activated == 0)
                    {
                        if (user.RoleId == 6)
                        {
                            result.result = 0;
                            result.error = "Your account has been InActive by adtones administrator.so, Please contact adtones admin.";
                            usererror = -1;
                        }
                        else
                        {
                            result.result = 0;
                            result.error = "Your account is not approved by adtones administrator so please contact adtones admin.";
                            usererror = -1;
                        }
                    }
                    else if (user.Activated == 1)
                    {
                        //if (user.OperatorId == (int)OperatorTableId.Safaricom)
                        //{
                        //    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        //    usererror = -1;
                        //}
                    }
                    else if (user.Activated == 2)
                    {
                        result.result = 0;
                        result.error = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                        usererror = -1;
                    }
                }
                else
                {
                    if (user == null)
                    {
                        result.result = 0;
                        result.error = "The user name or password provided is incorrect.";
                        usererror = -1;
                    }
                    else
                    {
                        if (user.Activated == 3)
                        {
                            result.result = 0;
                            result.error = "Your account has been deleted by adtones administrator so please contact adtones admin.";
                            usererror = -1;
                        }
                    }
                }
                if (usererror == 0)
                {
                    if (ValidatePassword(user, userForm))
                    {
                        var jwt = new AuthService(_configuration);
                        user.Token = jwt.GenerateSecurityToken(user);
                        user.PasswordHash = string.Empty;
                    }
                    else
                    {
                        result.result = 0;
                        result.error = "The user name and/or password provided is incorrect.";
                        user = null;
                    }
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "Login"
                };
                _logging.LogError();
                result.result = 0;
            }
            user.PasswordHash = null;
            result.body = user;
            return result;
        }


        public async Task<ReturnResult> ForgotPassword(string emailAddress)
        {
            User user = new User();
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    user = await connection.QueryFirstOrDefaultAsync<User>(@"SELECT UserId,RoleId,Email,FirstName,PasswordHash,Activated,
                                                                                        LastName,Outstandingdays,Organisation,DateCreated,
                                                                                        VerificationStatus
                                                                                        FROM Users WHERE LOWER(Email) = @email 
                                                                                        AND RoleId IN (1,4,5,6)",
                                                                                            new { email = emailAddress.ToLower() });
                }
                /// TODO: Remove once testing done
                // emailAddress = "philm127@gmail.com";
                if (user == null)
                {
                    result.result = 0;
                    result.error = "Your account cannot be found.";
                    return result;
                }
                else if (user.VerificationStatus == false)
                {
                    result.result = 0;
                    result.error = "Please verify your email account.";
                    return result;
                }
                else if (user.Activated == 0)
                {
                    result.result = 0;
                    result.error = "Your account is not approved by adtones administrator so please contact adtones admin.";
                    return result;
                }
                else if (user.Activated == 2)
                {
                    result.result = 0;
                    result.error = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                    return result;
                }
                else if (user.Activated == 3)
                {
                    result.result = 0;
                    result.error = "Your account is  deleted by adtones administrator so please contact adtones admin.";
                    return result;
                }
                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                string email = EncryptionHelper.EncryptSingleValue(user.Email);

                string url = string.Format("{0}?activationCode={1}", _configuration.GetValue<string>(
                                "AppSettings:AdminResetPassword"), email);

                /// TODO: Check the working of filepaths
                if (string.IsNullOrWhiteSpace(_env.WebRootPath))
                {
                    _env.WebRootPath = Directory.GetCurrentDirectory();
                }

                string webroot = _env.WebRootPath + _configuration.GetValue<string>("AppSettings:ResetPasswordEmailTemplate");

                var reader =
                    new StreamReader(webroot);
                //Path.Combine(webroot, _configuration.GetValue<string>("AppSettings:ResetPasswordEmailTemplate")));
                string emailContent = reader.ReadToEnd();
                emailContent = string.Format(emailContent, url);

                MailMessage mail = new MailMessage();
                mail.To.Add(user.Email);
                //mail.To.Add("xxx@gmail.com");
                var whatever = _configuration.GetValue<string>("AppSettings:SiteEmailAddress");
                mail.From = new MailAddress("support@adtones.com");// _configuration.GetValue<string>("SiteEmailAddress"));
                mail.Subject = "Email Verification";

                mail.Body = emailContent.Replace("\n", "<br/>");

                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = _configuration.GetValue<string>("AppSettings:SmtpServerAddress"); //Or Your SMTP Server Address
                smtp.Credentials = new System.Net.NetworkCredential
                     (_configuration.GetValue<string>("AppSettings:SMTPEmail"), _configuration.GetValue<string>("AppSettings:SMTPPassword")); // ***use valid credentials***

                smtp.Port = int.Parse(_configuration.GetValue<string>("AppSettings:SmtpServerPort"));

                //Or your Smtp Email ID and Password
                try
                {
                    /// TODO: fix email service
                    smtp.EnableSsl = Convert.ToBoolean(_configuration.GetValue<string>("AppSettings:EnableEmailSending").ToString());
                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "LogonService",
                        ProcedureName = "ForgotPassword - SendEmail"
                    };
                    _logging.LogError();

                    var msg = ex.Message.ToString();
                    result.result = 0;
                    result.error = "Email failed to send";
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "ForgotPassword"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// updates the UpdatePasswordHistory table as User cannot use same password for last 8 changes
        /// </summary>
        /// <returns></returns>
        public async Task<int> UpdatePasswordHistory(int userId, string password)
        {
            int res = 0;
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    res = await connection.ExecuteAsync(@"INSERT INTO UpdatePasswordHistory(UserId,PasswordHash,DateCreated)
                                                                        VALUES(@Userid,@PasswordHash,GETDATE())",
                                                                        new { UserId = userId, PasswordHash = password });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "UpdatePasswordHistory"
                };
                _logging.LogError();
                res = 0;
            }
            return res;
        }


        /// <summary>
        /// We have a new ule where Password cannot be reused I think 8 is the limit, therefore checks if
        /// Password is within the last 8 used.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public async Task<bool> IsPreviousPassword(int userId, string newPassword)
        {
            IEnumerable<string> userPasswordHistory;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                userPasswordHistory = await connection.QueryAsync<string>(@"SELECT TOP (@top) PasswordHash 
                                                                                FROM UserPasswordHistories  
                                                                                WHERE UserId=@userId ORDER BY CreatedDate DESC",
                                                                          new { userId = userId, top = PASSWORD_HISTORY_LIMIT });
            }

            if (userPasswordHistory.Cast<string>().Contains(newPassword))
                return true;
            else
                return false;
        }


        private bool ValidatePassword(User user,User userForm)
        {
            try
            {
                string encoded = Md5Encrypt.Md5EncryptPassword(userForm.PasswordHash);
                return user.PasswordHash.Equals(encoded);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "ValidatePassword"
                };
                _logging.LogError();
                result.result = 0;
                return false;
            }
        }
    
    
    }
}
