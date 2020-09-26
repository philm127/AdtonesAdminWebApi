using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
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
        private readonly ICampaignDAL _campDAL;
        private readonly IConfiguration _configuration;
        private readonly IConnectionStringService _connService;
        // private readonly CurrencyConversion _currencyConversion;

        ReturnResult result = new ReturnResult();


        public CampaignAuditService(IConfiguration configuration, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                ICampaignAuditDAL auditDAL, ICampaignDAL campDAL)

        {
            _configuration = configuration;
            _connService = connService;
            _httpAccessor = httpAccessor;
            _auditDAL = auditDAL;
            _campDAL = campDAL;
            //_currencyConversion = CurrencyConversion.CreateForCurrentUserAsync(this).Result;
        }

        public async Task<ReturnResult> GetCampaignDashboardSummariesOperatorByCampaign(int campaignId)
        {
            try
            {
                // var summary = await Task.FromResult(Task.FromResult(_auditDAL.GetCampaignDashboardSummariesOperators(campaignId)).ToList());
                //CampaignDashboardChartPREResult summaries = new CampaignDashboardChartPREResult();// (CampaignDashboardChartPREResult)summary;
                // var totalReach = await _summariesProvider.GetCampaignDashboardTotalReachSummaryForUser(efmvcUser.UserId);
                var summaries = await _auditDAL.GetCampaignDashboardSummariesForOperator(campaignId);
                //var totalBid = summaries.MaxBid;
                //var avgBid = summaries.AvgBid;
                //var totalPlays = summaries.TotalPlays;

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

                result.body = _CampaignDashboardChartResult;
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignAuditService",
                    ProcedureName = "GetCampaignDashboardSummariesOperatorByCampaign"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
        }


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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignAuditService",
                    ProcedureName = "GetPlayDetailsForOperatorByCampaign"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> GetDashboardSummariesByOperator(int operatorId)
        {
            try
            {
                var summaries = await _auditDAL.GetDashboardSummariesForOperator(operatorId);
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

                    result.body = _CampaignDashboardChartResult;
                }
                else
                {
                    result.result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignAuditService",
                    ProcedureName = "GetDashboardSummariesByOperator"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
        }


        public async Task<ReturnResult> GetPromoCampaignDashboardSummary(int campaignId)
        {
            try
            {
                result.body = await _auditDAL.GetPromoCampaignDashboardSummaries(campaignId);
                
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignAuditService",
                    ProcedureName = "GetPromoCampaignDashboardSummary"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignAuditService",
                    ProcedureName = "GetPromoPlayDetails"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
        }


    }
}
