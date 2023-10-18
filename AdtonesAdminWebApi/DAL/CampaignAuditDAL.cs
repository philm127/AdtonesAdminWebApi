using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{

    public class CampaignAuditDAL : BaseDAL, ICampaignAuditDAL
    {
        private readonly ILoggingService _logServ;
        const string PageName = "CampaignAuditDAL";

        public CampaignAuditDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                                IHttpContextAccessor httpAccessor, ILoggingService logServ,
                                IMemoryCache cache) : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }

        public static string getDashboardData = @"SELECT SUM(CAST(ISNULL(ca.Budget,0) AS bigint)) AS Budget, 
                                                    sum(ISNULL(ca.Spend,0)) AS Spend,
                                                    sum(ISNULL(ca.FundsAvailable,0)) AS FundsAvailable,
                                                    AVG(ca.AvgBid) AS AvgBid,
                                                    CAST(SUM(ISNULL(ca.TotalSMS,0)) AS bigint) AS TotalSMS,
                                                    SUM(ISNULL(ca.TotalSMSCost,0)) AS TotalSMSCost,
                                                    CAST(ISNULL(SUM(ca.TotalEmail),0) AS bigint) AS TotalEmail,
                                                    SUM(ISNULL(ca.TotalEmailCost,0)) AS TotalEmailCost,
                                                    Cast(SUM(ISNULL(ca.TotalPlays,0)) AS bigint) AS TotalPlays,
                                                    Cast(SUM(ISNULL(ca.MoreSixSecPlays,0)) AS bigint) AS MoreSixSecPlays,
                                                    (SUM(ISNULL(ca.TotalPlays,0)) - SUM(ISNULL(ca.MoreSixSecPlays,0))) AS FreePlays,
                                                    AVG(ISNULL(ca.AvgPlayLength,0))	AS AvgPlayLength,
                                                    MAX(ISNULL(ca.MaxPlayLength,0)) AS MaxPlayLength,
                                                    MAX(CAST(ISNULL(ca.MaxBid, 0) AS numeric(16,2))) AS MaxBid,
                                                    MAX(ISNULL(ca.Reach,0)) AS Reach,
                                                    'KES' AS CurrencyCode,
                                                    0 AS TotalReach
	                                                FROM [Arthar].[dbo].[CampaignProfile] AS cp 
                                                    LEFT JOIN [ManR].[dbo].[RollupsCampaign] AS ca ON cp.CampaignProfileId=ca.CampaignId
                                                    LEFT JOIN [ManR].[dbo].[CampaignReach] AS cr ON cr.CampaignId=cp.CampaignProfileId";


        ///// <summary>
        ///// Get Dashboard by individual campaign
        ///// Uses memory caching. If in cache returns that values else calls CampaignDashboardSummariesOperators
        ///// below. The result is then stored in cache.
        ///// </summary>
        ///// <param name="campaignId">supplied campaign id to group stats by</param>
        ///// <returns></returns>



        /// <summary>
        /// Called by operator individual campaign
        /// called by GetCampaignDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">camapign Id</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> CampaignDashboardSummary(int campaignId)
        {

            var str = getDashboardData;
            int pos = str.IndexOf("SELECT");
            if (pos != -1)
            {
                str = str.Remove(pos, "SELECT".Length).Insert(pos, "");
            }
            string modifiedStr = str.TrimStart();

            var sb = new StringBuilder();
            sb.Append(" SELECT u.Userid,cp.CampaignProfileId,ca.AdvertId,cp.CampaignName,a.AdvertName,");
            sb.Append(" CONCAT(u.FirstName,' ',u.LastName, ' (', u.Email, ')') AS CampaignHolder,");
            sb.Append(modifiedStr);
            sb.Append(" INNER JOIN [Arthar].[dbo].[Users] AS u ON cp.UserId=u.UserId ");
            sb.Append(" INNER JOIN [Arthar].[dbo].[Advert] AS a ON a.AdvertId=ca.AdvertId ");
            sb.Append(" WHERE cp.CampaignProfileId=@campId ");
            sb.Append(" GROUP BY u.UserId,cp.CampaignProfileId,ca.AdvertId,cp.CampaignName,a.AdvertName,u.FirstName,u.LastName, u.Email ");
            var strTst = sb.ToString();
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(sb.ToString(), new { campId = campaignId }, commandTimeout: 60));
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CampaignDashboardSummariesOperators";
                _logServ.LogLevel = sb.ToString();
                await _logServ.LogError();

                throw;
            }
        }


        /// <summary>
        /// Actually called by the operator
        /// called by GetDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">operatorId</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> DashboardSummariesOperators(int operatorId)
        {
            var sb = new StringBuilder();
            sb.Append(getDashboardData);
            sb.Append(" WHERE ca.OperatorId=@opId; ");
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(sb.ToString(), new { opId = operatorId }));
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DashboardSummariesOperators";
                _logServ.LogLevel = sb.ToString();
                await _logServ.LogError();

                throw;
            }
        }



        ///// <summary>
        ///// Get Dashboard campaigns grouped by A Sales Manager in theory but currently country
        ///// Uses memory caching. If in cache returns that values else calls DashboardSummariesSalesManager
        ///// below. The result is then stored in cache.
        ///// </summary>
        ///// <param name="countryId"></param>
        ///// <returns></returns>
        //public async Task<CampaignDashboardChartPREResult> GetDashboardSummariesForSalesManager()
        //{
        //    var salesId = _httpAccessor.GetUserIdFromJWT();
        //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        //    string key = $"SALES_DASHBOARD_STATS_{salesId}";
        //    return await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
        //    {
        //        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
        //        return DashboardSummariesSalesManager(salesId);
        //    });
        //    // return cacheEntry ?? new List<CampaignDashboardChartPREResult>();
        //}


        /// <summary>
        /// called by GetDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">operatorId</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> DashboardSummariesSalesManager(int salesmanId, int campaignId)
        {
            var countryId = await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<int>("SELECT CountryId FROM Contacts WHERE UserId=@Id ", new { Id = salesmanId }));

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(getDashboardData);
            sb.Append(" WHERE cp.CountryId=@Id ");
            if (campaignId > 0)
            {
                sb.Append(" AND cp.CampaignProfileId=@CampaignId ");
                builder.AddParameters(new { CampaignId = campaignId });
            }

            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { Id = countryId });
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


        /// <summary>
        /// called by GetDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">operatorId</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> DashboardSummariesSalesExec(int salesexecId)
        {
            var sb = new StringBuilder();
            sb.Append(getDashboardData);
            sb.Append(" INNER JOIN Advertisers_SalesTeam AS ast ON ast.AdvertiserId=cp.UserId ");
            sb.Append(" WHERE ast.SalesExecId=@Sid ");
            sb.Append(" AND ast.IsActive=1; ");
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(
                                                                    sb.ToString(), new { Sid = salesexecId }));
            }
            catch
            {
                throw;
            }
        }



        public async Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesAdvertisers(int userId, int campaignId = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(getDashboardData);
            sb.Append(" WHERE cp.UserId=@userId ");

            if (campaignId > 0)
            {
                sb.Append("  and cp.CampaignProfileId = @campId; ");
                builder.AddParameters(new { campId = campaignId });
            }

            var select = builder.AddTemplate(sb.ToString());

            builder.AddParameters(new { userId = userId });

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


        /// <summary>
        /// Maybe redundant
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<IEnumerable<CampaignAuditOperatorTable>> GetPlayDetailsByCampaignCount(PagingSearchClass param)
        {
            string getPlayDetailsByCampaign = @"SELECT CAST(ISNULL(ca.TotalCost,0) AS NUMERIC(36,2)) AS TotalCost,CAST(ISNULL(ca.BidValue,0) AS NUMERIC(36,2)) AS PlayCost,
													CAST(ISNULL(ca.EmailCost,0) AS NUMERIC(36,2)) AS EmailCost,CAST(ISNULL(ca.SMSCost,0) AS NUMERIC(36,2)) AS SMSCost,
													ca.StartTime,ca.EndTime,CAST((ca.PlayLengthTicks / 1000) AS NUMERIC(36,2)) AS PlayLength,ca.Email AS EmailMsg,ca.SMS,
													up.UserId,cp.CurrencyCode,ad.AdvertName,CampaignAuditId, cp.CampaignProfileId
													FROM CampaignProfile AS cp INNER JOIN CampaignAudit AS ca ON ca.CampaignProfileId=cp.CampaignProfileId
													LEFT JOIN UserProfile AS up ON ca.UserProfileId=up.UserProfileId
													LEFT JOIN 
														(SELECT AdvertId,CampaignProfileId FROM CampaignAdverts WHERE AdvertId in
			                                                (SELECT MAX(AdvertId) FROM CampaignAdverts GROUP BY CampaignProfileId)
		                                                ) AS cad 
													ON cad.CampaignProfileId=ca.CampaignProfileId
													LEFT JOIN Advert AS ad ON ad.AdvertId=cad.AdvertId
													WHERE cp.Status != 5
													AND ca.Status='Played'
													AND cp.CampaignProfileId=@Id ";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(getPlayDetailsByCampaign);
            builder.AddParameters(new { Id = param.elementId });

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignAuditOperatorTable>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }

        }


        public async Task<IEnumerable<CampaignAuditOperatorTable>> GetPlayDetailsByCampaign(PagingSearchClass param)
        {
            string getPlayDetailsByCampaign = @"SELECT CAST(ISNULL(ca.TotalCost,0) AS NUMERIC(36,2)) AS TotalCost,CAST(ISNULL(ca.BidValue,0) AS NUMERIC(36,2)) AS PlayCost,
													CAST(ISNULL(ca.EmailCost,0) AS NUMERIC(36,2)) AS EmailCost,CAST(ISNULL(ca.SMSCost,0) AS NUMERIC(36,2)) AS SMSCost,
													ca.StartTime,ca.EndTime,CAST((ca.PlayLengthTicks / 1000) AS NUMERIC(36,2)) AS PlayLength,ca.Email AS EmailMsg,ca.SMS,
													up.UserId,cp.CurrencyCode,ad.AdvertName,CampaignAuditId, cp.CampaignProfileId
													FROM CampaignProfile AS cp INNER JOIN CampaignAudit AS ca ON ca.CampaignProfileId=cp.CampaignProfileId
													LEFT JOIN UserProfile AS up ON ca.UserProfileId=up.UserProfileId
													LEFT JOIN 
														(SELECT AdvertId,CampaignProfileId FROM CampaignAdverts WHERE AdvertId in
			                                                (SELECT MAX(AdvertId) FROM CampaignAdverts GROUP BY CampaignProfileId)
		                                                ) AS cad 
													ON cad.CampaignProfileId=ca.CampaignProfileId
													LEFT JOIN Advert AS ad ON ad.AdvertId=cad.AdvertId
													WHERE cp.Status != 5
													AND ca.Status='Played'
													AND cp.CampaignProfileId=@Id ";

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            PageSearchModel searchList = null;

            sb.Append(getPlayDetailsByCampaign);

            try
            {
                if (param.search != null && param.search.Length > 3)
                {
                    searchList = JsonConvert.DeserializeObject<PageSearchModel>(param.search);

                    if (searchList.NumberFrom != null && (searchList.NumberTo == null || searchList.NumberTo >= searchList.NumberFrom))
                    {
                        sb.Append(" AND BidValue >= @playcostfrom ");
                        builder.AddParameters(new { playcostfrom = searchList.NumberFrom });
                    }

                    if (searchList.NumberTo != null && (searchList.NumberFrom == null || searchList.NumberFrom <= searchList.NumberTo))
                    {
                        sb.Append(" AND BidValue <= @playcostto");
                        builder.AddParameters(new { playcostto = searchList.NumberTo });
                    }

                    if (searchList.NumberFrom2 != null && (searchList.NumberTo2 == null || searchList.NumberTo2 >= searchList.NumberFrom2))
                    {
                        sb.Append(" AND PlayLengthTicks >= @playfrom * 1000 ");
                        builder.AddParameters(new { playfrom = searchList.NumberFrom2 });
                    }

                    if (searchList.NumberTo2 != null && (searchList.NumberFrom2 == null || searchList.NumberFrom2 <= searchList.NumberTo2))
                    {
                        sb.Append(" AND PlayLengthTicks <= @playto * 1000");
                        builder.AddParameters(new { playto = searchList.NumberTo2 });
                    }

                    if (searchList.NumberFrom3 != null && (searchList.NumberTo3 == null || searchList.NumberTo3 >= searchList.NumberFrom3))
                    {
                        sb.Append(" AND TotalCost >= @totalcostfrom ");
                        builder.AddParameters(new { totalcostfrom = searchList.NumberFrom3 });
                    }

                    if (searchList.NumberTo3 != null && (searchList.NumberFrom3 == null || searchList.NumberFrom3 <= searchList.NumberTo3))
                    {
                        sb.Append(" AND TotalCost <= @totalcostto");
                        builder.AddParameters(new { totalcostto = searchList.NumberTo3 });
                    }


                    if (searchList.responseFrom != null && (searchList.responseTo == null || searchList.responseTo >= searchList.responseFrom))
                    {
                        sb.Append(" AND StartTime >= @startfrom ");
                        builder.AddParameters(new { startfrom = searchList.responseFrom });
                    }

                    if (searchList.responseTo != null && (searchList.responseFrom == null || searchList.responseFrom <= searchList.responseTo))
                    {
                        sb.Append(" AND StartTime <= @startto");
                        builder.AddParameters(new { startto = searchList.responseTo });
                    }
                }



                sb.Append(" ORDER BY ");

                switch (param.sort)
                {
                    case "playCost":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" BidValue  ASC ");
                        else
                            sb.Append(" BidValue  DESC ");
                        break;
                    case "playLength":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" PlayLengthTicks  ASC ");
                        else
                            sb.Append(" PlayLengthTicks  DESC ");
                        break;
                    case "startTime":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" StartTime  ASC ");
                        else
                            sb.Append(" StartTime  DESC ");
                        break;
                    case "totalCost":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" TotalCost  ASC ");
                        else
                            sb.Append(" TotalCost  DESC ");
                        break;
                    case "emailCost":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" EmailCost  ASC ");
                        else
                            sb.Append(" EmailCost  DESC ");
                        break;
                    case "sMSlCost":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" SMSCost  ASC ");
                        else
                            sb.Append(" SMSCost  DESC ");
                        break;
                    default:
                        sb.Append(" ca.CampaignAuditId  DESC ");
                        break;
                }

                var select = builder.AddTemplate(sb.ToString());

                builder.AddParameters(new { Id = param.elementId });
                builder.AddParameters(new { PageIndex = param.page });
                builder.AddParameters(new { PageSize = param.pageSize });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<CampaignAuditOperatorTable>(select.RawSql, select.Parameters, commandTimeout: 60));
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignDashboardChartResult> CampaignPromoDashboardSummaries(int campaignId)
        {
            string getPromoCampaignDashboard = @"SELECT (CAST((ticks.sumplay / audCT.totalPlayCount) as DECIMAL(9,2)) / 1000) AS AveragePlayTime,
													pc.CampaignName,audCT.totalPlayCount AS TotalPlayed,dist.totalReach AS Reach,op.OperatorName,
													pa.AdvertName,CONCAT(@siteAddress,pa.AdvertLocation) AS AdvertLocation
													FROM PromotionalCampaigns AS pc LEFT JOIN PromotionalCampaignAudits AS pca
													ON pc.ID=pca.PromotionalCampaignId
													INNER JOIN PromotionalAdverts AS pa
													ON pa.CampaignID=pca.PromotionalCampaignId
													LEFT JOIN 
														(SELECT COUNT(PromotionalCampaignAuditId)  AS totalPlayCount,pca.PromotionalCampaignId 
															FROM PromotionalCampaignAudits AS pca
															GROUP BY pca.PromotionalCampaignId) AS audCT
													ON audCT.PromotionalCampaignId=pc.ID
													LEFT JOIN
														(SELECT COUNT(DISTINCT(MSISDN)) AS totalReach, PromotionalCampaignId 
															FROM PromotionalCampaignAudits
															WHERE (DTMFKey != '0' AND DTMFKey IS NOT NULL) GROUP BY PromotionalCampaignId) AS dist
													ON dist.PromotionalCampaignId=pc.ID
													LEFT JOIN 
														(SELECT SUM(PlayLengthTicks) AS sumplay, PromotionalCampaignId FROM PromotionalCampaignAudits
															GROUP BY PromotionalCampaignId) AS ticks
													ON ticks.PromotionalCampaignId=pc.ID
													INNER JOIN Operators AS op ON op.OperatorId=pc.OperatorID
													WHERE pc.ID=@Id
													GROUP BY ticks.sumplay,pc.CampaignName,audCT.totalPlayCount,dist.totalReach,op.OperatorName,
													pa.AdvertName,pa.AdvertLocation";

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(getPromoCampaignDashboard);
            builder.AddParameters(new { Id = campaignId });
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartResult>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<PromoCampaignPlaylist>> GetPromoPlayDetails(PagingSearchClass param)
        {
            string getPromoPlayDetails = @"SELECT ROUND(pca.PlayLengthTicks/1000,0) AS PlayLength,pa.AdvertName,
												pca.PromotionalCampaignAuditId AS AuditId,pca.MSISDN,pca.StartTime,ISNULL(pca.DTMFKey,'-') AS DTMFKey
												FROM PromotionalCampaignAudits AS pca
												INNER JOIN PromotionalAdverts AS pa
												ON pa.CampaignID=pca.PromotionalCampaignId
												WHERE pca.PromotionalCampaignId=@Id";

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            PageSearchModel searchList = null;
            sb.Append(getPromoPlayDetails);
            try
            {
                if (param.search != null && param.search.Length > 3)
                {
                    searchList = JsonConvert.DeserializeObject<PageSearchModel>(param.search);

                    if (searchList.NumberFrom != null && (searchList.NumberTo == null || searchList.NumberTo >= searchList.NumberFrom))
                    {
                        sb.Append(" AND PlayLengthTicks >= @playfrom * 1000 ");
                        builder.AddParameters(new { playfrom = searchList.NumberFrom });
                    }

                    if (searchList.NumberTo != null && (searchList.NumberFrom == null || searchList.NumberFrom <= searchList.NumberTo))
                    {
                        sb.Append(" AND PlayLengthTicks <= @playto * 1000");
                        builder.AddParameters(new { playto = searchList.NumberTo });
                    }

                    if (searchList.responseFrom != null && (searchList.responseTo == null || searchList.responseTo >= searchList.responseFrom))
                    {
                        sb.Append(" AND StartTime >= @startfrom ");
                        builder.AddParameters(new { startfrom = searchList.responseFrom });
                    }

                    if (searchList.responseTo != null && (searchList.responseFrom == null || searchList.responseFrom <= searchList.responseTo))
                    {
                        sb.Append(" AND StartTime <= @startto");
                        builder.AddParameters(new { startto = searchList.responseTo });
                    }

                    if (searchList.Name != null)
                    {
                        string likeStr = searchList.Name + "%";
                        sb.Append(" AND MSISDN LIKE @likeStr ");
                        builder.AddParameters(new { likeStr = likeStr });
                    }

                    if (searchList.fullName != null)
                    {
                        string likeDt = searchList.fullName + "%";
                        sb.Append(" AND DTMFKey LIKE @likeDt ");
                        builder.AddParameters(new { likeDt = likeDt });
                    }


                }

                sb.Append(" ORDER BY ");

                switch (param.sort)
                {
                    case "msisdn":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" MSISDN  ASC ");
                        else
                            sb.Append(" MSISDN  DESC ");
                        break;
                    case "playLength":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" PlayLengthTicks  ASC ");
                        else
                            sb.Append(" PlayLengthTicks  DESC ");
                        break;
                    case "startTime":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" StartTime  ASC ");
                        else
                            sb.Append(" StartTime  DESC ");
                        break;
                    case "dtmfKey":
                        if (param.direction.ToLower() == "asc")
                            sb.Append(" DTMFKey  ASC ");
                        else
                            sb.Append(" DTMFKey  DESC ");
                        break;
                    default:
                        sb.Append(" pca.PromotionalCampaignAuditId  DESC ");
                        break;
                }

                var select = builder.AddTemplate(sb.ToString());

                builder.AddParameters(new { Id = param.elementId });
                builder.AddParameters(new { PageIndex = param.page });
                builder.AddParameters(new { PageSize = param.pageSize });

                return await _executers.ExecuteCommand(_connStr,
                         conn => conn.Query<PromoCampaignPlaylist>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<DailyReportResponse>> GetDailyReportDetails(DailyReportCommand model)
        {
            var operatorId = 1;// _httpAccessor.GetUserIdFromJWT();
            var connStr = await _connService.GetConnectionStringByOperator(operatorId);
            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    connection.Open();

                    var parameters = new DynamicParameters();
                    parameters.Add("@DateFrom", model.DateFrom);
                    parameters.Add("@DateTo", model.DateTo);

                    return await connection.QueryAsync<DailyReportResponse>(
                        "sp_basic_stats_by_timeframe",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 180
                    );
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetDailyReportDetails";
                _logServ.LogLevel = JsonConvert.SerializeObject(model);
                await _logServ.LogError();

                throw;
            }
        }
    }
}
