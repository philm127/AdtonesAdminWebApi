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
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class LogonService : ILogonService
    {
        private readonly IConfiguration _configuration;
        private readonly AuthSettings _appSettings;
        private ILoginQuery _commandText;
        private readonly IHttpContextAccessor _httpAccessor;
        private IWebHostEnvironment _env;
        private readonly ILoginDAL _loginDAL;

        ReturnResult result = new ReturnResult();

        private const int PASSWORD_HISTORY_LIMIT = 8;

        public LogonService(IConfiguration configuration, IOptions<AuthSettings> appSettings, IWebHostEnvironment env,
                                ILoginDAL loginDAL, ILoginQuery commandText, IHttpContextAccessor httpAccessor)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
            _env = env;
            _loginDAL = loginDAL;
            _commandText = commandText;
            _httpAccessor = httpAccessor;

        }


        public async Task<ReturnResult> Login(User userForm)
        {
            User user = new User();
            try
            {                
                    user = await _loginDAL.GetLoginUser(_commandText.LoginUser, userForm);

                if (user != null)
                {
                    if (PasswordExpiredAttribute(user))
                    {
                        result.result = 0;
                        result.error = "Your Password has expired please reset it";
                        return result;
                    }

                    if (user.VerificationStatus == false)
                    {
                        // ModelState.AddModelError("", "Please verify your email account.");
                        result.result = 0;
                        result.error = "Please verify your email account.";
                        return result;
                    }
                    // 4 is user has been blocked for too many incorrect login attempts.
                    else if (user.Activated == 4)
                    {
                        DateTime date1 = user.LockOutTime.Value;
                        DateTime date2 = DateTime.Now;
                        TimeSpan ts = date2 - date1;
                        if ((int)ts.Minutes < 15)
                        {
                            int remainingminutes = 15 - ts.Minutes;
                            result.result = 0;
                            result.error = "Your account is locked, Kindly try again after " + remainingminutes + " minute(s)";
                            return result;
                        }
                        else
                        {
                            user.Activated = (int)Enums.UserStatus.Approved;
                            user.LockOutTime = null;
                            var x = await _loginDAL.UpdateUserLockout(_commandText.UpdateLockout, userForm);
                        }
                    }
                    else if (user.Activated == 0)
                    {
                        if (user.RoleId == 6)
                        {
                            result.result = 0;
                            result.error = "Your account has been InActive by adtones administrator.so, Please contact adtones admin.";
                            return result;
                        }
                        else
                        {
                            result.result = 0;
                            result.error = "Your account is not approved by Adtones so please contact Adtones Admin.";
                            return result;
                        }
                    }
                    else if (user.Activated == 2)
                    {
                        result.result = 0;
                        result.error = "Your account has been suspended by Adtones, please contact Adtones admin.";
                        return result;
                    }
                    else if (user.Activated == 3)
                    {
                        result.result = 0;
                        result.error = "Your account has been deleted by adtones administrator so please contact adtones admin.";
                       return result;
                    }
                    if (ValidatePassword(user, userForm))
                    {
                        var jwt = new AuthService(_configuration);
                        user.Token = jwt.GenerateSecurityToken(user);
                    }
                    else
                    {
                        result.result = 0;
                        result.error = "The user name and/or password provided is incorrect.";
                        user = null;
                    }
                }
                else
                {
                    if (user == null)
                    {
                        result.result = 0;
                        result.error = "The user name or password provided is incorrect.";
                        return result;
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


        private bool PasswordExpiredAttribute(User user)
        {

                int PasswordExpiresInDays = int.Parse(_configuration.GetSection("AppSettings").GetSection("PasswordExpiresInDays").Value);

                TimeSpan ts = DateTime.Today - user.LastPasswordChangedDate;

                if (ts.TotalDays > PasswordExpiresInDays)
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


        private async Task<ReturnResult> InvalidLoginAttemps(User user)
        {
            result.result = 0;
            try
            {
                user.cntAttemps++;
                if (user.cntAttemps == 1 || user.cntAttemps == 2 || user.cntAttemps == 3)
                    result.error = $"Invalid login attempt(s) {{user.cntAttemps}}, attempts remaining  {{Convert.ToInt32(5 - user.cntAttemps).ToString()}}";
                else if (user.cntAttemps == 4)
                {
                    result.error = $"Invalid login attempt(s) {{user.cntAttemps}}. Last attempt remaining.";
                }
                if (user.cntAttemps == 5)
                {
                    var x = await BlockUser(user);

                    user.cntAttemps = 0;
                    result.error = "Your account is locked, Kindly try again after 15 minutes";
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "InvalidLoginAttempts"
                };
                _logging.LogError();
                result.result = 0;
            }
            result.body = user;
            return result;
        }


        public async Task<int> BlockUser(User user)
        {
            try
            {
                user.Activated = (int)Enums.UserStatus.Blocked;
                user.LockOutTime = DateTime.Now;
                return await _loginDAL.UpdateUserLockout(_commandText.UpdateLockout, user);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "BlockUser"
                };
                _logging.LogError();
                return 0;
            }
        }


    }
}
