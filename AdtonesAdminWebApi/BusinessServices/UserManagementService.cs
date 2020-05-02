using AdtonesAdminWebApi.BusinessServices.Interfaces;
using Dapper;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using AdtonesAdminWebApi.Model;
using System;
using Microsoft.Extensions.Logging;
using AdtonesAdminWebApi.Services;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public UserManagementService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        public async Task<ReturnResult> LoadDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<AdvertiserUserResult>(UserResultQuery());
                    
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserManagementService",
                    ProcedureName = "LoadDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
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
                    var x = await connection.QueryAsync<int>(insert_query, contact);
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
                                PhoneNumber=@PhoneNumber,Address=@Address,CountryId=@CountryId,CurrencyId=@CurrencyId
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
            var update_query = @"UPDATE Users SET RoleId=@RoleId,FirstName=@FirstName,LastName=@LastName,Outstandingdays=@Outstandingdays,
                                Organisation=@Organisation WHERE UserId = @UserId";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(update_query, profile);
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
                                                    VALUES(@Email,@FirstName,@LastName,@Password,GETDATE(),@Organisation,@LastLoginTime,@RoleId,
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


        public async Task<ReturnResult> ApproveORSuspendUser(AdvertiserUserResult user)
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

        private async Task<bool> CheckIfUserExists(User model)
        {
            bool exists = false;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM Users WHERE LOWER(Email) = @email;",
                                                              new { email = model.Email.ToLower() });
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


        #region Long SQL Queries
        
        
        private string UserResultQuery()
        {
            return @"SELECT item.UserId,item.RoleId,item.Email,item.FirstName,item.LastName,
                          ISNULL(camp.NoOfactivecampaign, 0) AS NoOfactivecampaign,
                           ISNULL(ad.NoOfunapprovedadverts, 0) AS NoOfunapprovedadverts,
                           ISNULL(cred.AssignCredit, 0) AS creditlimit,ISNULL(billit.outStandingInvoice, 0) AS outStandingInvoice,
                           item.Activated,item.DateCreated,ISNULL(tkt.TicketCount, 0) AS TicketCount
                           FROM
                                (SELECT item.UserId, item.RoleId, item.Email, item.DateCreated, item.Activated,
                                item.FirstName,item.LastName
                                FROM Users item Where item.VerificationStatus = 1 AND (item.RoleId = 1 OR item.RoleId = 3)) item
                            LEFT JOIN
                                (SELECT a.[UserId], b.[AssignCredit], a.[Id] FROM
                                (SELECT[UserId], MIN(Id) AS Id FROM UsersCredit GROUP BY[UserId]) a
                                INNER JOIN UsersCredit b ON a.[UserId] = b.[UserId] AND a.Id = b.Id) cred
                            ON item.UserId = cred.UserId
                            LEFT JOIN
                                (SELECT COUNT(UserId)as TicketCount, UserId FROM Question WHERE Status IN (1, 2) GROUP BY UserId) tkt
                            ON item.UserId = tkt.UserId
                            LEFT JOIN
                                (SELECT COUNT(CampaignProfileId) AS NoOfactivecampaign,UserId FROM Campaignprofile WHERE Status IN (4, 3, 2, 1) GROUP BY UserId) camp
                            ON item.UserId = camp.UserId
                            LEFT JOIN
                                (SELECT COUNT(AdvertId) AS NoOfunapprovedadverts,UserId FROM Advert WHERE Status = 4 GROUP BY UserId) ad
                            ON item.UserId = ad.UserId
                            LEFT JOIN
                                (SELECT COUNT(bill3.UserId) AS outStandingInvoice,bill3.UserId
                                FROM
                                    (SELECT SUM(FundAmount) AS totalAmount, CampaignProfileId, UserId
                                    FROM Billing WHERE PaymentMethodId = 1 GROUP BY CampaignProfileId, UserId) bill3
                                LEFT JOIN
                                    (SELECT sum(Amount) AS paidAmount, UserId, CampaignProfileId
                                    FROM UsersCreditPayment GROUP BY CampaignProfileId, UserId) uc
                                ON bill3.UserId = uc.UserId AND bill3.CampaignProfileId = uc.CampaignProfileId
                                WHERE (ISNULL(bill3.totalAmount, 0) - ISNULL(uc.paidAmount, 0)) > 0
                                GROUP BY bill3.UserId) billit
                            ON item.UserId = billit.UserId;";
        }


        #endregion
    }
}
