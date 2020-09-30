using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class SalesManagementService : ISalesManagementService
    {
        private readonly ISalesManagementDAL _salesDAL;
        ReturnResult result = new ReturnResult();

        public SalesManagementService(ISalesManagementDAL salesDAL)
        {
            _salesDAL = salesDAL;
        }


        public async Task<ReturnResult> GetAllocatedAdvertisers(int userId = 0)
        {
            try
            {
                result.body = await GetAllocatedAdvertisersPrivate(userId);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SalesManagerService",
                    ProcedureName = "GetAllocatedAdvertisers"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        private async Task<IEnumerable<AllocationList>> GetAllocatedAdvertisersPrivate(int userId = 0)
        {
            try
            {
                return await _salesDAL.GetAllocationLists(userId);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SalesManagerService",
                    ProcedureName = "GetAllocatedAdvertisersPrivate"
                };
                _logging.LogError();

                var error = new List<AllocationList>();
                return error;
            }
        }


        public async Task<ReturnResult> GetDDSalesExec()
        {
            try
            {
                result.body = await _salesDAL.GetsalesExecDDList();

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SalesManagerService",
                    ProcedureName = "GetDDSalesExec"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateSalesExecAllocation(SalesAdAllocationModel model)
        {
            bool success = true;
            try
            {
                if (model.user1 != 0)
                    success = await SalesExecAllocationProcess(model.user1, model.user1array);

                if (model.user2 != 0)
                    success = await SalesExecAllocationProcess(model.user2, model.user2array);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SalesManagerService",
                    ProcedureName = "GetDDSalesExec"
                };
                _logging.LogError();
                result.result = 0;
            }
            if(success)
                return result;
            else
            {
                result.result = 0;
                return result;
            }
        }


        private async Task<bool> SalesExecAllocationProcess(int userid, List<AllocationList> listing)
        {
            try
            {
                var IEnum1 = await GetAllocatedAdvertisersPrivate(userid);
                var toLst1 = IEnum1.ToList();

                // List of items that should be made inactive in the table for this sales person
                var firstNotSecond1 = toLst1.Except(listing).ToList();

                // List of items that will be added to this sales person and made inactive against anyone else
                var SecondNotFirst1 = listing.Except(toLst1).ToList();

                foreach (var item1 in firstNotSecond1)
                {
                    var x1 = await _salesDAL.UpdateInactiveForSP(userid, item1.UserId);
                }

                foreach (var add1 in SecondNotFirst1)
                {
                    bool exists1 = await _salesDAL.CheckIfAdvertiserExists(add1.UserId);
                    if (!exists1)
                    {
                        int z1 = await _salesDAL.InsertToSalesAd(userid, add1.UserId);
                    }
                    else
                    {
                        var y1 = await _salesDAL.UpdateUserForSP(userid, add1.UserId);
                    }
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SalesManagerService",
                    ProcedureName = "SalesExecAllocationProcess"
                };
                _logging.LogError();
                return false;
            }
            return true;
        }

    }
}
