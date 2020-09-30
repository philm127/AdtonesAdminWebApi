using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Enums;
using AdtonesAdminWebApi.DAL.Queries;
using System.Text;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.Services;

namespace AdtonesAdminWebApi.DAL
{
    public class UserDashboardDAL : BaseDAL, IUserDashboardDAL
    {

        public UserDashboardDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboard(int operatorId=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            
            if (operatorId > 0)
            {
                sb.Append(UserDashboardQuery.OperatorAdvertiserResultQuery);
                builder.AddParameters(new { operatorId = operatorId });
            }
            else
            {
                var ytr = _httpAccessor.GetRoleIdFromJWT();
                if (ytr == (int)Enums.UserRole.SalesManager)
                {
                    sb.Append(UserDashboardQuery.SalesManagerAdvertiserResultQuery);
                }
                else
                {
                    sb.Append(UserDashboardQuery.AdvertiserResultQuery);
                }
                sb.Append(" WHERE 1=1 ");
                var values = CheckGeneralFile(sb, builder, pais: "cont", ops: "op", advs: "item");
                sb = values.Item1;
                builder = values.Item2;
            }

            var select = builder.AddTemplate(sb.ToString());

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<AdvertiserDashboardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<OperatorDashboardResult>> GetOperatorDashboard()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            try
            {
                sb.Append(UserDashboardQuery.OperatorResultQuery);
                var values = CheckGeneralFile(sb, builder, pais: "o", ops: "o");
                sb = values.Item1;
                builder = values.Item2;

                sb.Append(" ORDER BY u.DateCreated DESC");

                var select = builder.AddTemplate(sb.ToString());
                return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.Query<OperatorDashboardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdminDashboardResult>> GetAdminDashboard()
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.Query<AdminDashboardResult>(UserDashboardQuery.AdminResultQuery));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SubscriberDashboardResult>> GetSubscriberDashboard()
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<SubscriberDashboardResult>(UserDashboardQuery.SubscriberResultQuery));
            }
            catch
            {
                throw;
            }
        }

    }
}
