using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public static class CurrencyQuery
    {

        public static string GetCurrencyByCountry => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CountryId=@CountryId";


        public static string GetCurrencyByCurrency => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CurrencyCode=@CurrencyCode";
    }
}
