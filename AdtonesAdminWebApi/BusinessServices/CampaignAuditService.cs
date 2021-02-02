using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace AdtonesAdminWebApi.BusinessServices
{
    public class CampaignAuditService : ICampaignAuditService
    {
        IHttpContextAccessor _httpAccessor;
        private readonly ICampaignAuditDAL _auditDAL;
        private readonly IMemoryCache _cache;
        private readonly IConfiguration _configuration;
        private readonly IConnectionStringService _connService;
        // private readonly CurrencyConversion _currencyConversion;
        private readonly ILoggingService _logServ;
        const string PageName = "CampaignAuditService";

        ReturnResult result = new ReturnResult();


        public CampaignAuditService(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                ICampaignAuditDAL auditDAL, ICampaignDAL campDAL, IMemoryCache cache, LoggingService logServ)

        {
            _configuration = configuration;
            _connService = connService;
            _httpAccessor = httpAccessor;
            _auditDAL = auditDAL;
            _cache = cache;
            //_currencyConversion = CurrencyConversion.CreateForCurrentUserAsync(this).Result;
            _logServ = logServ;
        }

        public async Task<ReturnResult> GetCampaignDashboardSummariesOperatorByCampaign(int campaignId)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"OPERATOR_DASHBOARD_CAMPAIGN_STATS_{campaignId}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.CampaignDashboardSummariesOperators(campaignId);
                });

                if (summaries != null)
                {
                    result.body = GetDashboardSummariesToCovert(summaries);
                }
                else
                {
                    result.result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCampaignDashboardSummariesOperatorByCampaign";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// Called by operator for individual campaign playlist
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<ReturnResult> GetPlayDetailsForOperatorByCampaign(PagingSearchClass paging)
        {
            try
            {
                var res = await _auditDAL.GetPlayDetailsByCampaign(paging);
                result.recordcount = res.Count();
                result.body = res.Skip(paging.page * paging.pageSize).Take(paging.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetPlayDetailsForOperatorByCampaign";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// Actually called by the Operator
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public async Task<ReturnResult> GetDashboardSummariesByOperator(int operatorId)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"OPERATOR_DASHBOARD_STATS_{operatorId}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.DashboardSummariesOperators(operatorId);
                });

                if (summaries != null)
                {
                    result.body = GetDashboardSummariesToCovert(summaries);
                }
                else
                {
                    result.result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetDashboardSummariesByOperator";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }

        public async Task<ReturnResult> GetDashboardSummaryForSalesManager()
        {
            try
            {
                var salesId = _httpAccessor.GetUserIdFromJWT();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"SALES_DASHBOARD_STATS_{salesId}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.DashboardSummariesSalesManager(salesId);
                });

                // var summaries = await _auditDAL.GetDashboardSummariesForSalesManager();

                if (summaries != null)
                {
                    result.body = GetDashboardSummariesToCovert(summaries);
                }
                else
                {
                    result.result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetDashboardSummaryForSalesManager";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> GetDashboardSummaryForSalesExec(int userId)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"SALES_EXEC_DASHBOARD_STATS_{userId}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.DashboardSummariesSalesExec(userId);
                });

                if (summaries != null)
                {
                    result.body = GetDashboardSummariesToCovert(summaries);
                }
                else
                {
                    result.result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetDashboardSummaryForSalesExec";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


        private CampaignDashboardChartResult GetDashboardSummariesToCovert(CampaignDashboardChartPREResult summaries)
        {
            try
            {
                if (summaries != null)
                {
                    var _CampaignDashboardChartResult = new CampaignDashboardChartResult
                    {
                        CampaignName = summaries.CampaignName,
                        AdvertName = summaries.AdvertName,
                        TotalPlayed = (int)summaries.TotalPlays,
                        FreePlays = (int)summaries.FreePlays,
                        TotalSpend = (double)summaries.Spend,
                        AverageBid = (double)summaries.AvgBid,
                        AveragePlayTime = (double)summaries.AvgPlayLength,
                        TotalBudget = summaries.Budget,
                        MaxPlayLength = summaries.MaxPlayLength,
                        TotalReach = (int)summaries.TotalReach,
                        Reach = (int)summaries.Reach,
                        CurrencyCode = summaries.CurrencyCode
                        //MaxPlayLengthPercantage = totalPlays == 0 ? 0 : Math.Round((double)summaries.TotalValuablePlays / totalPlays, 2),
                    };

                    return _CampaignDashboardChartResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetDashboardSummariesToCovert";
                _logServ.LogError();
                
                return null;
            }
        }


        public async Task<ReturnResult> GetPromoCampaignDashboardSummary(int campaignId)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"PROMO_DASHBOARD_CAMPAIGN_STATS_{campaignId}";
                result.body = await _cache.GetOrCreateAsync<CampaignDashboardChartResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.CampaignPromoDashboardSummaries(campaignId);
                });
                // result.body = await _auditDAL.GetPromoCampaignDashboardSummaries(campaignId);
                
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetPromoCampaignDashboardSummary";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> GetPromoPlayDetails(PagingSearchClass paging)
        {
            try
            {
                var res = await _auditDAL.GetPromoPlayDetails(paging);
                result.recordcount = res.Count();
                result.body = res.Skip(paging.page * paging.pageSize).Take(paging.pageSize);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetPromoPlayDetails";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


    }
}
