using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Threading;
using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using AdtonesAdminWebApi.BusinessServices.ManagementReports;

namespace AdtonesAdminWebApi.BusinessServices
{
    
    public class ManagementReportService : IManagementReportService
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private IMemoryCache _cache;
        private readonly ILoggingService _logServ;
        private readonly IGenerateReportDataByOperator _getreportData;
        private readonly ISetDefaultParameters _defaults;
        const string PageName = "ManagementReportService";


        public ManagementReportService(IHttpContextAccessor httpAccessor, IMemoryCache cache,
                                        ILoggingService logServ,
                                        IGenerateReportDataByOperator getreportData,
                                        ISetDefaultParameters defaults)
        {
            _httpAccessor = httpAccessor;
            _cache = cache;
            _logServ = logServ;
            _getreportData = getreportData;
            _defaults = defaults;
        }

        public async Task<ReturnResult> GetReportData(ManagementReportsSearch search)
        {
            var result = new ReturnResult();
            try
            {
                ManagementReportModel model = await GetCachedOrNewReportData(search);
                if (model == null)
                    result.result = 0;
                else
                    result.body = model;
                
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetReportData";
                await _logServ.LogError();
                
                result.result = 0;
                return result;
            }
        }


        public async Task<ManagementReportModel> GetCachedOrNewReportData(ManagementReportsSearch search)
        {
            var searchString = System.Text.Json.JsonSerializer.Serialize(search);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(40));
            string key = $"MAMAGEMENT_REPORT_{searchString.ToString() + _httpAccessor.GetUserIdFromJWT().ToString()}";
            return await _cache.GetOrCreateAsync<ManagementReportModel>(key, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(40);
                return GetManReportDataAsync(search);
            });
        }


        private async Task<ManagementReportModel> GetManReportDataAsync(ManagementReportsSearch search)
        {
            var model = new ManagementReportModel();
            var models = new List<ManagementReportModel>();

            //// Currently hardcoded in SetDefaultParameters to the 2 available
            //List<int> ops = null; // _reportDAL.GetAllOperators().Result.ToList();
            //var currency = _curDAL.GetDisplayCurrencyCodeForUserAsync(_httpAccessor.GetUserIdFromJWT()).Result;

            //var defaults = new SetDefaultParameters();
            search = _defaults.SetDefaults(search);

            try
            {
                for(int i=0; i<search.operators.Length;i++)
                {
                    models.Add(await _getreportData.GetReportDataByOperator(search, search.operators[i]));
                }

                foreach (var tempResult in models)
                {
                    /// Users
                    model.TotalUsers += tempResult.TotalUsers;
                    model.TotalListened += tempResult.TotalListened;
                    model.TotalRemovedUser += tempResult.TotalRemovedUser;
                    model.AddedUsers += tempResult.AddedUsers;
                    model.NumListened += tempResult.NumListened;

                    /// Campaigns & Adverts
                    model.TotalAdverts += tempResult.TotalAdverts;
                    model.AdvertsProvisioned += tempResult.AdvertsProvisioned;
                    model.TotalCampaigns += tempResult.TotalCampaigns;
                    model.CampaignsAdded += tempResult.CampaignsAdded;
                    model.TotalCancelled += tempResult.TotalCancelled;
                    model.NumCancelled += tempResult.NumCancelled;
                    /// Plays
                    model.TotalEmail += tempResult.TotalEmail;
                    model.TotalSMS += tempResult.TotalSMS;
                    //model.TotalPlays += tempResult.TotalPlays;
                    model.TotalPlayLength += tempResult.TotalPlayLength;
                    model.Total6Over += tempResult.Total6Over;
                    model.TotalUnder6 += tempResult.TotalUnder6;

                    model.NumEmail += tempResult.NumEmail;
                    model.NumSMS += tempResult.NumSMS;
                    //model.TotalPlays += tempResult.TotalPlays;
                    model.PeriodPlayLength += tempResult.PeriodPlayLength;
                    model.Num6Over += tempResult.Num6Over;
                    model.NumUnder6 += tempResult.NumUnder6;

                    model.TotalPlays += tempResult.TotalPlays;
                    model.NumPlays += tempResult.NumPlays;

                    /// Credit & Spend
                    model.TotalCredit += (int)tempResult.TotalCredit;
                    model.TotalSpend += (int)tempResult.TotalSpend;
                    model.AmountSpent += (int)tempResult.AmountSpent;
                    model.AmountCredit += (int)tempResult.AmountCredit;
                    model.CurrencyCode = tempResult.CurrencyCode;

                    /// Rewards 
                    model.TotRewardUsers += tempResult.TotRewardUsers;
                    model.NumRewardUsers += tempResult.NumRewardUsers;
                    model.TotalRewards += tempResult.TotalRewards;
                    model.NumRewards += tempResult.NumRewards;
                }

                model.TotalAvgPlayLength = model.TotalPlayLength == 0 ? 0 : (double)((model.TotalPlayLength) / 1000) / (model.TotalPlays);
                model.NumAvgPlayLength = model.PeriodPlayLength == 0 ? 0 : (double)((model.PeriodPlayLength) / 1000) / (model.NumPlays);

                model.TotalAvgPlays = model.TotalUsers == 0 ? 0 : (double)(model.TotalPlays) / (double)model.TotalUsers;
                model.TotalAvgPlaysListened = model.TotalListened == 0 ? 0 : (double)(model.TotalPlays) / (double)model.TotalListened;

                model.NumAvgPlays = model.TotalUsers == 0 ? 0 : (double)(model.Num6Over + model.NumUnder6) / (double)model.TotalUsers;
                model.NumAvgPlaysListened = model.NumListened == 0 ? 0 : (double)(model.NumPlays) / (double)model.NumListened;

                model.TotAvgRewards = model.TotalRewards == 0 ? 0 : (decimal)(model.TotalRewards) / (decimal)model.TotRewardUsers;
                model.NumAvgRewards = model.NumRewards == 0 ? 0 : (decimal)(model.NumRewards) / (decimal)model.NumRewardUsers;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetManReportsTestAsync";
                await _logServ.LogError();
            }

            return model;
        }
    }
}

