using AdtonesAdminWebApi.DAL.Interfaces;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class UserProfileDAL : IUserProfileDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;

        public UserProfileDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }


        public async Task PrematchProcessForCampaign(int campaignId, string conn)
        {
            string _connStr = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync("CampaignUserMatchSpByCampaignId",
                                                                    new { CampaignProfileId = campaignId },
                                                                    commandType: CommandType.StoredProcedure);
            }

        }


        public async Task<int> GetProfileMatchInformationId(int countryId)
        {
            using (var connection = new SqlConnection(conn))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(UserMatchQuery.GetProfileMatchLabels, new { Id = infoId }));
            }

            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<int>(UserMatchQuery.GetProfileMatchInformationId,
                                                                                                                        new { Id = countryId }));
            }

        }


        public async Task<IEnumerable<string>> GetProfileMatchLabels(int infoId)
        {
            using (var connection = new SqlConnection(_connStr))
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<string>(UserMatchQuery.GetProfileMatchLabels, new { Id = infoId }));
            }

        }
    }
}
