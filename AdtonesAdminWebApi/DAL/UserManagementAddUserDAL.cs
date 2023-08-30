using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class UserManagementAddUserDAL : IUserManagementAddUserDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConnectionStringService _connService;

        public UserManagementAddUserDAL(IConfiguration configuration, IExecutionCommand executers, IHttpContextAccessor httpAccessor,
                                   IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _httpAccessor = httpAccessor;
            _connService = connService;
        }

        public async Task<bool> CheckIfUserExists(UserAddFormModel model)
        {
            bool exists = false;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                exists = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<bool>("SELECT COUNT(1) FROM Users WHERE LOWER(Email) = @email;", 
                                                                                    new { email = model.Email.ToLower() }));
            }
            return exists;
        }


        public async Task<bool> VerifyEmail(string code)
        {
            var model = new UserAddFormModel();

            model.Email = EncryptionHelper.DecryptSingleValue(code);
            bool exists = await CheckIfUserExists(model);

            if (exists)
            {
                var sql = @"UPDATE Users SET VerificationStatus=true,IsEmailVerfication=true WHERE LOWER(Email)=@email AND RoleId=3";
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var cnt = await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(sql, new { email = model.Email.ToLower() }));
                }
            }
            return exists;
        }


        public async Task<bool> CheckIfContactExists(Contacts model)
        {
            bool exists = false;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                exists = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<bool>("SELECT COUNT(1) FROM Contacts WHERE MobileNumber=@mobile;", 
                                                                                new { mobile = model.MobileNumber }));
            }
            return exists;
        }


        public async Task<int> AddNewUser(UserAddFormModel model)
        {
            string addNewUser = @"INSERT INTO Users(Email,FirstName,LastName,PasswordHash,DateCreated,Organisation,LastLoginTime,RoleId,
                                                Activated,VerificationStatus,Outstandingdays,OperatorId,IsMsisdnMatch,IsEmailVerfication,
                                                PhoneticAlphabet,IsMobileVerfication,OrganisationTypeId,UserMatchTableName,
                                                Permissions,LastPasswordChangedDate)
                                  VALUES(@Email,@FirstName,@LastName,@PasswordHash,GETDATE(),@Organisation,GETDATE(),@RoleId,
                                         @Activated,@VerificationStatus,@Outstandingdays,@OperatorId,@IsMsisdnMatch,@IsEmailVerfication,
                                         @PhoneticAlphabet,@IsMobileVerfication,@OrganisationTypeId,@UserMatchTableName,
                                         @Permissions,GETDATE());
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(addNewUser, model));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddNewUserToOperator(UserAddFormModel model)
        {
            string addNewUserToOperator = @"INSERT INTO Users(Email,FirstName,LastName,PasswordHash,DateCreated,Organisation,LastLoginTime,RoleId,
                                                Activated,VerificationStatus,Outstandingdays,OperatorId,IsMsisdnMatch,IsEmailVerfication,
                                                PhoneticAlphabet,IsMobileVerfication,OrganisationTypeId,UserMatchTableName,AdtoneServerUserId,LastPasswordChangedDate)
                                             VALUES(@Email,@FirstName,@LastName,@PasswordHash,GETDATE(),@Organisation,GETDATE(),@RoleId,
                                                    @Activated,@VerificationStatus,@Outstandingdays,@OperatorId,@IsMsisdnMatch,@IsEmailVerfication,
                                                    @PhoneticAlphabet,@IsMobileVerfication,@OrganisationTypeId,@UserMatchTableName,@AdtoneServerUserId,GETDATE());
                                              SELECT CAST(SCOPE_IDENTITY() AS INT);";

            int x = 0;
            try
            {
                var conns = new List<string>();
                if (model.RoleId == (int)Enums.UserRole.Admin || model.RoleId == (int)Enums.UserRole.AdvertAdmin ||
                    model.RoleId == (int)Enums.UserRole.ProfileAdmin || model.RoleId == (int)Enums.UserRole.UserAdmin
                    || model.CountryId == 0)
                {
                    conns = await _connService.GetConnectionStrings();
                }
                else if (model.RoleId == (int)Enums.UserRole.Advertiser)
                {
                    conns = await _connService.GetConnectionStringsByCountry(model.CountryId);
                }
                else
                {
                    conns.Add(await _connService.GetConnectionStringByOperator(model.OperatorId.Value));
                }

                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                        x = await _executers.ExecuteCommand(constr,
                             conn => conn.ExecuteScalar<int>(addNewUserToOperator, model));
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddNewContact(Contacts model)
        {
            string addNewContact = @"INSERT INTO Contacts(UserId,MobileNumber,FixedLine,Email,PhoneNumber,Address,CountryId,CurrencyId,AdtoneServerContactId)
                                               VALUES(@UserId,@MobileNumber,@FixedLine,@Email,@PhoneNumber,@Address,@CountryId,@CurrencyId,@AdtoneServerContactId);
                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(addNewContact, model));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddNewContactToOperator(Contacts model)
        {
            string addNewContact = @"INSERT INTO Contacts(UserId,MobileNumber,FixedLine,Email,PhoneNumber,Address,CountryId,CurrencyId,AdtoneServerContactId)
                                               VALUES(@UserId,@MobileNumber,@FixedLine,@Email,@PhoneNumber,@Address,@CountryId,@CurrencyId,@AdtoneServerContactId);
                                                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            int x = 0;
            try
            {
                var conns = new List<string>();
                if (model.RoleId == (int)Enums.UserRole.Admin || model.RoleId == (int)Enums.UserRole.AdvertAdmin ||
                    model.RoleId == (int)Enums.UserRole.ProfileAdmin || model.RoleId == (int)Enums.UserRole.UserAdmin ||
                    model.CountryId == null || model.CountryId.Value == 0)
                    conns = await _connService.GetConnectionStrings();
                else
                    conns = await _connService.GetConnectionStringsByCountry(model.CountryId.Value);


                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                        x = await _executers.ExecuteCommand(constr,
                             conn => conn.ExecuteScalar<int>(addNewContact, model));
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddCompanyDetails(CompanyDetails company)
        {
            var insert_query = @"INSERT INTO CompanyDetails(CompanyName,Address,AdditionalAddress,Town,PostCode,CountryId,UserId)
                                             VALUES(@CompanyName,@Address,@AdditionalAddress,@Town,@PostCode,@CountryId,@UserId);
                                             SELECT CAST(SCOPE_IDENTITY() AS INT);";
            var x = 0;

            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    await connection.OpenAsync();
                    x = await connection.ExecuteScalarAsync<int>(insert_query, company);
                }
            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> AddCompanyDetailsToOperator(CompanyDetails company)
        {
            var constr = await _connService.GetConnectionStringByOperator(1);
            var update_query = @"INSERT INTO CompanyDetails(CompanyName,Address,AdditionalAddress,Town,PostCode,CountryId,UserId,AdtoneServerCompanyDetailId)
                                                VALUES(@CompanyName,@Address,@AdditionalAddress,@Town,@PostCode,@CountryId,@UserId,@AdtoneServerCompanyDetailId);";

            try
            {
                using (var connection = new SqlConnection(constr))
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteScalarAsync<int>(update_query, company);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// If Contact form is not added for instance duplicate mobile number will also remove entry
        /// in main table. Insert into Operator table is done after so don't need to concern with that
        /// table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> DeleteNewUser(int userId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>("DELETE FROM Users WHERE UserId=@Id", new { Id = userId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RollbackUserDetails(int userId)
        {
            try
            {
                await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>("DELETE FROM CompanyDetails WHERE UserId=@Id", new { Id = userId }));
                await _executers.ExecuteCommand(_connStr,
                                            conn => conn.ExecuteScalar<int>("DELETE FROM Contacts WHERE UserId=@Id", new { Id = userId }));
                await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>("DELETE FROM Users WHERE UserId=@Id", new { Id = userId }));
                return 1;
                
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> RollbackRemoteUserDetails(int userId, int contactId)
        {
            var constr = await _connService.GetConnectionStringByOperator(1);
            try
            {
                if (contactId > 0)
                    await _executers.ExecuteCommand(constr,
                                                  conn => conn.ExecuteScalar<int>("DELETE FROM Contacts WHERE ContactId=@Id", new { Id = contactId }));
                await _executers.ExecuteCommand(constr,
                         conn => conn.ExecuteScalar<int>("DELETE FROM Users WHERE UserId=@Id", new { Id = userId }));
                return 1;

            }
            catch
            {
                throw;
            }
        }

        public async Task<int> InsertManagerToSalesExec(int manId, int execId)
        {
            string insertManagerToSalesExec = @"INSERT INTO SalesManager_SalesExec(ManId,ExecId,Active,CreatedDate,UpdatedDate)
                                                            VALUES(@manId,@execId,1,GETDATE(),GETDATE());";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                     conn => conn.ExecuteScalar<int>(insertManagerToSalesExec, new { manId = manId, execId = execId }));
            }
            catch
            {
                throw;
            }
        }
    }
}
