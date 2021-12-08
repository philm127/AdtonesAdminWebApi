using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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


        ///// <summary>
        ///// Get Dashboard by individual campaign
        ///// Uses memory caching. If in cache returns that values else calls CampaignDashboardSummariesOperators
        ///// below. The result is then stored in cache.
        ///// </summary>
        ///// <param name="campaignId">supplied campaign id to group stats by</param>
        ///// <returns></returns>
        //public async Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesForOperator(int campaignId)
        //{
        //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        //    string key = $"OPERATOR_DASHBOARD_CAMPAIGN_STATS_{campaignId}";
        //    return await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
        //    {
        //        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
        //        return CampaignDashboardSummariesOperators(campaignId);
        //    });
        //    // return cacheEntry ?? new List<CampaignDashboardChartPREResult>();
        //}


        /// <summary>
        /// Called by operator individual campaign
        /// called by GetCampaignDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">camapign Id</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> CampaignDashboardSummariesOperators(int campaignId)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(CampaignAuditQuery.GetCampaignDashboardSummaries);
            sb.Append(" cp.CampaignProfileId=@campId ");
            sb.Append(" GROUP BY u.UserId,cp.CampaignProfileId,a.AdvertId,cp.CampaignName,a.AdvertName,u.FirstName,u.LastName, u.Email, ");
            // Group By continued
            sb.Append(" cp.TotalBudget,g.TotalPlayedCost,g.TotalAvgBid,g.TotalSMS,g.TotalSMSCost,g.TotalEmail,g.TotalEmailCost,p.TotalPlayTracks, ");
            // Group By continued
            sb.Append(" p.AvgPlayLen,p.MaxPlayLen,r.UniqueListenrs,p.MaxBid,cp.CurrencyCode,ctu.TotalReach ");
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { campId = campaignId });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters, commandTimeout:60));

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CampaignDashboardSummariesOperators";
                _logServ.LogLevel = select.RawSql;
                await _logServ.LogError();
                
                throw;
            }
        }


        ///// <summary>
        ///// Get Dashboard campaigns grouped by Operator
        ///// Uses memory caching. If in cache returns that values else calls DashboardSummariesOperators
        ///// below. The result is then stored in cache.
        ///// </summary>
        ///// <param name="operatorId"></param>
        ///// <returns></returns>
        //public async Task<CampaignDashboardChartPREResult> GetDashboardSummariesForOperator(int operatorId)
        //{
        //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        //    string key = $"OPERATOR_DASHBOARD_STATS_{operatorId}";
        //    return await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
        //    {
        //        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
        //        return DashboardSummariesOperators(operatorId);
        //    });
        //    // return cacheEntry ?? new List<CampaignDashboardChartPREResult>();
        //}


        /// <summary>
        /// Actually called by the operator
        /// called by GetDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">operatorId</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> DashboardSummariesOperators(int operatorId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignAuditQuery.GetCampaignDashboardSummariesByOperator);
            builder.AddParameters(new { opId = operatorId });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters));

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DashboardSummariesOperators";
                _logServ.LogLevel = select.RawSql;
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
        public async Task<CampaignDashboardChartPREResult> DashboardSummariesSalesManager(int salesmanId)
        {
            
            var countryId = await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<int>(CampaignAuditQuery.GetCountryIdByUserId, new { Id = salesmanId }));

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignAuditQuery.GetCampaignDashboardSummariesByCountry);
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


        ///// <summary>
        ///// Get Dashboard campaigns grouped by A Sales Executive userid
        ///// Uses memory caching. If in cache returns that values else calls DashboardSummariesSalesExec
        ///// below. The result is then stored in cache.
        ///// </summary>
        ///// <param name="countryId"></param>
        ///// <returns></returns>
        //public async Task<CampaignDashboardChartPREResult> GetDashboardSummariesForSalesExec(int sid)
        //{
        //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //            .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        //    string key = $"SALES_EXEC_DASHBOARD_STATS_{sid}";
        //    return await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
        //    {
        //        cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
        //        return DashboardSummariesSalesExec(sid);
        //    });
        //    // return cacheEntry ?? new List<CampaignDashboardChartPREResult>();
        //}


        /// <summary>
        /// called by GetDashboardSummariesForOperator if it doesn't have the result set in cache
        /// </summary>
        /// <param name="id">operatorId</param>
        /// <returns></returns>
        public async Task<CampaignDashboardChartPREResult> DashboardSummariesSalesExec(int salesexecId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(
                                                        CampaignAuditQuery.GetCampaignDashboardSummariesByExec, new { Sid = salesexecId }));
            }
            catch
            {
                throw;
            }
        }



        public async Task<CampaignDashboardChartPREResult> GetCampaignDashboardSummariesAdvertisers(int userId, int campaignId=0)
        {
            var selectQuery = @"SELECT ISNULL(CAST(cp.TotalBudget AS bigint),0) AS Budget, ISNULL(g.TotalPlayedCost,0) AS Spend,
	                            ISNULL(cp.TotalBudget,0) - ISNULL(g.TotalPlayedCost,0) AS FundsAvailable,
	                            ISNULL(g.TotalAvgBid,0) AS AvgBid,CAST(ISNULL(g.TotalSMS,0) AS bigint) AS TotalSMS,
	                            ISNULL(g.TotalSMSCost,0) AS TotalSMSCost,CAST(ISNULL(g.TotalEmail,0) AS bigint) AS TotalEmail,
	                            ISNULL(g.TotalEmailCost,0) AS TotalEmailCost,Cast(ISNULL(p.TotalPlayTracks,0) AS bigint) AS TotalPlays,
	                            Cast(ISNULL(g.TotalPlayTracks,0) AS bigint) AS MoreSixSecPlays,
	                            ISNULL(p.TotalPlayTracks,0) - ISNULL(g.TotalPlayTracks,0) AS FreePlays,
	                            ISNULL(p.AvgPlayLen,0) AS AvgPlayLength,
	                            ISNULL(p.MaxPlayLen,0) AS MaxPlayLength,
	                            ISNULL(r.UniqueListenrs,0) AS Reach,
	                            CAST(ISNULL(p.MaxBid, 0) AS numeric(16,2)) AS MaxBid,
	                            cp.CurrencyCode AS CurrencyCode,ctu.TotalReach
	                            FROM 
		                            (SELECT SUM(ISNULL(TotalBudget,0)) AS TotalBudget,CountryId,CurrencyCode,UserId,CampaignProfileId FROM CampaignProfile
                                        WHERE UserId=@userId
		                            GROUP BY UserId,CampaignProfileId,CountryId,CurrencyCode) AS cp
	                            INNER JOIN
		                            ( SELECT cpi.CountryId,CONVERT(numeric(16,0), SUM(ca.TotalCost)) AS TotalPlayedCost,
			                            CONVERT(numeric(16,2), AVG(ca.BidValue)) AS TotalAvgBid,
			                            SUM(CASE WHEN ca.SMS IS NOT NULL THEN 1 ELSE 0 END) AS TotalSMS,
			                            CONVERT(numeric(16,0), SUM(ISNULL(ca.SMSCost,0))) AS TotalSMSCost,
			                            SUM(CASE WHEN ca.Email IS NOT NULL THEN 1 ELSE 0 END) AS TotalEmail,
			                            CONVERT(NUMERIC(16,0), SUM(ISNULL(ca.EmailCost,0))) AS TotalEmailCost,
			                            count(*) AS TotalPlayTracks,UserId,cpi.CampaignProfileId 
			                            FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
                                        WHERE cpi.UserId=@userId
			                            AND ca.PlayLengthTicks >= 6000 AND ca.Proceed = 1
			                            GROUP BY cpi.CountryId,UserId,cpi.CampaignProfileId
		                            ) AS g ON g.CountryId = cp.CountryId AND cp.UserId=g.UserId AND cp.CampaignProfileId=g.CampaignProfileId

	                            LEFT JOIN 
		                            ( SELECT cpi.CountryId, COUNT(DISTINCT ca.UserProfileId) AS UniqueListenrs
			                            FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
                                            WHERE cpi.UserId=@userId
			                            AND ca.Proceed = 1
			                            GROUP BY cpi.CountryId
		                            ) AS r ON r.CountryId = cp.CountryId
	                            LEFT JOIN 
		                            (  SELECT cpi.CountryId, COUNT(*) AS TotalPlayTracks,AVG(ca.PlayLengthTicks) AS AvgPlayLen,
			                            MAX(ca.PlayLengthTicks) AS MaxPlayLen,MAX(ca.BidValue) AS MaxBid
			                            FROM CampaignAudit AS ca INNER JOIN CampaignProfile AS cpi ON cpi.CampaignProfileId=ca.CampaignProfileId
                                        WHERE cpi.UserId=@userId
			                            AND ca.Proceed = 1
			                            GROUP BY cpi.CountryId
		                            ) AS p ON p.CountryId = cp.CountryId

	                            LEFT JOIN
		                            (SELECT COUNT(UserId) AS TotalReach,op.CountryId FROM Users AS u 
			                            INNER JOIN Operators AS op ON u.OperatorId=op.OperatorId 
			                            WHERE VerificationStatus=1 AND Activated=1 GROUP BY op.CountryId) AS ctu
	                            ON cp.CountryId=ctu.CountryId 
	                            INNER JOIN Users AS u ON u.UserId = cp.UserId
	                            WHERE u.UserId=@userId ";

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(selectQuery);
            if (campaignId > 0)
            {
                sb.Append("  and cp.CampaignProfileId = @campaignId; ");
                builder.AddParameters(new { campId = campaignId });
            }
            
            var select = builder.AddTemplate(sb.ToString());
            
            builder.AddParameters(new { userId = userId });

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters));
                //var result = x.AsList<CampaignDashboardChartPREResult>();
                //return result;
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

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignAuditQuery.GetPlayDetailsByCampaign);
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
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            PageSearchModel searchList = null;

            sb.Append(CampaignAuditQuery.GetPlayDetailsByCampaign);

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


        //public async Task<CampaignDashboardChartResult> GetPromoCampaignDashboardSummaries(int campaignId)
        //{
        //    try
        //    {
        //        var cacheEntryOptions = new MemoryCacheEntryOptions()
        //                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
        //        string key = $"PROMO_DASHBOARD_CAMPAIGN_STATS_{campaignId}";
        //        return await _cache.GetOrCreateAsync<CampaignDashboardChartResult>(key, cacheEntry =>
        //        {
        //            cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
        //            return CampaignPromoDashboardSummaries(campaignId);
        //        });
        //        // return cacheEntry ?? new List<CampaignDashboardChartPREResult>();
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        public async Task<CampaignDashboardChartResult> CampaignPromoDashboardSummaries(int campaignId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(CampaignAuditQuery.GetPromoCampaignDashboard);
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
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            PageSearchModel searchList = null;
            sb.Append(CampaignAuditQuery.GetPromoPlayDetails);
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
    }
}
