using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICountryService
    {
        Task<ReturnResult> LoadDataTable();
        Task<ReturnResult> AddCountry(CountryResult countrymodel);
        Task<ReturnResult> UpdateCountry(CountryResult countrymodel);
        Task<ReturnResult>GetCountry(IdCollectionViewModel countrymodel);
    }
}
