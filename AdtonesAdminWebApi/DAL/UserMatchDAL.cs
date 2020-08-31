using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class UserMatchDAL : IUserMatchDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;

        public UserMatchDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
        }


        public async Task<int> UpdateMediaLocation(string conn, string media, int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserMatchQuery.UpdateMediaLocation);
            try
            {
                builder.AddParameters(new { media = media });
                builder.AddParameters(new { campaignProfileId = id });

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task PrematchProcessForCampaign(int campaignId, string conn)
        {
            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("CampaignUserMatchSpByCampaignId",
                                                                    new { CampaignProfileId = campaignId }, 
                                                                    commandType: CommandType.StoredProcedure);
            }
            
        }


        public async Task<CampaignBudgetModel> GetBudgetAmounts(int campaignId, string conn)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserMatchQuery.GetBudgetUpdateAmount);
            try
            {
                builder.AddParameters(new { Id = campaignId });

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.QueryFirstOrDefault<CampaignBudgetModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateBucketCount(int campaignId, string conn, int bucketCount)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserMatchQuery.UpdateBucketAmount);
            try
            {
                builder.AddParameters(new { Id = campaignId });
                builder.AddParameters(new { BucketCount = bucketCount });

                return await _executers.ExecuteCommand(conn,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task PrematchUserProcess(int campaignId, string conn)
        {
            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("NAME OF SP",
                                                                    new { CampaignProfileId = campaignId },
                                                                    commandType: CommandType.StoredProcedure);
            }

        }

    }
}
