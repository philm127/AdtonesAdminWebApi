using AdtonesAdminWebApi.BusinessServices.Interfaces;
using Dapper;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using AdtonesAdminWebApi.Model;
using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using AdtonesAdminWebApi.Services;
using System.Text;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogonService _logonService;
        ReturnResult result = new ReturnResult();

        


        public UserManagementService(IConfiguration configuration, ILogonService logonService)
        {
            _configuration = configuration;
            _logonService = logonService;
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
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<Contacts>(@"SELECT Id, UserId,MobileNumber,FixedLine,Email,
                                                                                    PhoneNumber,Address,CountryId,CurrencyId 
                                                                                    FROM Contacts WHERE UserId = @userid ",
                                                                                        new { userid = userId });
                }
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


        public async Task<ReturnResult> AddContactInformation(Contacts contact)
        {
            try
            {
                bool exists = await CheckIfContactExists(contact);
                if (exists)
                {
                    result.body = "A user with that mobile number already exists.";
                    result.result = 0;
                    return result;
                }

                var insert_query = @"INSERT INTO Contacts(UserId,MobileNumber,FixedLine,Email,PhoneNumber,Address,CountryId,CurrencyId)
                                                    VALUES(@UserId,@MobileNumber,@FixedLine,@Email,@PhoneNumber,@Address,@CountryId,@CurrencyId);
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteScalarAsync<int>(insert_query, contact);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "AddContact"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateContactForm(Contacts contact)
        {
            var update_query = @"UPDATE Contacts SET MobileNumber=@MobileNumber,FixedLine=@FixedLine,Email=@Email,
                                PhoneNumber=@PhoneNumber,Address=@Address,CountryId=@CountryId
                                WHERE Id = @Id";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query, contact);
                }
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
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<User>(@"SELECT UserId,RoleId,Email,FirstName,Activated,
                                                                                        LastName,Outstandingdays,Organisation,DateCreated 
                                                                                        FROM Users WHERE UserId = @userid ",
                                                                                            new { userid = userId });
                    return result;
                }
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
            var update_query = new StringBuilder(@"UPDATE Users SET FirstName=@FirstName, LastName = @LastName, Organisation = @Organisation ");

            if (profile.RoleId == 3)
                update_query.Append(",Outstandingdays=@Outstandingdays ");
            
            if(profile.PasswordHash != null && profile.PasswordHash != "")
                    update_query.Append(" ,PasswordHash=@passwordHash, LastPasswordChangedDate=GETDATE() ");

            update_query.Append(" WHERE UserId = @UserId");

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query.ToString(), profile);
                }
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
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<CompanyDetails>(@"SELECT Id, UserId,CompanyName,Address,AdditionalAddress,
                                            Town,PostCode,CountryId FROM CompanyDetails WHERE UserId = @userid ",
                                                new { userid = userId });
                }
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


        public async Task<ReturnResult> AddUser(User user)
        {
            try
            {
                bool exists = await CheckIfUserExists(user);
                if (exists)
                {
                    result.body = "A user with that email already exists.";
                    result.result = 0;
                    return result;
                }
                int profileId = 0;
                var insert_query = @"INSERT INTO Users(Email,FirstName,LastName,Password,DateCreated,Organisation,LastLoginTime,RoleId,
                                                        Activated,VerificationStatus,Outstandingdays,OperatorId,IsMsisdnMatch,IsEmailVerfication,
                                                        PhoneticAlphabet,IsMobileVerfication,OrganisationTypeId,UserMatchTableName)
                                                    VALUES(@Email,@FirstName,@LastName,@Password,GETDATE(),@Organisation,GETDATE(),@RoleId,
                                                            @Activated,@VerificationStatus,@Outstandingdays,@OperatorId,@IsMsisdnMatch,@IsEmailVerfication,
                                                            @PhoneticAlphabet,@IsMobileVerfication,@OrganisationTypeId,@UserMatchTableName);
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    profileId = await connection.ExecuteScalarAsync<int>(insert_query, user);

                    if (!(profileId > 0))
                    {
                        result.result = 0;
                        result.error = "ProfileInfo was NOT added successfully";
                        return result;
                    }
                    result.body = profileId;
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


        public async Task<ReturnResult> ApproveORSuspendUser(AdvertiserDashboardResult user)
        {
            var update_query = @"UPDATE Users SET Activated=@Activated WHERE UserId = @UserId";
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query, user);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "ApproveORSuspendUser"
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
            int registeredId = 0;
            var topRoleId = (int)Enums.UserRole.OperatorAdmin;

            try
            {
                bool userOperatorIdExist = false;
                var usr = new User { RoleId = topRoleId, OperatorId = model.OperatorId };

                // Checks to see if an Operator Admin already exist.
                userOperatorIdExist = await CheckIfUserExists(usr);


                if (userOperatorIdExist)
                {
                    result.body = "An Operator Admin already exists.";
                    result.result = 0;
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
                    ProcedureName = "AddOperator-CheckExists"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Checks for unique failed";
            }

            try
            {

                // Not sure if all users will have same defaults so left this here for now.
                // If it transpires that they have similar or the same defaults then may move.
                /// TODO: Check what other user types have as defaults.
                var command = new User();

                command.Email = model.Email;
                command.FirstName = model.FirstName;
                command.LastName = model.LastName;
                command.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.PasswordHash);
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


                var body = await AddUser(command);
                if (body.result == 0)
                {
                    result.result = 0;
                    result.error = body.error;
                    return result;
                }

                registeredId = (int)body.body;

                if (registeredId > 0)
                {
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
                    command1.UserId = registeredId;
                    command1.MobileNumber = model.MobileNumber;
                    command1.FixedLine = null;
                    command1.Email = model.Email;
                    command1.PhoneNumber = model.PhoneNumber;
                    command1.Address = model.Address;
                    command1.CountryId = model.CountryId;

                    var res = await AddContactInformation(command1);
                    if (res.result == 0)
                    {
                        result.result = 0;
                        result.error = res.error;
                        return result;
                    }

                    if ((int)res.body > 0)
                    {
                        // For scalability added an array of app config settings to retrieve specifically in this case for
                        // operator admin.
                        /// TODO: Sort out E-Mail settings ideally into one service for all.
                        var confSettings = new string[] { "OperatorAdminRegistrationEmailTemplete", "OperatorAdminUrl" };
                        // SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.PasswordHash, confSettings);
                        result.body = "Operator Admin registered for Operator " + model.FirstName + " " + model.LastName;
                    }
                    else
                    {
                        result.result = 0;
                        result.error = "Contact information was not added";
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
                    PageName = "OperatorService",
                    ProcedureName = "AddOperator-Adding"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Adding user failed";
            }
            return result;
        }


        /// <summary>
        /// Look if possible to tie into Update ProfileInfo.
        /// TODO: See if I can merge with Update Profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> UpdateOperatorAdminUser(OperatorAdminFormModel model)
        {
            try
            {
                if (model.PasswordHash != null && model.PasswordHash != "")
                {
                    string passwordHash = Md5Encrypt.Md5EncryptPassword(model.PasswordHash);
                    if (await _logonService.IsPreviousPassword(model.UserId, passwordHash))
                    {
                        result.error = "Cannot reuse old password.";
                        result.result = 0;
                        return result;
                    }
                }

                var command = new User
                {
                    UserId = model.UserId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Organisation = model.Organisation

                };

                bool passChanged = false;
                var password = model.PasswordHash;
                if (model.PasswordHash == null || model.PasswordHash == "")
                    command.PasswordHash = null;
                else
                {
                    command.PasswordHash = Md5Encrypt.Md5EncryptPassword(model.PasswordHash);
                    passChanged = true;
                }
                var x = await UpdateProfileForm(command);
                if (x.result == 0)
                    return x;

                var command1 = new Contacts
                {
                    Id = model.Id,
                    MobileNumber = model.MobileNumber,
                    FixedLine = model.FixedLine,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    CountryId = model.CountryId,
                };

                var y = await UpdateContactForm(command1);
                if (y.result == 0)
                    return y;

                /// TODO: Sort out a standard Email sending service
                if (passChanged)
                {
                    // SendEmailVerificationCode(model.FirstName, model.LastName, model.Email, model.PasswordHash);
                }

                var z = await _logonService.UpdatePasswordHistory(model.UserId, model.PasswordHash);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "UpdateOperatorAdminUser"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetOperatorAdmin(IdCollectionViewModel model)
        {
            try
            {
                var select_query = @"SELECT u.UserId,FirstName,LastName,Email,Organisation,u.OperatorId,o.CountryId,
                        c.Name AS CountryName,o.OperatorName,u.Activated,u.DateCreated,con.MobileNumber,con.PhoneNumber,
                        con.Address,con.Id
                        FROM Users AS u LEFT JOIN Operators AS o ON u.OperatorId=o.OperatorId
                        LEFT JOIN Country AS c ON o.CountryId=c.Id
                        LEFT JOIN Contacts AS con ON con.UserId=u.UserId
                        WHERE u.UserId=@userId";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<OperatorAdminFormModel>(select_query, new { userId = model.userId });
                }
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


        /// <summary>
        /// Checks if user exists by Email only. Some others such as Operator check if there is an existing admin user.
        /// I'm guessing only allowed one.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<bool> CheckIfUserExists(User model)
        {
            bool exists = false;
            if (model.RoleId == 6)
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    exists = connection.ExecuteScalar<bool>(@"SELECT COUNT(1) FROM Users 
                                                                    WHERE OperatorId=@OperatorId AND RoleId=@topRoleId",
                                                                  new { OperatorId = model.OperatorId, topRoleId = model.RoleId });
                }
            }
            else
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM Users WHERE LOWER(Email) = @email;",
                                                                  new { email = model.Email.ToLower() });
                }
            }
            return exists;
        }


        private async Task<bool> CheckIfContactExists(Contacts model)
        {
            bool exists = false;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM Contacts WHERE MobileNumber=@mobile;",
                                                              new { mobile = model.MobileNumber });
            }
            return exists;
        }

    }
}
