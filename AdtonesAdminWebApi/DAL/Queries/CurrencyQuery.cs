using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Queries
{
    public interface ICurrencyQuery
    {
        string GetCurrencyByCountry { get; }
        string GetCurrencyByCurrency { get; }
    }
    
    public class CurrencyQuery : ICurrencyQuery
    {

        public string GetCurrencyByCountry => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CountryId=@CountryId";


        public string GetCurrencyByCurrency => "SELECT CurrencyId, CurrencyCode,CountryId FROM Currencies WHERE CurrencyCode=@CurrencyCode";
    }
}
