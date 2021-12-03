using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ISalesManagementService
    {
        Task<ReturnResult> GetAllocatedAdvertisers(int userId = 0);
        Task<ReturnResult> GetDDSalesExec();

        Task<ReturnResult> UpdateSalesExecAllocation(SalesAdAllocationModel model);
    }
}
