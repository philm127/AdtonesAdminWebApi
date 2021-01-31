using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Services
{
    public class CurrencySymbol
    {
        public string GetCurrencySymbolByCurrencyCode(string currencyCode)
        {
            if (string.IsNullOrWhiteSpace(currencyCode))
                return CurrencyDefaults.DefaultCurrencySymbol;

            switch (currencyCode)
            {
                case "GBP": return "£";
                case "USD": return "$";
                case "XOF": return "(XOF)";
                case "EUR": return "€";
                case "KES": return "(KES)";
                default: return CurrencyDefaults.DefaultCurrencySymbol;
            }
        }


        public static class CurrencyDefaults
        {
            public const string DefaultCurrencySymbol = "$";
            public const string DefaultCurrencyCode = "USD";
        }
    }
}
