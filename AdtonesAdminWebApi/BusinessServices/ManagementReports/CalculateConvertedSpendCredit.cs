using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports
{
    public interface ICalculateConvertedSpendCredit
    {
        TotalCostCredit Calculate(List<SpendCredit> creditList, ManagementReportsSearch search);
        Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId);
        string GetCurrencySymbol(string currencyId);
    }


    public class CalculateConvertedSpendCredit : ICalculateConvertedSpendCredit
    {
        private readonly IGetConvertedCurrency _convertedCur;
        private readonly ILoggingService _logServ;

        public CalculateConvertedSpendCredit(IGetConvertedCurrency convertedCur, ILoggingService logServ)
        {
            _convertedCur = convertedCur;
            _logServ = logServ;
        }

        public TotalCostCredit Calculate(List<SpendCredit> creditList, ManagementReportsSearch search)
        {
            TotalCostCredit campaignAudit = new TotalCostCredit();
            try
            {

                if (creditList.Count > 0)
                {
                    var currencyConv = _convertedCur.BuildConvertConverter(creditList, search);

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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "CalculateConvertedSpendCredit";
                _logServ.ProcedureName = "Calculate";
                _logServ.LogError();

            }

            return campaignAudit;
        }

        public async Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId)
        {
            return await _convertedCur.GetCurrencyUsingCurrencyIdAsync(currencyId);
        }


        public string GetCurrencySymbol(string currencyId)
        {
            return _convertedCur.GetCurrencySymbol(currencyId);
        }

    }
}
