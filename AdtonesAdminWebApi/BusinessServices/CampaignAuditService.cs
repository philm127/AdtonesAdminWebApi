using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
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
        private readonly CurrencyConversion _currencyConversion;

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

        public async Task<ReturnResult> GetCampaignDashboardSummariesOperators(int campaignId)
        {
            // var summary = await Task.FromResult(Task.FromResult(_auditDAL.GetCampaignDashboardSummariesOperators(campaignId)).ToList());
            //CampaignDashboardChartPREResult summaries = new CampaignDashboardChartPREResult();// (CampaignDashboardChartPREResult)summary;
            // var totalReach = await _summariesProvider.GetCampaignDashboardTotalReachSummaryForUser(efmvcUser.UserId);
            var summaries = await _auditDAL.GetCampaignDashboardSummariesForCampaign(campaignId);
            var totalBid = summaries.MaxBid;
            var avgBid = summaries.AvgBid;
            var totalPlays = summaries.TotalPlays;

            var _CampaignDashboardChartResult = new CampaignDashboardChartResult
            {
                PlaystoDate = summaries.TotalPlays,
                FreePlays = (int)summaries.FreePlays,
                SpendToDate = (double)summaries.Spend,
                AverageBid = (double)summaries.AvgBid,
                AveragePlayTime = (double)summaries.AvgPlayLength,
                FreePlaysPercentage = (double)summaries.FreePlaysPercentage,
                TotalBudget = summaries.Budget,
                TotalBudgetPercentage = (double)summaries.TotalBudgetPercentage,
                MaxBid = (double)summaries.MaxBid,
                MaxBidPercantage = (double)Math.Round(totalBid == 0 ? 0 : avgBid / totalBid, 2),
                MaxPlayLength = summaries.MaxPlayLength,
                SMSCost = (double)summaries.TotalSMSCost,
                EmailCost = (double)summaries.TotalEmailCost,
                Cancelled = 0,
                TotalPlayed = (int)summaries.TotalPlays,
                //TotalReach = (int)(totalReach?.Reach ?? 0),
                TotalSpend = (double)summaries.Spend,
                AvgMaxBid = (double)summaries.AvgBid,
                CurrencyCode = "KES",// _currencyConversion.DisplayCurrency.CurrencyCode,
                //MaxPlayLengthPercantage = totalPlays == 0 ? 0 : Math.Round((double)summaries.TotalValuablePlays / totalPlays, 2),
            };

            result.body = _CampaignDashboardChartResult;
            return result;
        }


    }
}
