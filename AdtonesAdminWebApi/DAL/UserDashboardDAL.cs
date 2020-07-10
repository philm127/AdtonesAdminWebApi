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

namespace AdtonesAdminWebApi.DAL
{
    public class UserDashboardDAL : IUserDashboardDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;

        public UserDashboardDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboard(int operatorId=0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserDashboardQuery.AdvertiserResultQuery);
            if(operatorId > 0)
                builder.AddParameters(new { operatorId = operatorId });

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
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<OperatorDashboardResult>(UserDashboardQuery.OperatorResultQuery));
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
