using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    
    public class ManagementReportService : IManagementReportService
    {
        private readonly IManagementReportDAL _reportDAL;
        private readonly ICurrencyConversion _curConv;
        private readonly ICurrencyDAL _curDAL;
        private readonly IHttpContextAccessor _httpAccessor;
        ReturnResult result = new ReturnResult();


        public ManagementReportService(IManagementReportDAL reportDAL, ICurrencyConversion curConv, ICurrencyDAL curDAL, IHttpContextAccessor httpAccessor)
        {
            _reportDAL = reportDAL;
            _curConv = curConv;
            _curDAL = curDAL;
            _httpAccessor = httpAccessor;
        }


        private ManagementReportsSearch SetDefaults(ManagementReportsSearch search)
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var old = today.AddDays(-5000);

            if (search.DateTo == null || search.DateTo < old || search.DateTo < search.DateFrom)
                search.DateTo = tomorrow;

            if (search.DateFrom == null || search.DateFrom < old || search.DateFrom > search.DateTo)
                search.DateFrom = old;

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
            search = SetDefaults(search);

            ManagementReportModel model = new ManagementReportModel();

            try
            {

                // Separated this out as conversion likely to take more time than the initial fetch.
                IEnumerable<SpendCredit> totCosts = await _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetTotalCost);
                var costAudit = totCosts.ToList();

                Task<TotalCostCredit> totCost = CalculateConvertedSpendCredit(costAudit, search);

                Task<int> totUser = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfTotalUser);
                Task<int> totrem = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfRemovedUser);
                Task<int> totads = _reportDAL.GetreportInts(search, ManagementReportQuery.NumberOfAdsProvisioned);
                //// Task<int> up2Aud = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfUpdateToAudit);

                Task<int> totCancel = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfCancel);
                Task<int> totCam = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfLiveCampaign);
                Task<int> totEmail = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfEmail);
                //Task<int> totFile = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfTextFile);
                //Task<int> totline = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfTextLine);
                Task<int> totSMS = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfSMS);
                Task<int> totPlays = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfPlay);
                Task<int> totLess6Plays = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfPlayUnder6);


                await Task.WhenAll(
                    totUser,
                    totrem,
                    totads,
                    totCancel,
                    totCam,
                    totEmail,
                    //totFile,
                    //totline,
                    totSMS,
                    totPlays,
                    //up2Aud, 
                    totLess6Plays,
                    totCost
                    );
                var currency = await _curDAL.GetCurrencyUsingCurrencyIdAsync(search.currency);

                model.NumOfTotalUser = totUser.Result;
                model.NumOfRemovedUser = totrem.Result;
                model.NumberOfAdsProvisioned = totads.Result;
                //// model.NumOfUpdateToAudit = up2Aud.Result;
                model.NumOfCancel = totCancel.Result;
                model.NumOfLiveCampaign = totCam.Result;
                model.NumOfEmail = totEmail.Result;
                //model.NumOfTextFile = totFile.Result;
                //model.NumOfTextLine = totline.Result;
                model.NumOfSMS = totSMS.Result;
                model.NumOfPlay = totPlays.Result;
                model.NumOfPlayUnder6secs = totLess6Plays.Result;
                model.AveragePlaysPerUser = totUser.Result == 0 ? 0 : (double)totPlays.Result / (double)totUser.Result;
                model.TotalCredit = (int)totCost.Result.TotalCredit;
                model.TotalSpend = (int)totCost.Result.TotalSpend;
                model.CurrencyCode = GetCurrencySymbol(currency.CurrencyCode);

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


        public async Task<XLWorkbook> GenerateExcelReport(ManagementReportsSearch search)
        {
            
            var wb = new XLWorkbook();

            List<ManagementReportModel> mappingResult = new List<ManagementReportModel>();
            ManagementReportModel model = new ManagementReportModel();

            model = await GetManReports(search);
            mappingResult.Add(model);

            // Get defaults or real to show in report
            search = SetDefaults(search);

            string[] operatorArray = _reportDAL.GetOperatorNames(search).Result.ToList().ToArray();
            //string[] operatorArray = ops.ToArray();
            string operatorName = string.Join(", ", operatorArray);

            string fromDate = "", toDate = "";
            fromDate = search.DateFrom.ToString();
            toDate = search.DateTo.ToString();


            var ws = wb.Worksheets.Add("Management Report");
            ws.Style.Font.FontSize = 9;
            ws.Range("A1" + ":" + "N1").Merge().Value = "Management Report Data";
            ws.Range("A1" + ":" + "N1").Style.Font.FontSize = 14;
            ws.Range("A1" + ":" + "N1").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A1" + ":" + "N1").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A1" + ":" + "N1").Style.Font.Bold = true;
            ws.Columns("A:M").Width = 25;

            ws.Range("A2" + ":" + "B2").Merge().Value = "Operator";
            ws.Range("A2" + ":" + "B2").Style.Font.FontSize = 12;
            ws.Range("A2" + ":" + "B2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A2" + ":" + "B2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A2" + ":" + "B2").Style.Font.Bold = true;
            ws.Columns("A:B").Width = 10;

            ws.Range("C2" + ":" + "D2").Merge().Value = operatorName.ToString();
            ws.Range("C2" + ":" + "D2").Style.Font.FontSize = 10;
            ws.Range("C2" + ":" + "D2").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C2" + ":" + "D2").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Columns("C:D").Width = 10;

            ws.Range("A3" + ":" + "B3").Merge().Value = "Date";
            ws.Range("A3" + ":" + "B3").Style.Font.FontSize = 12;
            ws.Range("A3" + ":" + "B3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A3" + ":" + "B3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A3" + ":" + "B3").Style.Font.Bold = true;
            ws.Columns("A:B").Width = 10;

            ws.Range("C3" + ":" + "D3").Merge().Value = fromDate + " - " + toDate;
            ws.Range("C3" + ":" + "D3").Style.Font.FontSize = 10;
            ws.Range("C3" + ":" + "C3").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C3" + ":" + "C3").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Columns("C:D").Width = 10;

            ws.Range("A4" + ":" + "A5").Merge().Value = "Total Users";
            ws.Range("A4" + ":" + "A5").Style.Font.FontSize = 12;
            ws.Range("A4" + ":" + "A5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("A4" + ":" + "A5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("A4" + ":" + "A5").Style.Font.Bold = true;
            ws.Column("A").Width = 15;

            ws.Range("B4" + ":" + "B5").Merge().Value = "Removed Users";
            ws.Range("B4" + ":" + "B5").Style.Font.FontSize = 12;
            ws.Range("B4" + ":" + "B5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("B4" + ":" + "B5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("B4" + ":" + "B5").Style.Font.Bold = true;
            ws.Column("B").Width = 18;

            ws.Range("C4" + ":" + "C5").Merge().Value = "Plays";
            ws.Range("C4" + ":" + "C5").Style.Font.FontSize = 12;
            ws.Range("C4" + ":" + "C5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("C4" + ":" + "C5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("C4" + ":" + "C5").Style.Font.Bold = true;
            ws.Column("C").Width = 10;

            ws.Range("D4" + ":" + "D5").Merge().Value = "Plays (Under 6sec)";
            ws.Range("D4" + ":" + "D5").Style.Font.FontSize = 12;
            ws.Range("D4" + ":" + "D5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("D4" + ":" + "D5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("D4" + ":" + "D5").Style.Font.Bold = true;
            ws.Column("D").Width = 20;

            ws.Range("E4" + ":" + "E5").Merge().Value = "SMS";
            ws.Range("E4" + ":" + "E5").Style.Font.FontSize = 12;
            ws.Range("E4" + ":" + "E5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("E4" + ":" + "E5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("E4" + ":" + "E5").Style.Font.Bold = true;
            ws.Column("E").Width = 10;

            ws.Range("F4" + ":" + "F5").Merge().Value = "Email";
            ws.Range("F4" + ":" + "F5").Style.Font.FontSize = 12;
            ws.Range("F4" + ":" + "F5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("F4" + ":" + "F5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("F4" + ":" + "F5").Style.Font.Bold = true;
            ws.Column("F").Width = 10;

            ws.Range("G4" + ":" + "G5").Merge().Value = "Live Campaign";
            ws.Range("G4" + ":" + "G5").Style.Font.FontSize = 12;
            ws.Range("G4" + ":" + "G5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("G4" + ":" + "G5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("G4" + ":" + "G5").Style.Font.Bold = true;
            ws.Column("G").Width = 18;

            ws.Range("H4" + ":" + "H5").Merge().Value = "Ads provisioned";
            ws.Range("H4" + ":" + "H5").Style.Font.FontSize = 12;
            ws.Range("H4" + ":" + "H5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("H4" + ":" + "H5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("H4" + ":" + "H5").Style.Font.Bold = true;
            ws.Column("H").Width = 20;

            ws.Range("I4" + ":" + "I5").Merge().Value = "Total Spend (in GBP)";
            ws.Range("I4" + ":" + "I5").Style.Font.FontSize = 12;
            ws.Range("I4" + ":" + "I5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("I4" + ":" + "I5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("I4" + ":" + "I5").Style.Font.Bold = true;
            ws.Column("I").Width = 25;

            ws.Range("J4" + ":" + "J5").Merge().Value = "Total Credit (in GBP)";
            ws.Range("J4" + ":" + "J5").Style.Font.FontSize = 12;
            ws.Range("J4" + ":" + "J5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("J4" + ":" + "J5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("J4" + ":" + "J5").Style.Font.Bold = true;
            ws.Column("J").Width = 25;

            ws.Range("K4" + ":" + "K5").Merge().Value = "Total Cancel";
            ws.Range("K4" + ":" + "K5").Style.Font.FontSize = 12;
            ws.Range("K4" + ":" + "K5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("K4" + ":" + "K5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("K4" + ":" + "K5").Style.Font.Bold = true;
            ws.Column("K").Width = 15;

            ws.Range("L4" + ":" + "L5").Merge().Value = "Average Plays Per User";
            ws.Range("L4" + ":" + "L5").Style.Font.FontSize = 12;
            ws.Range("L4" + ":" + "L5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("L4" + ":" + "L5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("L4" + ":" + "L5").Style.Font.Bold = true;
            ws.Column("L").Width = 25;

            ws.Range("M4" + ":" + "M5").Merge().Value = "Text Files Processed";
            ws.Range("M4" + ":" + "M5").Style.Font.FontSize = 12;
            ws.Range("M4" + ":" + "M5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("M4" + ":" + "M5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("M4" + ":" + "M5").Style.Font.Bold = true;
            ws.Column("M").Width = 25;

            ws.Range("N4" + ":" + "N5").Merge().Value = "Text Lines Processed";
            ws.Range("N4" + ":" + "N5").Style.Font.FontSize = 12;
            ws.Range("N4" + ":" + "N5").Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            ws.Range("N4" + ":" + "N5").Style.Alignment.SetVertical(XLAlignmentVerticalValues.Center);
            ws.Range("N4" + ":" + "N5").Style.Font.Bold = true;
            ws.Column("N").Width = 25;



            int first = 5;
            int last = first;
            int excelrowno = first;
            if (mappingResult.Count() > 0)
            {
                for (int i = 0; i < mappingResult.Count(); i++)
                {
                    excelrowno += 1;
                    int j = excelrowno;

                    ws.Cell("A" + j.ToString()).Value = mappingResult[i].NumOfTotalUser.ToString();
                    ws.Cell("B" + j.ToString()).Value = mappingResult[i].NumOfRemovedUser.ToString();
                    ws.Cell("C" + j.ToString()).Value = mappingResult[i].NumOfPlay.ToString();
                    ws.Cell("D" + j.ToString()).Value = mappingResult[i].NumOfPlayUnder6secs.ToString();
                    ws.Cell("E" + j.ToString()).Value = mappingResult[i].NumOfSMS.ToString();
                    ws.Cell("F" + j.ToString()).Value = mappingResult[i].NumOfEmail.ToString();
                    ws.Cell("G" + j.ToString()).Value = mappingResult[i].NumOfLiveCampaign.ToString();
                    ws.Cell("H" + j.ToString()).Value = mappingResult[i].NumberOfAdsProvisioned.ToString();
                    ws.Cell("I" + j.ToString()).Value = mappingResult[i].TotalSpend.ToString("N");
                    ws.Cell("J" + j.ToString()).Value = mappingResult[i].TotalCredit.ToString("N");
                    ws.Cell("K" + j.ToString()).Value = mappingResult[i].NumOfCancel.ToString();
                    ws.Cell("L" + j.ToString()).Value = mappingResult[i].AveragePlaysPerUser.ToString();
                    ws.Cell("M" + j.ToString()).Value = mappingResult[i].NumOfTextFile.ToString();
                    ws.Cell("N" + j.ToString()).Value = mappingResult[i].NumOfUpdateToAudit.ToString();
                }
            }

            return wb;
        }


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
                case "XOF": return "(XOF)";
                case "EUR": return "€";
                case "KES": return "(KES)";
                default: return "£";
            }
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

    }
}
