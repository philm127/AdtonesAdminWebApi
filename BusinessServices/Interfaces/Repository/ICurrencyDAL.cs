using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface ICurrencyDAL
    {
        
        Task<Currency> GetDisplayCurrencyCodeForUserAsync(int userId);
        Task<Currency> GetCurrencyUsingCountryIdAsync(int? countryId);
        Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currencyId);
    }
}
