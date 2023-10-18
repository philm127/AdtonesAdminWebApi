using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class UserCreditDAL : BaseDAL, IUserCreditDAL
    {

        public UserCreditDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }

        public async Task<int> UpdateUserCredit(int id, decimal amt)
        {
            string UpdateUserCredit = @"UPDATE UsersCredit SET AvailableCredit=@AvailableCredit, UpdatedDate=GETDATE()
                                                WHERE UserId = @Id";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(UpdateUserCredit,
                             new { Id = id, AvailableCredit = amt }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddUserCredit(AdvertiserCreditFormCommand _creditmodel)
        {
            string AddUserCredit = @"INSERT INTO UsersCredit(UserId,AssignCredit,AvailableCredit,UpdatedDate,CreatedDate,CurrencyId) 
                                            VALUES(@UserId,@AssignCredit,@AssignCredit,GETDATE(),GETDATE(),@CurrencyId)";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(AddUserCredit, _creditmodel));
            }
            catch
            {
                throw;
            }
        }


        public async Task<UserCreditDetailsDto> GetUserCreditDetail(int id)
        {
            string UserCreditDetails = @"SELECT uc.Id,uc.UserId,AssignCredit,CONCAT(usr.FirstName,' ',usr.LastName) AS FullName,
                                                    ISNULL(CAST(AvailableCredit AS decimal(18,2)),0) AS AvailableCredit,
                                                    CreatedDate,uc.CurrencyId,c.CountryId 
                                                    FROM  UsersCredit AS uc
                                                    INNER JOIN Users AS usr ON usr.UserId=uc.UserId
                                                    LEFT JOIN Currencies AS c ON c.CurrencyId=uc.CurrencyId
                                                    WHERE uc.UserId=@Id";
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<UserCreditDetailsDto>(UserCreditDetails, new { Id = id }));
            }
            catch (Exception ex)
            {
                var msg = ex.Message.ToString();
                throw;
            }
        }


        public async Task<bool> CheckUserCreditExist(int userId)
        {
            string CheckIfUserExists = @"SELECT COUNT(1) FROM UsersCredit WHERE UserId=@userId";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>(CheckIfUserExists,
                                                                    new { userId = userId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<decimal> GetAvailableCredit(int userId)
        {
            string GetAvailableCredit = @"SELECT AvailableCredit FROM UsersCredit WHERE UserId=@Id";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.QueryFirstOrDefault<decimal>(GetAvailableCredit,new { Id = userId }));
            }
            catch
            {
                throw;
            }
        }

    }
}
