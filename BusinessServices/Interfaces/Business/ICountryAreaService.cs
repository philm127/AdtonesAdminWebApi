using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ICountryAreaService
    {
        Task<ReturnResult> LoadDataTable();
        Task<ReturnResult> AddCountry(CountryResult countrymodel);
        Task<ReturnResult> UpdateCountry(CountryResult countrymodel);
        Task<ReturnResult>GetCountry(int Id);


        Task<ReturnResult> LoadAreaDataTable();
        Task<ReturnResult> AddArea(AreaResult areamodel);
        Task<ReturnResult> GetArea(int id);
        Task<ReturnResult> UpdateArea(AreaResult areamodel);
        Task<ReturnResult> DeleteArea(int id);

        Task<ReturnResult> GetMinBid(int countryId);
    }
}
