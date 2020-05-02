using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAreaService
    {
        Task<ReturnResult> LoadDataTable();
        Task<ReturnResult> AddArea(AreaResult areamodel);
        Task<ReturnResult> GetArea(int id);
        Task<ReturnResult> UpdateArea(AreaResult areamodel);
        Task<ReturnResult> DeleteArea(int id);
    }
}
