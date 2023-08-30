using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class SalesManagementDAL : BaseDAL, ISalesManagementDAL
    {
        public SalesManagementDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<AllocationList>> GetAllocationLists(int userId = 0)
        {
            string getUnallocated = @"SELECT u.UserId,CONCAT(u.FirstName,' ',u.LastName) AS FullName FROM Users AS u
                                                   INNER JOIN Contacts AS con ON con.UserId=u.UserId
                                                   WHERE RoleId=3 AND Activated=1 AND u.UserId NOT IN 
                                                    (SELECT AdvertiserId FROM Advertisers_SalesTeam WHERE IsActive=1) ";


            string getAllocatedBySalesExec = @"SELECT u.UserId,CONCAT(u.FirstName,' ',u.LastName) AS FullName FROM Users AS u 
                                                            INNER JOIN Contacts AS con ON con.UserId=u.UserId
                                                            WHERE RoleId=3 AND u.UserId IN 
                                                                (SELECT AdvertiserId FROM Advertisers_SalesTeam 
                                                                  WHERE IsActive=1 AND SalesExecId=@userId) ";

        var sb = new StringBuilder();
            var builder = new SqlBuilder();
            try
            {
                if (userId > 0)
                {
                    sb.Append(getAllocatedBySalesExec);
                    builder.AddParameters(new { userId = userId });
                }
                else
                    sb.Append(getUnallocated);

                var values = CheckGeneralFile(sb, builder, pais: "con");
                sb = values.Item1;
                builder = values.Item2;

                var select = builder.AddTemplate(sb.ToString());
                return await _executers.ExecuteCommand(_connStr,
                                   conn => conn.Query<AllocationList>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetsalesExecDDList()
        {
            string getSalesExecDDList = @"SELECT u.UserId AS Value,CONCAT(u.FirstName,' ',u.LastName) AS Text FROM Users AS u
                                          INNER JOIN SalesManager_SalesExec AS s ON u.UserId=s.ExecId 
                                            WHERE Active=1 AND ManId=@Id";


        var ytr = _httpAccessor.GetUserIdFromJWT();
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    return await connection.QueryAsync<SharedSelectListViewModel>(getSalesExecDDList, new { Id = ytr });
                }
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Check if an advertiser exists in the Advertisers_SalesTeam table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckIfAdvertiserExists(int id)
        {

            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    await connection.OpenAsync();
                    return await connection.ExecuteScalarAsync<bool>("SELECT COUNT(1) FROM Advertisers_SalesTeam WHERE AdvertiserId = @Id",
                                                                        new { Id = id });
                }

            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateInactiveForSP(int sp, int ad)
        {
            int x = 0;
            var userId = _httpAccessor.GetUserIdFromJWT();

            string updateSalesToAdvertiserToInActive = @"UPDATE Advertisers_SalesTeam SET IsActive=0,UpdatedDate=GETDATE() 
                                                         WHERE AdvertiserId=@AdId AND SalesExecId=@Sid";


            using (var connection = new SqlConnection(_connStr))
            {
                await connection.OpenAsync();
                x = await connection.ExecuteScalarAsync<int>(updateSalesToAdvertiserToInActive, new
                {
                    Sid = sp,
                    AdId = ad
                });
            }
            return x;
        }


        public async Task<int> InsertToSalesAd(int sp, int ad)
        {
            string insertNewAdToSales = @"INSERT INTO Advertisers_SalesTeam(AdvertiserId,SalesExecId,SalesManId,MailSupressed,
                                                                                IsActive,CreatedDate,UpdatedDate)
                                          VALUES(@AdId,@Sid, @ManId, @Suppress,1,GETDATE(), GETDATE())";

            var manId = _httpAccessor.GetUserIdFromJWT();
            int x;
            using (var connection = new SqlConnection(_connStr))
            {
                await connection.OpenAsync();
                x = await connection.ExecuteScalarAsync<int>(insertNewAdToSales, new
                {
                    Sid = sp,
                    AdId = ad,
                    ManId = manId,
                    Suppress = true
                });
            }
            return x;
        }


        public async Task<int> InsertNewAdvertiserToSalesExec(int sp, int ad, bool mail)
        {
            string insertNewAdToSales = @"INSERT INTO Advertisers_SalesTeam(AdvertiserId,SalesExecId,SalesManId,MailSupressed,
                                                                                IsActive,CreatedDate,UpdatedDate)
                                          VALUES(@AdId,@Sid, @ManId, @Suppress,1,GETDATE(), GETDATE())";



            var Id = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>("SELECT ManId FROM SalesManager_SalesExec WHERE ExecId=@Id", new
                                                                                                                         {
                                                                                                                             Id = sp
                                                                                                                         }));
            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(insertNewAdToSales, new
                         {
                             Sid = sp,
                             AdId = ad,
                             ManId = Id,
                             Suppress = mail
                         }));
            return x;
        }


        public async Task<int> UpdateUserForSP(int sp, int ad)
        {
            string updateAdToSales = @"UPDATE Advertisers_SalesTeam SET SalesExecId=@Sid, IsActive=1,UpdatedDate=GETDATE() 
                                                    WHERE AdvertiserId=@AdId";
        var manId = _httpAccessor.GetUserIdFromJWT();
            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(updateAdToSales, new
                         {
                             Sid = sp,
                             AdId = ad
                         }));
            return x;
        }


        public async Task<string> GetSalesExecInvoiceMailDets(int advertiserId)
        {
            string getSalesExecInvDets = @"SELECT u.Email FROM Users AS u INNER JOIN Advertisers_SalesTeam AS adsal 
                                                        ON adsal.SalesExecId=u.UserId WHERE MailSupressed=1 AND IsActive=1
                                                        AND AdvertiserId=@Id";
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<string>(getSalesExecInvDets, new { Id = advertiserId }));

            }
            catch
            {
                throw;
            }
        }
    }
}