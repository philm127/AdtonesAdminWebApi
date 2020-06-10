using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAreaDAL
    {
        Task<IEnumerable<AreaResult>> LoadAreaResultSet();
        Task<int> AddArea(AreaResult areamodel);
        Task<AreaResult> GetAreaById(int id);
        Task<int> DeleteAreaById(int id);
        Task<int> UpdateArea(AreaResult model);
    }
}
