using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface ISalesManagementDAL
    {
        Task<IEnumerable<AllocationList>> GetAllocationLists(int userId = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetsalesExecDDList();
        Task<bool> CheckIfAdvertiserExists(int id);
        Task<int> UpdateInactiveForSP(int sp, int ad);
        Task<int> InsertToSalesAd(int sp, int ad);
        Task<int> UpdateUserForSP(int sp, int ad);
        Task<int> InsertNewAdvertiserToSalesExec(int sp, int ad);
    }
}
