using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using AdtonesAdminWebApi.Services;
using Domain.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SalesManagementDAL : BaseDAL, ISalesManagementDAL
    {
        public SalesManagementDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<AllocationList>> GetAllocationLists(int userId = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            try
            {
                if (userId > 0)
                {
                    sb.Append(SalesManagementQuery.GetAllocatedBySalesExec);
                    builder.AddParameters(new { userId = userId });
                }
                else
                    sb.Append(SalesManagementQuery.GetUnallocated);

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
            var ytr = _httpAccessor.GetUserIdFromJWT();
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    return await connection.QueryAsync<SharedSelectListViewModel>(SalesManagementQuery.GetSalesExecDDList, new { Id = ytr });
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(SalesManagementQuery.CheckAdvertiserExists);
            builder.AddParameters(new { Id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>(select.RawSql, select.Parameters));

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

            x = await _executers.ExecuteCommand(_connStr,
                                                conn => conn.ExecuteScalar<int>(SalesManagementQuery.UpdateSalesToAdvertiserToInActive, new
                                                {
                                                    Sid = sp,
                                                    AdId = ad
                                                }));
            return x;
        }


        public async Task<int> InsertToSalesAd(int sp, int ad)
        {
            var manId = _httpAccessor.GetUserIdFromJWT();
            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(SalesManagementQuery.InsertNewAdToSales, new
                         {
                             Sid = sp,
                             AdId = ad,
                             ManId = manId,
                             Suppress = true
                         }));
            return x;
        }


        public async Task<int> InsertNewAdvertiserToSalesExec(int sp, int ad, bool mail)
        {
            var Id = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(SalesManagementQuery.GetSalesManagerId, new
                         {
                             Id = sp
                         }));
            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(SalesManagementQuery.InsertNewAdToSales, new
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
            var manId = _httpAccessor.GetUserIdFromJWT();
            var x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(SalesManagementQuery.UpdateAdToSales, new
                         {
                             Sid = sp,
                             AdId = ad
                         }));
            return x;
        }


        public async Task<string> GetSalesExecInvoiceMailDets(int advertiserId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<string>(SalesManagementQuery.GetSalesExecInvDets, new { Id = advertiserId }));

            }
            catch
            {
                throw;
            }
        }
    }
}