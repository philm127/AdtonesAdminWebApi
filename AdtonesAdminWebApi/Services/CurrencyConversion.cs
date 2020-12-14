using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Services
{

    public class CurrencyModel
    {
        public string Code { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }

        public decimal Convert(decimal value)
        {
            return value * Amount;
        }
    }

    public class CurrencyListing
    {
        public string CurrencyCode { get; set; }
        public decimal CurrencyRate { get; set; }
    }


    public interface ICurrencyConversion
    {
        decimal Convert(decimal value, string currencyFrom, string currencyTo);
        decimal GetCurrencyRateModel(string from, string to);
        string GetCurrencySymbol(string currencyCode);
    }

    public class CurrencyConversion : ICurrencyConversion
    {
        private readonly string url;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly Dictionary<string, decimal> _cachedRates = new Dictionary<string, decimal>();
        private readonly IConfiguration _configuration;
       public static int Userjwt { get; private set; }

        private readonly ICurrencyDAL _repository;

        public CurrencyConversion(IConfiguration configuration, IHttpContextAccessor httpAccessor)
        {
            _configuration = configuration;
            _httpAccessor = httpAccessor;
            url = _configuration.GetValue<string>("AppSettings:CurrencyUrl"); 
             Userjwt = _httpAccessor.GetUserIdFromJWT();
        }


        //public CurrencyConversionX(ICurrencyDAL repository)
        //{
        //    _repository = repository;
        //}


        public Model.Currency DisplayCurrency { get; private set; }


        public decimal GetCurrencyRateModel(string from, string to)
        {
            return Convert(1, from, to);
        }

        public string GetCurrencySymbol(string currencyCode)
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


        //public async Task InitializeAsync(int userId)
        //{
        //    DisplayCurrency = await _repository.GetDisplayCurrencyCodeForUserAsync(userId);
        //}


        //public void Initialize(int userId)
        //{
        //    DisplayCurrency = _repository.GetDisplayCurrencyCodeForUserAsync(userId).Result;
        //}

        //public static CurrencyConversionX CreateForCurrentUser(ICurrencyDAL currencyRepository)
        //{
        //    CurrencyConversionX conversion = new CurrencyConversionX(currencyRepository);
        //    conversion.Initialize(Userjwt);
        //    return conversion;
        //}

        //public static async Task<CurrencyConversion> CreateForCurrentUserAsync(object controller)
        //{
        //    CurrencyConversion conversion = new CurrencyConversion();
        //    await conversion.InitializeAsync(Userjwt);
        //    return conversion;
        //}


        //[Obsolete]
        //public CurrencyModel ForeignCurrencyConversion(string amount, string currencyFrom, string currencyTo)
        //{
        //    decimal dAmount;
        //    if (!decimal.TryParse(amount, out dAmount))
        //        return new CurrencyModel { Amount = 0.00M, Code = "FAIL", Message = "Failed to parse amount" };
        //    return new CurrencyModel { Amount = Convert(dAmount, currencyFrom, currencyTo), Message = string.Empty, Code = "OK" };
        //}

        public decimal Convert(decimal value, string currencyFrom, string currencyTo)
        {
            decimal rate;
            string key = $"{currencyFrom}~{currencyTo}";
            if (!_cachedRates.TryGetValue(key, out rate))
            {
                rate = CallForRate(currencyFrom, currencyTo);
                _cachedRates[key] = rate;
            }

            return Math.Round(rate * value, 4, MidpointRounding.AwayFromZero);
        }

        public decimal ConvertToDisplayCurrency(decimal value, string currencyFrom)
        {
            return Convert(value, currencyFrom, DisplayCurrency.CurrencyCode);
        }

        public decimal ConvertFromDisplayCurrency(decimal value, string currencyTo)
        {
            return Convert(value, DisplayCurrency.CurrencyCode, currencyTo);
        }

        private decimal CallForRate(string currencyFrom, string currencyTo)
        {
            var config = _configuration;
            //var url2 = _configuration.GetValue<string>("AppSettings:CurrencyUrl");
            try
            {
                
                var param = new Currency { Amount = 1M, From = currencyFrom, To = currencyTo };
                var client = new RestClient("http://217.160.184.168:5555/api/currency/convert");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(param);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return JsonConvert.DeserializeObject<decimal>(response.Content);
                return InvalidRate;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = "Failed to get Currency rate. Error: {ex.Message.ToString()}",
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "Service - CurrencyConversion",
                    ProcedureName = "CallForRate"
                };
                _logging.LogError();
                return InvalidRate;
            }
        }

        private const decimal InvalidRate = 0M;

        private class Currency
        {
            public decimal Amount { get; set; }
            public string From { get; set; }
            public string To { get; set; }
        }
    }

    //public static class CurrencyConversionExtensions
    //{
    //    public static decimal Convert(this decimal value, CurrencyConversionX conversion, string from, string to)
    //    {
    //        return conversion.Convert(value, from, to);
    //    }

    //    public static decimal ConvertToDisplay(this decimal value, CurrencyConversionX conversion, string from)
    //    {
    //        return conversion.ConvertToDisplayCurrency(value, from);
    //    }

    //    public static decimal ConvertFromDisplay(this decimal value, CurrencyConversionX conversion, string to)
    //    {
    //        return conversion.ConvertFromDisplayCurrency(value, to);
    //    }
    //}
}