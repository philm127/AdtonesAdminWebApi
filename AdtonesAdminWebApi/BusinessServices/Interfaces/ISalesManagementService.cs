using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ISalesManagementService
    {
        Task<ReturnResult> GetAllocatedAdvertisers(int userId = 0);

        Task<ReturnResult> UpdateSalesExecAllocation(SalesAdAllocationModel model);
        Task<IEnumerable<int>> GetAdvertiserIdsBySalesExecList(int userId);
        Task<Dictionary<int, SalesExecDetails>> GetSalesExecDictDetails();
    }
}
