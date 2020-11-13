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

namespace AdtonesAdminWebApi.BusinessServices
{
    
    public class ManagementReportService : IManagementReportService
    {
        private readonly IManagementReportDAL _reportDAL;
        private readonly ICurrencyConversion _curConv;
        private readonly ICurrencyDAL _curDAL;
        private readonly IHttpContextAccessor _httpAccessor;
        private IMemoryCache _cache;
        private readonly IConnectionStringService _connService;
        ReturnResult result = new ReturnResult();


        public ManagementReportService(IManagementReportDAL reportDAL, ICurrencyConversion curConv, ICurrencyDAL curDAL, 
            IHttpContextAccessor httpAccessor, IMemoryCache cache, IConnectionStringService connService)
        {
            _reportDAL = reportDAL;
            _curConv = curConv;
            _curDAL = curDAL;
            _httpAccessor = httpAccessor;
            _cache = cache;
            _connService = connService;
        }


        private ManagementReportsSearch SetDefaults(ManagementReportsSearch search)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(2);
            var old = today.AddDays(-5000);

            if (search.DateTo == null || search.DateTo < old || search.DateTo < search.DateFrom)
                search.DateTo = tomorrow;
            else
                search.DateTo.Value.AddDays(1).Add(new TimeSpan(0, 0, 0));

            if (search.DateFrom == null || search.DateFrom < old || search.DateFrom > search.DateTo)
                search.DateFrom = old;
            else
                search.DateFrom.Value.Add(new TimeSpan(0, 0, 0));

            if (search.operators == null || search.operators.Length == 0)
            {
                List<int> ops = _reportDAL.GetAllOperators().Result.ToList();
                search.operators = ops.ToArray();
            }

            if (search.currency == null || search.currency == 0)
            {
                var currency = _curDAL.GetDisplayCurrencyCodeForUserAsync(_httpAccessor.GetUserIdFromJWT()).Result;
                search.currency = currency.CurrencyId;
            }

            return search;
        }


