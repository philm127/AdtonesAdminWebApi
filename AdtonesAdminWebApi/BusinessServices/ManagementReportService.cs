using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    
    public class ManagementReportService : IManagementReportService
    {
        private readonly IManagementReportDAL _reportDAL;
        //private readonly CurrencyConversion _currencyConversion;
        private readonly ICurrencyDAL _currencyRepository;
        ReturnResult result = new ReturnResult();


        public ManagementReportService(ICurrencyDAL currencyRepository, IManagementReportDAL reportDAL)
        {
            _reportDAL = reportDAL;
            //_currencyConversion = CurrencyConversion.CreateForCurrentUser(_currencyRepository);
            _currencyRepository = currencyRepository;
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
                var ops = new[] { 1, 2 };
                search.operators = ops.Concat(new[] { 3 }).ToArray();
            }

            return search;
        }


        public async Task<ReturnResult> GetNumOfTotalUser(ManagementReportsSearch search)
        {
            search = SetDefaults(search);

            ManagementReportModel model = new ManagementReportModel();


            try
            {
                Task<int> totUser = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfTotalUser);
                Task<int> totrem = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfRemovedUser);
                Task<int> totads = _reportDAL.GetreportInts(search, ManagementReportQuery.NumberOfAdsProvisioned);
                Task<int> up2Aud = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfUpdateToAudit);
                
                Task<IEnumerable<SpendCredit>> totCost = _reportDAL.GetTotalCreditCost(search, ManagementReportQuery.GetTotalCost);
                
                Task<int> totCancel = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfCancel);
                Task<int> totCam = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfLiveCampaign);
                Task<int> totEmail = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfEmail);
                Task<int> totFile = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfTextFile);
                Task<int> totline = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfTextLine);
                Task<int> totSMS = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfSMS);
                Task<int> totPlays = _reportDAL.GetreportInts(search, ManagementReportQuery.NumOfPlay);

                await Task.WhenAll(totUser, totrem, totads, up2Aud, totCancel, totCam, totEmail, totFile, totline, totSMS, totPlays,
                                    totCost);

                model.NumOfTotalUser = totUser.Result;
                model.NumOfRemovedUser = totrem.Result;
                model.NumberOfAdsProvisioned = totads.Result;
                model.NumOfUpdateToAudit = up2Aud.Result;
                model.NumOfCancel = totCancel.Result;
                model.NumOfLiveCampaign = totCam.Result;
                model.NumOfEmail = totEmail.Result;
                model.NumOfTextFile = totFile.Result;
                model.NumOfTextLine = totline.Result;
                model.NumOfSMS = totSMS.Result;
                model.NumOfPlay = totPlays.Result;

                var costAudit = totCost.Result.ToList();

                var calcModel = CalculateSearchData(costAudit);

                model.TotalCredit = calcModel.TotalCredit;
                model.TotalSpend = calcModel.TotalSpend;

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

        public class CurrencyListing
        {
            public string CurrencyCode { get; set; }
            public decimal CurrencyRate { get; set; }
        }


        private decimal GetCurrencyRateModel(string from, string to)
        {
            CurrencyConversion curCon = new CurrencyConversion(this._currencyRepository);
            return curCon.Convert(1, from, to);
        }


        private List<CurrencyListing> GetConvertedCurrency(List<SpendCredit> creditList)
        {
            string toCurrencyCode = "GBP";
            var currency = creditList.Select(y => y.CurrencyCode).Distinct().ToList();
            if (!currency.Contains("GBP"))
                currency.Add("GBP");

            var clList = new List<CurrencyListing>();
            foreach(string cur in currency)
            {
                var cl = new CurrencyListing();

                if (cur == "GBP")
                {
                    cl.CurrencyCode = "GBP";
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
            return clList;
        }


        private TotalCostCredit CalculateSearchData(List<SpendCredit> creditList)
        {
            TotalCostCredit campaignAudit = new TotalCostCredit();

            if (creditList.Count > 0)
            {
                var currencyConv = GetConvertedCurrency(creditList);

                foreach (var campaignAuditItem in creditList)
                {
                        var currencyRate = currencyConv.Where(kv => kv.CurrencyCode.Contains(campaignAuditItem.CurrencyCode)).Select(kv => kv.CurrencyRate).FirstOrDefault();
                        if (currencyRate == 0)
                            currencyRate = 1;

                        campaignAudit.TotalSpend = campaignAudit.TotalSpend + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCost) * currencyRate));
                        campaignAudit.TotalCredit = campaignAudit.TotalCredit + (Convert.ToDouble(Convert.ToDecimal(campaignAuditItem.TotalCredit) * currencyRate));
                }
            }
            
            return campaignAudit;
        }

    }
}
