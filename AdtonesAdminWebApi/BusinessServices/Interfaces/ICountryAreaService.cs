using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ICountryAreaService
    {
        Task<ReturnResult> AddCountry(CountryResult countrymodel);
        Task<ReturnResult> UpdateCountry(CountryResult countrymodel);
        Task<ReturnResult> AddArea(AreaResult areamodel);
    }
}
