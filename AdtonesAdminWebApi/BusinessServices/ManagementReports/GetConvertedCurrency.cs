using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports
{
    public interface IGetConvertedCurrency
    {
        List<CurrencyListing> BuildConvertConverter(List<SpendCredit> creditList, ManagementReportsSearch search);
        Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId);
        string GetCurrencySymbol(string currencyId);
    }


    public class GetConvertedCurrency : IGetConvertedCurrency
    {
        private readonly ICurrencyDAL _curDAL;
        private readonly ICurrencyConversion _curConv;
        private readonly ILoggingService _logServ;

        public GetConvertedCurrency(ICurrencyDAL curDAL, ICurrencyConversion curConv, ILoggingService logServ)
        {
            _curDAL = curDAL;
            _curConv = curConv;
            _logServ = logServ;
        }

        public List<CurrencyListing> BuildConvertConverter(List<SpendCredit> creditList, ManagementReportsSearch search)
        {
            string toCurrencyCode = "GBP";
            var clList = new List<CurrencyListing>();
            try
            {
                var currency = creditList.Select(y => y.CurrencyCode).Distinct().ToList();
                var currencySelectionData = _curDAL.GetCurrencyUsingCurrencyIdAsync(search.currency).Result;
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

                        cl.CurrencyRate = _curConv.GetCurrencyRateModel(cur, toCurrencyCode);
                        clList.Add(cl);
                    }
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "GetConvertedCurrency";
                _logServ.ProcedureName = "BuildConvertConverter";
                _logServ.LogError();
            }
            return clList;
        }


        public async Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId)
        {
            return await _curDAL.GetCurrencyUsingCurrencyIdAsync(currencyId);
        }


        public string GetCurrencySymbol(string currencyId)
        {
            return _curConv.GetCurrencySymbol(currencyId);
        }
    }
}