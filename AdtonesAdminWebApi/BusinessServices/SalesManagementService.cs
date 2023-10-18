using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
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
        private readonly ILoggingService _logServ;
        const string PageName = "SalesManagementService";

        public SalesManagementService(ISalesManagementDAL salesDAL, ILoggingService logServ)
        {
            _salesDAL = salesDAL;
            _logServ = logServ;
        }


        public async Task<ReturnResult> GetAllocatedAdvertisers(int userId = 0)
        {
            try
            {
                result.body = await GetAllocatedAdvertisersPrivate(userId);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAllocatedAdvertisers";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAllocatedAdvertisersPrivate";
                await _logServ.LogError();
                

                var error = new List<AllocationList>();
                return error;
            }
        }

        public async Task<IEnumerable<int>> GetAdvertiserIdsBySalesExecList(int userId)
        {
            try
            {
                return await _salesDAL.GetAdvertiserIdsBySalesExec(userId);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAllocatedAdvertisers";
                await _logServ.LogError();

                result.result = 0;
            }
            return null;
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateSalesExecAllocation";
                await _logServ.LogError();
                
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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "SalesExecAllocationProcess";
                await _logServ.LogError();
                
                return false;
            }
            return true;
        }

        public async Task<Dictionary<int,SalesExecDetails>> GetSalesExecDictDetails()
        {
            var salesList = await _salesDAL.GetSalesExecDetails();
            List<SalesExecDetails> sales = salesList.ToList();
            Dictionary<int, SalesExecDetails> salesDict = sales.ToDictionary(s => s.AdvertiserId);
            return salesDict;
        }

    }
}
