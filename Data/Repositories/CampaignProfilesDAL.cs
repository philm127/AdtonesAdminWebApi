using Microsoft.Extensions.Configuration;
using AdtonesAdminWebApi.Services;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using Data.Repositories.Queries;
using System.Collections.Generic;

namespace Data.Repositories
{
    public static class CampaignProfilesDAL
    {
        private static readonly string _connStr = ConfigHelper.AppSetting("DefaultConnection");


        public static async Task<List<string>> GetProfileMatchLabels(int countryId, string profileName)
        {
            using (var connection = new SqlConnection(_connStr))
            { 
                await connection.OpenAsync();
                var profId = await connection.QueryFirstAsync<int>(UserMatchQuery.GetProfileMatchInformationId,
                                                                                        new { Id = countryId, profileName = profileName });

                var labels = await connection.QueryAsync<string>(UserMatchQuery.GetProfileMatchLabels, new { Id = profId });

                List<string> labelList = labels.AsList();
                return labelList;

            }

        }


    }


}
