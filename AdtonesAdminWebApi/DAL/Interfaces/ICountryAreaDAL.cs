using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ICountryAreaDAL
    {
        Task<IEnumerable<CountryResult>> LoadCountryResultSet();
        Task<CountryResult> GetCountryById(int id);
        Task<bool> CheckCountryExists(CountryResult model);
        Task<int> AddCountry(CountryResult model);
        Task<int> UpdateCountry(CountryResult model);
        Task<IEnumerable<AreaResult>> LoadAreaResultSet();
        Task<int> AddArea(AreaResult areamodel);
        Task<AreaResult> GetAreaById(int id);
        Task<int> DeleteAreaById(int id);
        Task<int> UpdateArea(AreaResult model);
        Task<bool> CheckAreaExists(AreaResult areamodel);
    }
}
