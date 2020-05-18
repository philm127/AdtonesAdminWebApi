using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAreaDAL
    {
        Task<IEnumerable<AreaResult>> LoadAreaResultSet(string command);
        Task<int> AddArea(string command, AreaResult areamodel);
        Task<AreaResult> GetAreaById(string command, int id);
        Task<int> DeleteAreaById(string command, int id);
        Task<int> UpdateArea(string command, AreaResult model);
    }
}
