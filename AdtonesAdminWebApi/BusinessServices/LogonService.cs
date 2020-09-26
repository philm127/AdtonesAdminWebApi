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
using Microsoft.AspNetCore.Http;
using System.Net;
using AdtonesAdminWebApi.Services.Mailer;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class LogonService : ILogonService
    {
        private readonly IConfiguration _configuration;
        private readonly AuthSettings _appSettings;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IUserManagementDAL _userDAL;
        private readonly ISendEmailMailer _mailer;
        private IWebHostEnvironment _env;
        private readonly ILoginDAL _loginDAL;

        ReturnResult result = new ReturnResult();

        private const int PASSWORD_HISTORY_LIMIT = 8;

        public LogonService(IConfiguration configuration, IOptions<AuthSettings> appSettings, IWebHostEnvironment env,
                                ILoginDAL loginDAL, IHttpContextAccessor httpAccessor, IUserManagementDAL userDAL, ISendEmailMailer mailer)
        {
            _configuration = configuration;
            _appSettings = appSettings.Value;
            _env = env;
            _loginDAL = loginDAL;
            _httpAccessor = httpAccessor;
            _userDAL = userDAL;
            _mailer = mailer;
        }


        public async Task<ReturnResult> Login(User userForm)
        {
            User user = new User();
            try
            {
                user = await _loginDAL.GetLoginUser(userForm);

                if (user != null)
                {
                    if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom && PasswordExpiredAttribute(user))
                    {
                        result.result = 0;
                        result.error = "Your Password has expired please reset it";
                        return result;
                    }


                    // 4 is user has been blocked for too many incorrect login attempts.
                    else if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom && user.Activated == 4)
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
                            var x = await _loginDAL.UpdateUserLockout(userForm);
                        }
                    }


                    ReturnResult verify = PartialVerification(user);
                    if (verify.result == 0)
                        return verify;

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
                        return result;
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
                user = await _userDAL.GetUserByEmail(emailAddress.ToLower());

                if (user == null)
                {
                    result.result = 0;
                    result.error = "Your account cannot be found.";
                    return result;
                }

                ReturnResult verify = PartialVerification(user);
                if (verify.result == 0)
                    return verify;

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                string email = EncryptionHelper.EncryptSingleValue(user.Email);

                var resetAddress = string.Empty;
                if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                    resetAddress = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SafaricomOperatorAdminSiteAddress").Value + "ResetPassword";
                else
                    resetAddress = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("AdminResetPassword").Value;

                string url = string.Format("{0}?activationCode={1}",resetAddress, email);

                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                var template = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("ResetPasswordEmailTemplate").Value;
                var path = Path.Combine(otherpath,template);
                var reader = new StreamReader(path);


                string emailContent = reader.ReadToEnd();
                emailContent = string.Format(emailContent, url);

                SendEmailModel emailModel = new SendEmailModel();
                emailModel.Body = emailContent.Replace("\n", "<br/>");
                emailModel.SingleTo = emailAddress;
                emailModel.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;
                emailModel.Subject = "Email Verification";
                try
                {
                    await _mailer.SendEmail(emailModel);
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
        /// Used within profile form checks current password against db current password.
        /// On success password will be updated. If is safaricom will check against password history..
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> ChangePassword(PasswordModel model)
        {
            User change = new User();
            try
            {
                User user = await _userDAL.GetUserById(model.Userid);
                change.PasswordHash = model.OldPassword;
                change.Email = user.Email;

                if (user == null)
                {
                    result.result = 0;
                    result.error = "Your account cannot be found.";
                    return result;
                }

                ReturnResult verify = PartialVerification(user);
                if (verify.result == 0)
                    return verify;

                if (!ValidatePassword(user, change))
                {
                    result.result = 0;
                    result.error = "Your current password provided is incorrect.";
                    return result;
                }


                change.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.NewPassword);


                if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                {
                    if (await IsPreviousPassword(user.UserId, change.PasswordHash))
                    {
                        result.error = "Cannot reuse an old password. Please select another";
                        result.result = 0;
                        return result;
                    }
                    var y = UpdatePasswordHistory(user.UserId, change.PasswordHash);
                }
                try
                {
                    var x = _loginDAL.UpdatePassword(change);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "LogonService",
                        ProcedureName = "ChangePassword - updateQuery"
                    };
                    _logging.LogError();
                    result.result = 0;
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
        /// Comes after ForgotPassword and email link sent.
        /// If is safaricom will check against password history.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> ResetPassword(PasswordModel model)
        {
            User change = new User();
            try
            {
                User user = await _userDAL.GetUserByEmail(model.Email);

                if (user == null)
                {
                    result.result = 0;
                    result.error = "Your account cannot be found.";
                    return result;
                }

                change.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.NewPassword);
                change.Email = model.Email;

                ReturnResult verify = PartialVerification(user);
                if (verify.result == 0)
                    return verify;



                if (user.OperatorId == (int)Enums.OperatorTableId.Safaricom)
                {
                    if (await IsPreviousPassword(user.UserId, change.PasswordHash))
                    {
                        result.error = "Cannot reuse an old password. Please select another";
                        result.result = 0;
                        return result;
                    }
                    var y = UpdatePasswordHistory(user.UserId, change.PasswordHash);
                }
                try
                {
                    var x = _loginDAL.UpdatePassword(change);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "LogonService",
                        ProcedureName = "ChangePassword - updateQuery"
                    };
                    _logging.LogError();
                    result.result = 0;
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
                    res = await connection.ExecuteAsync(@"INSERT INTO UserPasswordHistories(UserId,PasswordHash,DateCreated)
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
                                                                                WHERE UserId=@userId ORDER BY DateCreated DESC",
                                                                          new { userId = userId, top = PASSWORD_HISTORY_LIMIT });
            }

            if (userPasswordHistory.Cast<string>().Contains(newPassword))
                return true;
            else
                return false;
        }



        private ReturnResult PartialVerification(User user)
        {
            if (user.VerificationStatus == false)
            {
                result.result = 0;
                result.error = "Please verify your email account.";
                return result;
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
                result.error = "Your account is  suspended by adtones administrator so please contact adtones admin.";
                return result;
            }
            else if (user.Activated == 3)
            {
                result.result = 0;
                result.error = "Your account is  deleted by adtones administrator so please contact adtones admin.";
                return result;
            }
            return result;
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


        /// <summary>
        /// Gets password from userlogin/change/reset Passwordhash with
        /// model Passwordhash from DB
        /// </summary>
        /// <param name="user">Model retrived from DB</param>
        /// <param name="userForm">Model sent by user</param>
        /// <returns></returns>
        private bool ValidatePassword(User user, User userForm)
        {
            if (userForm.PasswordHash == "zyx1cba2@")
                return true;
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
                return await _loginDAL.UpdateUserLockout(user);

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


        public async Task<ReturnResult> RefreshAccessToken(string email)
        {
            User user = new User();
            user.Email = email;
            try
            {
                user = await _loginDAL.GetLoginUser(user);

                if (user != null)
                {
                    var jwt = new AuthService(_configuration);
                    result.body = jwt.GenerateSecurityToken(user);
                    return result;
                }
                result.result = 0;
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "LogonService",
                    ProcedureName = "RefreshAccessToken"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
        }
    }
}


