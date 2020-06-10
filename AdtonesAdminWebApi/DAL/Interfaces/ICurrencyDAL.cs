using AdtonesAdminWebApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICurrencyDAL
    {
        
        Task<Currency> GetDisplayCurrencyCodeForUserAsync(int userId);
        Task<Currency> GetCurrencyUsingCountryIdAsync(int? countryId);
        Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId);
    }
}
