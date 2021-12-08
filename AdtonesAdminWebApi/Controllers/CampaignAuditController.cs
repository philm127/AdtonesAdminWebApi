using System;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.BusinessServices.ManagementReports;
using AdtonesAdminWebApi.DAL.Interfaces;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CampaignAuditController : ControllerBase
    {
        IHttpContextAccessor _httpAccessor;
        private readonly IManagementReportService _manService;
        private readonly ICampaignAuditDAL _auditDAL;
        private readonly ILoggingService _logServ;
        private readonly IMemoryCache _cache;
        ReturnResult result = new ReturnResult();
        const string PageName = "FinancialsController";
        //private readonly ICreateExelManagementReport _excelService;

        public CampaignAuditController(IHttpContextAccessor httpAccessor, IManagementReportService manService, //), ICreateExelManagementReport excelService)
                                        ICampaignAuditDAL auditDAL, IMemoryCache cache, ILoggingService logServ)
        {
            _manService = manService;
            _auditDAL = auditDAL;
            _cache = cache;
            _logServ = logServ;
            _httpAccessor = httpAccessor;
            //_excelService = excelService;
        }


        [HttpPut("v1/GetManagementReport")]
        public async Task<ReturnResult> GetManagementReport(ManagementReportsSearch search)
        {
            return await _manService.GetReportData(search);
        }


        //[HttpPut("v1/GenerateManReport")]
        //public async Task<IActionResult> GenerateManReport(ManagementReportsSearch search)
        //{
        //    string fileName = "Management_Report.xlsx";// + DateTime.Now.ToString("yyyy -MM-dd HH':'mm':'ss") + ".xlsx";
        //    byte[] filebyte = await _excelService.GenerateExcelManagementReport(search);
        //    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetDashboardSummaryByOperator/{id}")]
        public async Task<ReturnResult> GetDashboardSummaryByOperator(int id = 0)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"OPERATOR_DASHBOARD_STATS_{id}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.DashboardSummariesOperators(id);
                });

                if (summaries != null)
                    result.body = GetDashboardSummariesToCovert(summaries);
                else
                    result.result = 0;
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


        /// <summary>
        /// GetCampaignDashboardSummariesOperatorByCampaign
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetDashboardSummaryByCampaign/{id}")]
        public async Task<ReturnResult> GetDashboardSummaryByCampaign(int id = 0)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"OPERATOR_DASHBOARD_CAMPAIGN_STATS_{id}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.CampaignDashboardSummariesOperators(id);
                });

                if (summaries != null)
                    result.body = GetDashboardSummariesToCovert(summaries);
                else
                    result.result = 0;
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
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetPromoDashboardSummary/{id}")]
        public async Task<ReturnResult> GetPromoDashboardSummary(int id = 0)
        {
            var campaignId = id;
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpPut("v1/GetPlayDetailsForOperatorByCampaign")]
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
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpPut("v1/GetPromoPlayDetailsByCampaign")]
        public async Task<ReturnResult> GetPromoPlayDetailsByCampaign(PagingSearchClass paging)
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/GetCampDashboardSummarySalesManager")]
        public async Task<ReturnResult> GetCampDashboardSummarySalesManager()
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

                if (summaries != null)
                    result.body = GetDashboardSummariesToCovert(summaries);
                else
                    result.result = 0;
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

        /// <summary>
        /// id refers to campaignID
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/GetDashboardSummaryAdvertiser/{id}")]
        public async Task<ReturnResult> GetDashboardSummaryAdvertiser(int id=0)
        {
            var campaignId = id;
            try
            {
                string key = String.Empty;
                var advertiserId = _httpAccessor.GetUserIdFromJWT();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                if (campaignId == 0)
                    key = $"ADVERTISER_DASHBOARD_STATS_{advertiserId}";
                else
                    key = $"ADVERTISER_DASHBOARD_STATS_{campaignId}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.GetCampaignDashboardSummariesAdvertisers(advertiserId, campaignId);
                });

                if (summaries != null)
                    result.body = GetDashboardSummariesToCovert(summaries);
                else
                    result.result = 0;
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetDashboardSummaryForSalesAdvertiser";
                await _logServ.LogError();
                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/GetCampDashboardSummarySalesExec/{id}")]
        public async Task<ReturnResult> GetCampDashboardSummarySalesExec(int id = 0)
        {
            try
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
                string key = $"SALES_EXEC_DASHBOARD_STATS_{id}";
                var summaries = await _cache.GetOrCreateAsync<CampaignDashboardChartPREResult>(key, cacheEntry =>
                {
                    cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(30);
                    return _auditDAL.DashboardSummariesSalesExec(id);
                });

                if (summaries != null)
                    result.body = GetDashboardSummariesToCovert(summaries);
                else
                    result.result = 0;
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


    }
}