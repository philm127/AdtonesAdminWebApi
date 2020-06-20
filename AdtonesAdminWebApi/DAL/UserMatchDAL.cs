using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
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
        private readonly IUserMatchQuery _commandText;

        public UserMatchDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                            IUserMatchQuery commandText)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _commandText = commandText;
        }


        public async Task<int> UpdateMediaLocation(string conn, string media, int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(_commandText.UpdateMediaLocation);
            try
            {
                builder.AddParameters(new { media = media });
                builder.AddParameters(new { campaignProfileId = id });

                return await _executers.ExecuteCommand(_connStr,
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

    }
}
