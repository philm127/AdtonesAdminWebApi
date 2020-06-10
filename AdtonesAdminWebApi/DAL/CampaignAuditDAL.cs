using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{

    public class CampaignAuditDAL : ICampaignAuditDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;
        private readonly ICampaignAuditQuery _commandText;
        private IMemoryCache _cache;

        public CampaignAuditDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, ICampaignAuditQuery commandText,
                                IMemoryCache cache)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
            _commandText = commandText;
            _cache = cache;
        }


        public async Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesOperators(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(_commandText.GetCampaignDashboardSummaries);
            sb.Append(" cp.CampaignProfileId=@campId;");
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { campId = id });
            try
            {

               return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesForCampaign(int campaignId)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            string key = $"DASHBOARD_STATS_{campaignId}";
            return await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry => {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                return GetCampaignDashboardSummariesOperators(campaignId);
            });
            // return cacheEntry ?? new List<CampaignDashboardChartPREResult>();
        }


        public async Task<List<CampaignDashboardChartPREResult>> GetCampaignDashboardSummariesAdvertisers(int campid = 0, int userId = 0)
        {
            int? campId = null;
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(_commandText.GetCampaignDashboardSummaries);
            if (campid > 0)
                campId = campid;

            sb.Append(" u.UserId=@userId and(@campaignId is null or(cp.CampaignProfileId = @campaignId)); ");
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { campId = campId });
            builder.AddParameters(new { userId = userId });

            try
            {

                var x =  await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters));
                var result = x.AsList<CampaignDashboardChartPREResult>();
                return result;
            }
            catch
            {
                throw;
            }
        }

    }
}
