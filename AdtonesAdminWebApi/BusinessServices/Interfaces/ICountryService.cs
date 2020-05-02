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
        Task<ReturnResult> AddCountry(CountryFormModel countrymodel);
        Task<ReturnResult> UpdateCountry(CountryFormModel countrymodel);
        Task<ReturnResult>GetCountry(IdCollectionViewModel countrymodel);
    }
}
