using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class CheckExistsDAL : ICheckExistsDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;

        public CheckExistsDAL(IConfiguration configuration, IExecutionCommand executers)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
        }

        public async Task<bool> CheckAreaExists(string command, AreaResult areamodel)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { areaname = areamodel.AreaName.Trim().ToLower() });
            builder.AddParameters(new { countryId = areamodel.CountryId });

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


        public async Task<bool> CheckCampaignBillingExists(string command, int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { Id = campaignId });

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
    }
}