//private string GetCurrencySymbol(string currencyCode)
//{
//    switch (currencyCode)
//    {
//        case "GBP": return "£";
//        case "USD": return "$";
//        case "XOF": return "CFA";
//        case "EUR": return "€";
//        case "KES": return "Ksh";
//        default: return "£";
//    }
//}



//private async Task<List<CurrencyListing>> GetConvertedCurrency(List<SpendCredit> creditList)
//{
//    string toCurrencyCode = "GBP";
//    var currency = creditList.Select(y => y.CurrencyCode).Distinct().ToList();
//    if (!currency.Contains("GBP"))
//        currency.Add("GBP");

//    var clList = new List<CurrencyListing>();
//    foreach (string cur in currency)
//    {
//        var cl = new CurrencyListing();

//        if (cur == "GBP")
//        {
//            cl.CurrencyCode = "GBP";
//            cl.CurrencyRate = 1;
//            clList.Add(cl);
//        }
//        else
//        {
//            cl.CurrencyCode = cur;

//            cl.CurrencyRate = GetCurrencyRateModel(cur, toCurrencyCode);
//            clList.Add(cl);
//        }
//    }
//    return clList;
//}

//private async Task<List<CurrencyListing>> GetConvertedCurrency(List<SpendCredit> creditList, ManagementReportsSearch search)
//{
//    string toCurrencyCode = "GBP";
//    var clList = new List<CurrencyListing>();
//    try
//    {
//        var currency = creditList.Select(y => y.CurrencyCode).Distinct().ToList();
//        var currencySelectionData = await _curDAL.GetCurrencyUsingCurrencyIdAsync(search.currency);
//        toCurrencyCode = currencySelectionData.CurrencyCode;

//        foreach (string cur in currency)
//        {
//            var cl = new CurrencyListing();

//            if (cur == toCurrencyCode)
//            {
//                cl.CurrencyCode = toCurrencyCode;
//                cl.CurrencyRate = 1;
//                clList.Add(cl);
//            }
//            else
//            {
//                cl.CurrencyCode = cur;

//                cl.CurrencyRate = _curConv.GetCurrencyRateModel(cur, toCurrencyCode);
//                clList.Add(cl);
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        _logServ.ErrorMessage = ex.Message.ToString();
//        _logServ.StackTrace = ex.StackTrace.ToString();
//        _logServ.PageName = PageName;
//        _logServ.ProcedureName = "GetConvertedCurrency";
//        await _logServ.LogError();
//    }
//    return clList;
//}


//private async Task<TotalCostCredit> CalculateConvertedSpendCredit(List<SpendCredit> creditList, ManagementReportsSearch search)
//{
//    TotalCostCredit campaignAudit = new TotalCostCredit();
//    try
//    {

//        if (creditList.Count > 0)
//        {
//            var currencyConv = await GetConvertedCurrency(creditList, search);

//            foreach (var campaignAuditItem in creditList)
//            {
//                var currencyRate = currencyConv.Where(kv => kv.CurrencyCode.Contains(campaignAuditItem.CurrencyCode)).Select(kv => kv.CurrencyRate).FirstOrDefault();
//                if (currencyRate == 0)
//                    currencyRate = 1;

//                campaignAudit.TotalSpend = campaignAudit.TotalSpend + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCost) * currencyRate));
//                campaignAudit.TotalCredit = campaignAudit.TotalCredit + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCredit) * currencyRate));
//            }
//        }
//    }
//    catch (Exception ex)
//    {
//        _logServ.ErrorMessage = ex.Message.ToString();
//        _logServ.StackTrace = ex.StackTrace.ToString();
//        _logServ.PageName = PageName;
//        _logServ.ProcedureName = "CalculateConvertedSpendCredit";
//        await _logServ.LogError();

//    }

//    return campaignAudit;
//}
