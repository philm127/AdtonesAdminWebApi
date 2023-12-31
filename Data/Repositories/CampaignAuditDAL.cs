﻿using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using AdtonesAdminWebApi.Services;
using Domain.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
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
                                 conn => conn.QueryFirstOrDefault<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters));

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



        public async Task<List<CampaignDashboardChartPREResult>> GetCampaignDashboardSummariesAdvertisers(int campid = 0, int userId = 0)
        {
            int? campId = null;
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(CampaignAuditQuery.GetCampaignDashboardSummaries);
            if (campid > 0)
                campId = campid;

            sb.Append(" u.UserId=@userId and(cp.CampaignProfileId is null or(cp.CampaignProfileId = @campaignId)); ");
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { campId = campId });
            builder.AddParameters(new { userId = userId });

            try
            {

                var x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignDashboardChartPREResult>(select.RawSql, select.Parameters));
                var result = x.AsList<CampaignDashboardChartPREResult>();
                return result;
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
                                    conn => conn.Query<CampaignAuditOperatorTable>(select.RawSql, select.Parameters));
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
