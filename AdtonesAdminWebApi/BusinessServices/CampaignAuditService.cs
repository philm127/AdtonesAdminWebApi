using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
                                ICampaignAuditDAL auditDAL, ICampaignDAL campDAL, IMemoryCache cache, ILoggingService logServ)

        {
            _configuration = configuration;
            _connService = connService;
            _httpAccessor = httpAccessor;
            _auditDAL = auditDAL;
            _cache = cache;
            //_currencyConversion = CurrencyConversion.CreateForCurrentUserAsync(this).Result;
            _logServ = logServ;
        }


        public CampaignDashboardChartResult GetDashboardSummariesToCovert(CampaignDashboardChartPREResult summaries)
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