        public async Task<ReturnResult> GetReportData(ManagementReportsSearch search)
        {
            try
            {
                ManagementReportModel model = await GetManReports(search);
                if (model == null)
                    result.result = 0;
                else
                    result.body = model;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportService",
                    ProcedureName = "GetQueries"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        private async Task<ManagementReportModel> GetManReports(ManagementReportsSearch search)
        {
            var searchString = System.Text.Json.JsonSerializer.Serialize(search);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(40));
            string key = $"MAMAGEMENT_REPORT_{searchString.ToString() + _httpAccessor.GetUserIdFromJWT().ToString()}";
            return await _cache.GetOrCreateAsync<ManagementReportModel>(key, cacheEntry =>
            {
                cacheEntry.SlidingExpiration = TimeSpan.FromMinutes(40);
                return GetManReportTestAsync(search);
            });
        }


        //private async Task<ManagementReportModel> GetManReport(ManagementReportsSearch search)
        //{
        //    search = SetDefaults(search);

        //    ManagementReportModel model = new ManagementReportModel();
        //    //List<ManagementReportModel> models = new List<ManagementReportModel>();

        //    try
        //    {
        //        //foreach (var op in search.operators)
        //        //{
        //        //    var constring = await _connService.GetConnectionStringByOperator(op);
        //        //    if (constring != null && constring.Length > 10)
        //        //    {

        //        /// Temp holding
        //        var op = 1;
        //        var constring = "fconn";
        //        // sets the times correct for the server
        //        //if (op == (int)Enums.OperatorTableId.Safaricom)
        //        //{
        //        //search.DateFrom = search.DateFrom.Value.AddHours(-2);
        //        // search.DateTo = search.DateTo.Value.AddHours(22);
        //        // }

        //        // Separated this out as conversion likely to take more time than the initial fetch.
        //        /// Spend & Credit
        //        IEnumerable<SpendCredit> totCosts = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetTotalCost, op, constring);
        //        var costAudit = totCosts.ToList();
        //        Task<TotalCostCredit> totCost = CalculateConvertedSpendCredit(costAudit, search);

        //        IEnumerable<SpendCredit> amtsSpent = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetAmountSpent, op, constring);
        //        var spentAudit = amtsSpent.ToList();
        //        Task<TotalCostCredit> amtSpent = CalculateConvertedSpendCredit(spentAudit, search);

        //        IEnumerable<SpendCredit> amtsCredit = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetAmountCredit, op, constring);
        //        var creditAudit = amtsCredit.ToList();
        //        Task<TotalCostCredit> amtCredit = CalculateConvertedSpendCredit(creditAudit, search);

        //        /// Subscribers
        //        Task<ManRepUsers> totUser = _reportDAL.GetManReportsForUsers(search, ManagementReportQuery.TotalUsers, op, constring);
        //        Task<int> totListen = _reportDAL.GetreportInts(search, ManagementReportQuery.TotalListened, op, constring);
        //        Task<int> numListen = _reportDAL.GetreportInts(search, ManagementReportQuery.NumListened, op, constring);

        //        /// Campaigns & Adverts
        //        Task<int> numads = _reportDAL.GetreportInts(search, ManagementReportQuery.NumberOfAdsProvisioned, op, constring);
        //        Task<int> totads = _reportDAL.GetreportInts(search, ManagementReportQuery.TotalAdsProvisioned, op, constring);
        //        Task<int> totCam = _reportDAL.GetreportInts(search, ManagementReportQuery.TotalLiveCampaign, op, constring);
        //        Task<int> numCam = _reportDAL.GetreportInts(search, ManagementReportQuery.NumLiveCampaign, op, constring);

        //        /// Plays
        //        Task<CampaignTableManReport> totPlays = _reportDAL.GetReportPlayLengths(search, ManagementReportQuery.TotalPlayStuff, op, constring);
        //        Task<CampaignTableManReport> numPlays = _reportDAL.GetReportPlayLengths(search, ManagementReportQuery.NumOfPlayStuff, op, constring);


        //        await Task.WhenAll(
        //                            totUser,
        //                            totListen,
        //                            numListen,
        //                            totads,
        //                            numads,
        //                            totCam,
        //                            numCam,
        //                            totPlays,
        //                            numPlays,
        //                            totCost,
        //                            amtCredit,
        //                            amtSpent
        //                            );
        //        var currency = await _curDAL.GetCurrencyUsingCurrencyIdAsync(search.currency);

        //        var eqOver6 = totPlays.Result;
        //        var numPlay = numPlays.Result;
        //        var usrs = totUser.Result;
        //        /// Users
        //        model.TotalUsers += usrs.TotalUsers;
        //        model.TotalListened += totListen.Result;
        //        model.TotalRemovedUser += usrs.TotalRemovedUser;
        //        model.AddedUsers += usrs.AddedUsers;
        //        model.NumListened += numListen.Result;

        //        /// Campaigns & Adverts
        //        model.TotalAdverts += totads.Result;
        //        model.AdvertsProvisioned += numads.Result;
        //        model.TotalCampaigns += totCam.Result;
        //        model.CampaignsAdded += numCam.Result;
        //        model.TotalCancelled += eqOver6.NumCancelled;
        //        /// Plays
        //        model.TotalEmail += eqOver6.NumOfEmail;
        //        model.TotalSMS += eqOver6.NumOfSMS;
        //        //model.TotalPlays += eqOver6.TotalPlays;
        //        model.TotalPlayLength += eqOver6.Playlength;
        //        model.Total6Over += eqOver6.NumOfPlaySixOver;
        //        model.TotalUnder6 += eqOver6.NumOfPlayUnderSix;

        //        model.NumEmail += numPlay.NumOfEmail;
        //        model.NumSMS += numPlay.NumOfSMS;
        //        //model.TotalPlays += eqOver6.TotalPlays;
        //        model.PeriodPlayLength += numPlay.Playlength;
        //        model.Num6Over += numPlay.NumOfPlaySixOver;
        //        model.NumUnder6 += numPlay.NumOfPlayUnderSix;

        //        /// Credit & Spend
        //        model.TotalCredit += (int)totCost.Result.TotalCredit;
        //        model.TotalSpend += (int)totCost.Result.TotalSpend;
        //        model.AmountSpent += (int)amtSpent.Result.TotalSpend;
        //        model.AmountCredit += (int)amtCredit.Result.TotalCredit;
        //        model.CurrencyCode = GetCurrencySymbol(currency.CurrencyCode);
        //        //    }
        //        //}

        //        model.TotalAvgPlayLength = (double)((model.TotalPlayLength) / 1000) / (model.Total6Over + model.TotalUnder6);
        //        model.NumAvgPlayLength = (double)((model.PeriodPlayLength) / 1000) / (model.Num6Over + model.NumUnder6);

        //        model.TotalAvgPlays = model.TotalUsers == 0 ? 0 : (double)(model.Total6Over + model.TotalUnder6) / (double)model.TotalUsers;
        //        model.TotalAvgPlaysListened = model.TotalListened == 0 ? 0 : (double)(model.Total6Over + model.TotalUnder6) / (double)model.TotalListened;

        //        model.NumAvgPlays = model.TotalUsers == 0 ? 0 : (double)(model.Num6Over + model.NumUnder6) / (double)model.TotalUsers;
        //        model.NumAvgPlaysListened = model.NumListened == 0 ? 0 : (double)(model.Num6Over + model.NumUnder6) / (double)model.NumListened;

        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "ManagementReportService",
        //            ProcedureName = "GetQueries"
        //        };
        //        _logging.LogError();

        //    }
        //    return model;
        //}


        private async Task<ManagementReportModel> GetManReportTestAsync(ManagementReportsSearch search)
        {
            search = SetDefaults(search);

            var model = new ManagementReportModel();
            var models = new List<ManagementReportModel>();
            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions
            { CancellationToken = cts.Token };

            Parallel.ForEach(search.operators,
            options,
            () => model,
            (item, loopState, localCount) =>
            {
                cts.Token.ThrowIfCancellationRequested();
                ManagementReportModel distance = GetManReportAsync(search, item).Result;
                return distance;
            },
            (tempResult) =>
            {
                if (tempResult != null)
                {
                    // models.Add(tempResult);
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
                    model.CurrencyCode = model.CurrencyCode;
                }
            });

            model.TotalAvgPlayLength = (double)((model.TotalPlayLength) / 1000) / (model.TotalPlays);
            model.NumAvgPlayLength = (double)((model.PeriodPlayLength) / 1000) / (model.NumPlays);

            model.TotalAvgPlays = model.TotalUsers == 0 ? 0 : (double)(model.TotalPlays) / (double)model.TotalUsers;
            model.TotalAvgPlaysListened = model.TotalListened == 0 ? 0 : (double)(model.TotalPlays) / (double)model.TotalListened;

            model.NumAvgPlays = model.TotalUsers == 0 ? 0 : (double)(model.Num6Over + model.NumUnder6) / (double)model.TotalUsers;
            model.NumAvgPlaysListened = model.NumListened == 0 ? 0 : (double)(model.NumPlays) / (double)model.NumListened;

            return model;
        }


        private async Task<ManagementReportModel> GetManReportAsync(ManagementReportsSearch search, int op)
        {
            ManagementReportModel model = new ManagementReportModel();

            try
            {
                var constring = await _connService.GetConnectionStringByOperator(op);
                if (constring != null && constring.Length > 10)
                {
                    search.connstring = constring;
                    search.singleOperator = op;

                    // Separated this out as conversion likely to take more time than the initial fetch.
                    /// Spend & Credit
                    IEnumerable<SpendCredit> totCosts = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetTotalCost, op, constring);
                    var costAudit = totCosts.ToList();
                    Task<TotalCostCredit> totCost = CalculateConvertedSpendCredit(costAudit, search);

                    IEnumerable<SpendCredit> amtsSpent = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetAmountSpent, op, constring);
                    var spentAudit = amtsSpent.ToList();
                    Task<TotalCostCredit> amtSpent = CalculateConvertedSpendCredit(spentAudit, search);

                    IEnumerable<SpendCredit> amtsCredit = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetAmountCredit, op, constring);
                    var creditAudit = amtsCredit.ToList();
                    Task<TotalCostCredit> amtCredit = CalculateConvertedSpendCredit(creditAudit, search);

                    /// Subscribers
                    Task<ManRepUsers> totUser = _reportDAL.GetManReportsForUsers(search, ManagementReportQuery.TotalUsers, op, constring);
                    Task<TwoDigitsManRep> totListen = _reportDAL.GetreportDoubleInts(search, ManagementReportQuery.TotalListened, op, constring);
                    // Task<int> numListen = _reportDAL.GetreportInts(search, ManagementReportQuery.NumListened, op, constring);

                    /// Campaigns & Adverts
                    // Task<int> numads = _reportDAL.GetreportInts(search, ManagementReportQuery.NumberOfAdsProvisioned, op, constring);
                    Task<TwoDigitsManRep> totads = _reportDAL.GetreportDoubleInts(search, ManagementReportQuery.TotalAdsProvisioned, op, constring);
                    Task<TwoDigitsManRep> totCam = _reportDAL.GetreportDoubleInts(search, ManagementReportQuery.TotalLiveCampaign, op, constring);
                    // Task<int> numCam = _reportDAL.GetreportInts(search, ManagementReportQuery.NumLiveCampaign, op, constring);

                    /// Plays
                    Task<CampaignTableManReport> totPlays = _reportDAL.GetReportPlayLengths(search, ManagementReportQuery.TotalPlayStuff, op, constring);
                    // Task<CampaignTableManReport> numPlays = _reportDAL.GetReportPlayLengths(search, ManagementReportQuery.NumOfPlayStuff, op, constring);


                    await Task.WhenAll(
                                                totUser,
                                                totListen,
                                                // numListen,
                                                totads,
                                                // numads,
                                                totCam,
                                                // numCam,
                                                totPlays,
                                                // numPlays,
                                                totCost,
                                                amtCredit,
                                                amtSpent
                                                );
                    var currency = await _curDAL.GetCurrencyUsingCurrencyIdAsync(search.currency);

                    // var eqOver6 = totPlays.Result;
                    var numPlay = totPlays.Result;
                    var usrs = totUser.Result;
                    var camps = totCam.Result;
                    var ads = totads.Result;
                    var listen = totListen.Result;
                    /// Users
                    model.TotalUsers = usrs.TotalUsers;
                    model.TotalListened = listen.TotalItem;
                    model.TotalRemovedUser = usrs.TotalRemovedUser;
                    model.AddedUsers = usrs.AddedUsers;
                    model.NumListened = listen.NumItem;

                    /// Campaigns & Adverts
                    model.TotalAdverts = ads.TotalItem;
                    model.AdvertsProvisioned = ads.NumItem;
                    model.TotalCampaigns = camps.TotalItem;
                    model.CampaignsAdded = camps.NumItem;
                    model.TotalCancelled = numPlay.TotCancelled;
                    model.NumCancelled = numPlay.NumCancelled;
                    /// Plays
                    model.TotalEmail = numPlay.TotOfEmail;
                    model.TotalSMS = numPlay.TotOfSMS;
                    //model.TotalPlays = eqOver6.TotalPlays;
                    model.TotalPlayLength = numPlay.TotPlaylength;
                    model.Total6Over = numPlay.TotOfPlaySixOver;
                    model.TotalUnder6 = numPlay.TotOfPlayUnderSix;

                    model.NumEmail = numPlay.NumOfEmail;
                    model.NumSMS = numPlay.NumOfSMS;
                    //model.TotalPlays = eqOver6.TotalPlays;
                    model.PeriodPlayLength = numPlay.Playlength;
                    model.Num6Over = numPlay.NumOfPlaySixOver;
                    model.NumUnder6 = numPlay.NumOfPlayUnderSix;
                    // Total Plays
                    model.TotalPlays = numPlay.TotalPlays;
                    model.NumPlays = numPlay.Plays;

                    /// Credit & Spend
                    model.TotalCredit = (int)totCost.Result.TotalCredit;
                    model.TotalSpend = (int)totCost.Result.TotalSpend;
                    model.AmountSpent = (int)amtSpent.Result.TotalSpend;
                    model.AmountCredit = (int)amtCredit.Result.TotalCredit;
                    model.CurrencyCode = GetCurrencySymbol(currency.CurrencyCode);
                }
                else
                {
                    model = null;
                }

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportService",
                    ProcedureName = "GetQueries"
                };
                _logging.LogError();

            }
            return model;
        }


        //public async Task<XLWorkbook> GenerateExcelReport(ManagementReportsSearch search)
        //{

        //    var wb = new XLWorkbook();

        //    List<ManagementReportModel> mappingResult = new List<ManagementReportModel>();
        //    ManagementReportModel model = new ManagementReportModel();

        //    model = await GetManReports(search);
        //    mappingResult.Add(model);

        //    // Get defaults or real to show in report
        //    search = SetDefaults(search);

        //    string[] operatorArray = _reportDAL.GetOperatorNames(search).Result.ToList().ToArray();
        //    //string[] operatorArray = ops.ToArray();
        //    string operatorName = string.Join(", ", operatorArray);

        //    string fromDate = "", toDate = "";
        //    fromDate = search.DateFrom.ToString();
        //    toDate = search.DateTo.ToString();


        //    var ws = wb.Worksheets.Add("Management Report");
        //    ws.Style.Font.FontSize = 9;
        //    ws.Range("A1" + ":" + "N1").Merge().Value = "Management Report Data";
        //    ws.Range("A1" + ":" + "N1").Style.Font.FontSize = 14;
        //    ws.Range("A1" + ":" + "N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("A1" + ":" + "N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("A1" + ":" + "N1").Style.Font.Bold = true;
        //    ws.Columns("A:M").Width = 25;

        //    ws.Range("A2" + ":" + "B2").Merge().Value = "Operator";
        //    ws.Range("A2" + ":" + "B2").Style.Font.FontSize = 12;
        //    ws.Range("A2" + ":" + "B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("A2" + ":" + "B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("A2" + ":" + "B2").Style.Font.Bold = true;
        //    ws.Columns("A:B").Width = 10;

        //    ws.Range("C2" + ":" + "D2").Merge().Value = operatorName.ToString();
        //    ws.Range("C2" + ":" + "D2").Style.Font.FontSize = 10;
        //    ws.Range("C2" + ":" + "D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("C2" + ":" + "D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Columns("C:D").Width = 10;

        //    ws.Range("A3" + ":" + "B3").Merge().Value = "Date";
        //    ws.Range("A3" + ":" + "B3").Style.Font.FontSize = 12;
        //    ws.Range("A3" + ":" + "B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("A3" + ":" + "B3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("A3" + ":" + "B3").Style.Font.Bold = true;
        //    ws.Columns("A:B").Width = 10;

        //    ws.Range("C3" + ":" + "D3").Merge().Value = fromDate + " - " + toDate;
        //    ws.Range("C3" + ":" + "D3").Style.Font.FontSize = 10;
        //    ws.Range("C3" + ":" + "C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("C3" + ":" + "C3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Columns("C:D").Width = 10;

        //    ws.Range("A4" + ":" + "A5").Merge().Value = "Total Users";
        //    ws.Range("A4" + ":" + "A5").Style.Font.FontSize = 12;
        //    ws.Range("A4" + ":" + "A5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("A4" + ":" + "A5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("A4" + ":" + "A5").Style.Font.Bold = true;
        //    ws.Column("A").Width = 15;

        //    ws.Range("B4" + ":" + "B5").Merge().Value = "Removed Users";
        //    ws.Range("B4" + ":" + "B5").Style.Font.FontSize = 12;
        //    ws.Range("B4" + ":" + "B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("B4" + ":" + "B5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("B4" + ":" + "B5").Style.Font.Bold = true;
        //    ws.Column("B").Width = 18;

        //    ws.Range("C4" + ":" + "C5").Merge().Value = "Plays";
        //    ws.Range("C4" + ":" + "C5").Style.Font.FontSize = 12;
        //    ws.Range("C4" + ":" + "C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("C4" + ":" + "C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("C4" + ":" + "C5").Style.Font.Bold = true;
        //    ws.Column("C").Width = 10;

        //    ws.Range("D4" + ":" + "D5").Merge().Value = "Plays (Under 6sec)";
        //    ws.Range("D4" + ":" + "D5").Style.Font.FontSize = 12;
        //    ws.Range("D4" + ":" + "D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("D4" + ":" + "D5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("D4" + ":" + "D5").Style.Font.Bold = true;
        //    ws.Column("D").Width = 20;

        //    ws.Range("E4" + ":" + "E5").Merge().Value = "SMS";
        //    ws.Range("E4" + ":" + "E5").Style.Font.FontSize = 12;
        //    ws.Range("E4" + ":" + "E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("E4" + ":" + "E5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("E4" + ":" + "E5").Style.Font.Bold = true;
        //    ws.Column("E").Width = 10;

        //    ws.Range("F4" + ":" + "F5").Merge().Value = "Email";
        //    ws.Range("F4" + ":" + "F5").Style.Font.FontSize = 12;
        //    ws.Range("F4" + ":" + "F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("F4" + ":" + "F5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("F4" + ":" + "F5").Style.Font.Bold = true;
        //    ws.Column("F").Width = 10;

        //    ws.Range("G4" + ":" + "G5").Merge().Value = "Live Campaign";
        //    ws.Range("G4" + ":" + "G5").Style.Font.FontSize = 12;
        //    ws.Range("G4" + ":" + "G5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("G4" + ":" + "G5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("G4" + ":" + "G5").Style.Font.Bold = true;
        //    ws.Column("G").Width = 18;

        //    ws.Range("H4" + ":" + "H5").Merge().Value = "Ads provisioned";
        //    ws.Range("H4" + ":" + "H5").Style.Font.FontSize = 12;
        //    ws.Range("H4" + ":" + "H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("H4" + ":" + "H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("H4" + ":" + "H5").Style.Font.Bold = true;
        //    ws.Column("H").Width = 20;

        //    ws.Range("I4" + ":" + "I5").Merge().Value = $"Total Spend (in {model.CurrencyCode})";
        //    ws.Range("I4" + ":" + "I5").Style.Font.FontSize = 12;
        //    ws.Range("I4" + ":" + "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("I4" + ":" + "I5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("I4" + ":" + "I5").Style.Font.Bold = true;
        //    ws.Column("I").Width = 25;

        //    ws.Range("J4" + ":" + "J5").Merge().Value = $"Total Credit (in {model.CurrencyCode})";
        //    ws.Range("J4" + ":" + "J5").Style.Font.FontSize = 12;
        //    ws.Range("J4" + ":" + "J5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("J4" + ":" + "J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("J4" + ":" + "J5").Style.Font.Bold = true;
        //    ws.Column("J").Width = 25;

        //    ws.Range("K4" + ":" + "K5").Merge().Value = "Total Cancel";
        //    ws.Range("K4" + ":" + "K5").Style.Font.FontSize = 12;
        //    ws.Range("K4" + ":" + "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("K4" + ":" + "K5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("K4" + ":" + "K5").Style.Font.Bold = true;
        //    ws.Column("K").Width = 15;

        //    ws.Range("L4" + ":" + "L5").Merge().Value = "Average Plays Per User";
        //    ws.Range("L4" + ":" + "L5").Style.Font.FontSize = 12;
        //    ws.Range("L4" + ":" + "L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("L4" + ":" + "L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("L4" + ":" + "L5").Style.Font.Bold = true;
        //    ws.Column("L").Width = 25;

        //    ws.Range("M4" + ":" + "M5").Merge().Value = "Text Files Processed";
        //    ws.Range("M4" + ":" + "M5").Style.Font.FontSize = 12;
        //    ws.Range("M4" + ":" + "M5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("M4" + ":" + "M5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("M4" + ":" + "M5").Style.Font.Bold = true;
        //    ws.Column("M").Width = 25;

        //    ws.Range("N4" + ":" + "N5").Merge().Value = "Text Lines Processed";
        //    ws.Range("N4" + ":" + "N5").Style.Font.FontSize = 12;
        //    ws.Range("N4" + ":" + "N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
        //    ws.Range("N4" + ":" + "N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
        //    ws.Range("N4" + ":" + "N5").Style.Font.Bold = true;
        //    ws.Column("N").Width = 25;



        //    int first = 5;
        //    int last = first;
        //    int excelrowno = first;
        //    if (mappingResult.Count() > 0)
        //    {
        //        for (int i = 0; i < mappingResult.Count(); i++)
        //        {
        //            excelrowno += 1;
        //            int j = excelrowno;

        //            ws.Cell("A" + j.ToString()).Value = mappingResult[i].NumOfTotalUser.ToString();
        //            ws.Cell("B" + j.ToString()).Value = mappingResult[i].NumOfRemovedUser.ToString();
        //            ws.Cell("C" + j.ToString()).Value = mappingResult[i].NumOfPlay.ToString();
        //            ws.Cell("D" + j.ToString()).Value = mappingResult[i].NumOfPlayUnder6secs.ToString();
        //            ws.Cell("E" + j.ToString()).Value = mappingResult[i].NumOfSMS.ToString();
        //            ws.Cell("F" + j.ToString()).Value = mappingResult[i].NumOfEmail.ToString();
        //            ws.Cell("G" + j.ToString()).Value = mappingResult[i].NumOfLiveCampaign.ToString();
        //            ws.Cell("H" + j.ToString()).Value = mappingResult[i].NumberOfAdsProvisioned.ToString();
        //            ws.Cell("I" + j.ToString()).Value = mappingResult[i].TotalSpend.ToString("N");
        //            ws.Cell("J" + j.ToString()).Value = mappingResult[i].TotalCredit.ToString("N");
        //            ws.Cell("K" + j.ToString()).Value = mappingResult[i].NumOfCancel.ToString();
        //            ws.Cell("L" + j.ToString()).Value = mappingResult[i].AveragePlaysPerUser.ToString();
        //            ws.Cell("M" + j.ToString()).Value = mappingResult[i].NumOfTextFile.ToString();
        //            ws.Cell("N" + j.ToString()).Value = mappingResult[i].NumOfUpdateToAudit.ToString();
        //        }
        //    }

        //    return wb;
        //}


        public class CurrencyListing
        {
            public string CurrencyCode { get; set; }
            public decimal CurrencyRate { get; set; }
        }


        private decimal GetCurrencyRateModel(string from, string to)
        {
            return _curConv.Convert(1, from, to);
        }


        private async Task<List<CurrencyListing>> GetConvertedCurrency(List<SpendCredit> creditList,ManagementReportsSearch search)
        {
            string toCurrencyCode = "GBP";
            var clList = new List<CurrencyListing>();
            try
            {
                var currency = creditList.Select(y => y.CurrencyCode).Distinct().ToList();
                var currencySelectionData = await _curDAL.GetCurrencyUsingCurrencyIdAsync(search.currency);
                toCurrencyCode = currencySelectionData.CurrencyCode;
                
                foreach (string cur in currency)
                {
                    var cl = new CurrencyListing();

                    if (cur == toCurrencyCode)
                    {
                        cl.CurrencyCode = toCurrencyCode;
                        cl.CurrencyRate = 1;
                        clList.Add(cl);
                    }
                    else
                    {
                        cl.CurrencyCode = cur;

                        cl.CurrencyRate = GetCurrencyRateModel(cur, toCurrencyCode);
                        clList.Add(cl);
                    }
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportService",
                    ProcedureName = "GetConvertedCurrency"
                };
                _logging.LogError();

            }
            return clList;
        }


        private string GetCurrencySymbol(string currencyCode)
        {
            switch (currencyCode)
            {
                case "GBP": return "£";
                case "USD": return "$";
                case "XOF": return "CFA";
                case "EUR": return "€";
                case "KES": return "Ksh";
                default: return "£";
            }
        }

        
        private async Task<TotalCostCredit> CalculateConvertedSpendCredit(List<SpendCredit> creditList, ManagementReportsSearch search)
        {
            TotalCostCredit campaignAudit = new TotalCostCredit();
            try
            {

                if (creditList.Count > 0)
                {
                    var currencyConv = await GetConvertedCurrency(creditList, search);

                    foreach (var campaignAuditItem in creditList)
                    {
                        var currencyRate = currencyConv.Where(kv => kv.CurrencyCode.Contains(campaignAuditItem.CurrencyCode)).Select(kv => kv.CurrencyRate).FirstOrDefault();
                        if (currencyRate == 0)
                            currencyRate = 1;

                        campaignAudit.TotalSpend = campaignAudit.TotalSpend + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCost) * currencyRate));
                        campaignAudit.TotalCredit = campaignAudit.TotalCredit + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCredit) * currencyRate));
                    }
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportService",
                    ProcedureName = "CalculateConvertedSpendCredit"
                };
                _logging.LogError();

            }

            return campaignAudit;
        }

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



    }
}
