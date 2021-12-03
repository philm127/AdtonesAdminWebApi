using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories.Queries
{
    public static class CurrencyQuery
    {

        public static string GetCurrencyByCountry => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CountryId=@CountryId";


        public static string GetCurrencyByCurrency => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CurrencyCode=@CurrencyCode";


        public static string GetCurrencyByUserIdContact => @"SELECT con.CurrencyId, cur.CurrencyCode,con.CountryId FROM Contacts AS con 
                                                            INNER JOIN Currencies AS cur ON cur.CountryId=con.CountryId
                                                            WHERE con.UserId = @userId ";


        public static string GetCurrencyByCurrencyId => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CurrencyId=@currencyId";
    }
}
