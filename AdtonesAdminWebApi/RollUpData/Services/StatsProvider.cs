using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.RollUpData.DataObjects;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace AdtonesAdminWebApi.RollUpData.Services
{
    public class StatsProvider : IStatsProvider
    {
        private IConfiguration Configuration { get; }

        private IConnectionStringService _connService;
        public readonly string _connStr;

        public StatsProvider(IConfiguration configuration, IConnectionStringService connService)
        {
            Configuration = configuration;
            _connService = connService;
            _connStr = Configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<ConsolidatedStatsDao> GetConsolidatedStatsAsync(StatsDetailLevels detailLevel, int entityId, ICurrencyConverter converter)
        {
            try
            {
                ConsolidatedStatsDao result = new ConsolidatedStatsDao { DetailLevel = detailLevel.ToSqlValue() };
                switch (detailLevel)
                {
                    case StatsDetailLevels.Campaign:
                        result.CampaignId = entityId;
                        break;
                    case StatsDetailLevels.Advertiser:
                        result.AdvertiserId = entityId;
                        break;
                    case StatsDetailLevels.Operator:
                        result.OperatorId = entityId;
                        break;
                    default:
                        throw new ArgumentException("DetailLevel must be specified to either Campaign, Advertiser or Operator levels.", nameof(detailLevel));
                }
                var raw = await GetDashboardStatsAsync(detailLevel, result.OperatorId, result.AdvertiserId, result.CampaignId);
                result.Dashboard = raw.Select(c => c.ConvertValuesIfNeeded(converter)).ToList();
                result.DashboardReduced = raw.Reduce(detailLevel, converter);
                result.Reach = await GetReachStatsAsync(detailLevel, entityId);
                result.SpikeLengths = await GetSpikePlayLengthAsync(detailLevel, entityId);
                result.PlaysByPeriods = await GetPeriodPlayDataAsync(detailLevel, entityId);
                result.PlaysByPeriods = result.PlaysByPeriods.Reduce();

                return result;
            }
            catch (Exception e)
            {
                return new ConsolidatedStatsDao
                {
                    DetailLevel = detailLevel.ToSqlValue(),
                    PlaysByPeriods = new PeriodicIntegerData(),
                    Dashboard = new List<DashboardSummariesDao>(0),
                    DashboardReduced = new DashboardSummariesDao(),
                    Reach = new ReachDao(),
                    SpikeLengths = new SpikePlayLengthsDao()
                };
            }
        }

        public async Task<PeriodicIntegerData> GetPeriodPlayDataAsync(StatsDetailLevels detailLevel, int entityId)
        {
            PeriodicIntegerData result = new PeriodicIntegerData();
            string sql = $@"select 
                ReportType as {nameof(PeriodicIntegerValue.PeriodString)}, 
                PeriodOfPlay as {nameof(PeriodicIntegerValue.PeriodName)}, 
                StatValue as {nameof(PeriodicIntegerValue.Value)}
                from RollupsCampaignLastPeriodInteger
                where DetailLevel=@detailLevel";
            switch (detailLevel)
            {
                case StatsDetailLevels.Campaign:
                    result.CampaignId = entityId;
                    sql = $"{sql} and CampaignId=@entityId and OperatorId is not null and AdvertiserId is not null";
                    break;
                case StatsDetailLevels.Advertiser:
                    result.AdvertiserId = entityId;
                    sql = $"{sql} and AdvertiserId=@entityId and OperatorId is null and CampaignId is null";
                    break;
                case StatsDetailLevels.Operator:
                    result.OperatorId = entityId;
                    sql = $"{sql} and OperatorId=@entityId and AdvertiserId is null and CampaignId is null";
                    break;
                default:
                    throw new ArgumentException("DetailLevel must be specified to either Campaign, Advertiser or Operator.");
            }

            using (var conn = new SqlConnection(_connStr))
            {
                await conn.OpenAsync();
                var stats = await conn.QueryAsync<PeriodicIntegerValue>(sql, new {@detailLevel=detailLevel.ToSqlValue(), @entityId = entityId});
                result.Values.AddRange(stats);
                return result;
            }
        }

        public async Task<List<DashboardSummariesDao>> GetDashboardStatsAsync(StatsDetailLevels detailLevel, int? operatorId, int? advertiserId, int? campaignId)
        {

            SqlExecutionContext conditionWithParams=null;
            switch (detailLevel)
            {
                case StatsDetailLevels.Campaign:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = "select * from RollupsCampaign where DetailLevel='C' and CampaignId=@campaignId",
                        Param = new { @campaignId = campaignId }
                    };
                    break;
                case StatsDetailLevels.Advertiser:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = "select * from RollupsCampaign where DetailLevel='C' and AdvertiserId=@advertiserId",
                        Param = new { @advertiserId=advertiserId }
                    };
                    break;
                case StatsDetailLevels.Operator:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = "select * from RollupsCampaign where (DetailLevel='C' and OperatorId=@operatorId",
                        Param = new { @operatorId = operatorId }
                    };
                    break;
                default:
                    throw new ArgumentException("DetailLevel must be provided and set to either Campaing, Advertiser or Operator values", nameof(detailLevel));
            }


            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                await conn.OpenAsync();
                var dashboardResult = await conn.QueryAsync<DashboardSummariesDao>(
                    conditionWithParams.Sql, conditionWithParams.Param);
                return dashboardResult.ToList();
            }
        }

        public async Task<SpikePlayLengthsDao> GetSpikePlayLengthAsync(StatsDetailLevels detailLevel, int entityId)
        {
            string selectPart = @"select 
	PlaysBelow6 as p1,
	Plays6to9 as p2,
	Plays9to12 as p3,
	Plays12to15 as p4,
	Plays15to18 as p5,
	Plays18to21 as p6,
	Plays21to24 as p7,
	Plays24to27 as p8,
	Plays27to30 as p9,
	PlaysMore30 as p10 
	from RollupsPlaySpike";
            SpikePlayLengthsDao result = new SpikePlayLengthsDao { DetailLevel = detailLevel.ToSqlValue() };
            SqlExecutionContext conditionWithParams = null;
            switch (detailLevel)
            {
                case StatsDetailLevels.Campaign:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = $"{selectPart} where DetailLevel='C' and CampaignId=@campaignId",
                        Param = new { @campaignId = entityId }
                    };
                    result.CampaignId = entityId;
                    break;
                case StatsDetailLevels.Advertiser:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = $"{selectPart} where DetailLevel='A' and AdvertiserId=@advertiserId",
                        Param = new { @advertiserId = entityId }
                    };
                    result.AdvertiserId = entityId;
                    break;
                case StatsDetailLevels.Operator:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = $"{selectPart} where DetailLevel='O' and OperatorId=@operatorId",
                        Param = new { @operatorId = entityId }
                    };
                    result.OperatorId = entityId;
                    break;
                default:
                    throw new ArgumentException("DetailLevel must be provided and set to either Campaing, Advertiser or Operator values", nameof(detailLevel));
            }
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                await conn.OpenAsync();
                var reader = await conn.ExecuteReaderAsync(conditionWithParams.Sql, conditionWithParams.Param);
                if(reader.Read())
                {
                    for (int idx = 0; idx < 10; idx++)
                        result.Values.Add(reader.GetInt64(idx));
                }
                return result;
            }
        }

        public async Task<ReachDao> GetReachStatsAsync(StatsDetailLevels detailLevel, int entityId)
        {
            const string tableName = "RollupsReach";
            ReachDao reachDao = new ReachDao { DetailLevel = detailLevel.ToSqlValue() };
            SqlExecutionContext conditionWithParams = null;
            switch (detailLevel)
            {
                case StatsDetailLevels.Campaign:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = $"select Reach from RollupsCampaign where DetailLevel='C' and CampaignId=@campaignId",
                        Param = new { @campaignId = entityId }
                    };
                    reachDao.CampaignId = entityId;
                    break;
                case StatsDetailLevels.Advertiser:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = $"select Reach from {tableName} where DetailLevel='A' and AdvertiserId=@advertiserId",
                        Param = new { @advertiserId = entityId }
                    };
                    reachDao.AdvertiserId = entityId;
                    break;
                case StatsDetailLevels.Operator:
                    conditionWithParams = new SqlExecutionContext
                    {
                        Sql = $"select Reach from {tableName} where DetailLevel='O' and OperatorId=@operatorId",
                        Param = new { @operatorId = entityId }
                    };
                    reachDao.OperatorId = entityId;
                    break;
                default:
                    throw new ArgumentException("DetailLevel must be provided and set to either Campaing, Advertiser or Operator values", nameof(detailLevel));
            }
            using (SqlConnection conn = new SqlConnection(_connStr))
            {
                await conn.OpenAsync();
                var result = await conn.QuerySingleOrDefaultAsync<long>(
                    conditionWithParams.Sql, conditionWithParams.Param);
                reachDao.ReachValue = result;
                return reachDao;
            }
        }

        public async Task<List<CampaignUserMatches>> GetCampaignUserMatchCountAsync(Int32[] CampaignProfileIds)
        {
            // TODO: Must be queried against the REST endpoint on a particular Provision plant.
            //using (SqlConnection conn = new SqlConnection(Configuration.StatsConnectionString))
            //{
    
            //    await conn.OpenAsync();

            //    var results = await conn.QueryAsync<CampaignUserMatches>("GetCampaignUserMatchCount",
            //        new { CampaignProfileIds = String.Join(",", CampaignProfileIds) },
            //        commandType: CommandType.StoredProcedure);

            //    return results.ToList();
            //}
            return new List<CampaignUserMatches>(0);
        }

        private class SqlExecutionContext
        {
            public string Sql { get; set; }
            public object Param { get; set; }
        }
    }
}