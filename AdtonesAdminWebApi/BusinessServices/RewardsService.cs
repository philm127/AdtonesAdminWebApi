using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class RewardsService : IRewardsService
    {
        
        ReturnResult result = new ReturnResult();
        private readonly IRewardDAL _rewardDAL;
        private readonly ILoggingService _logServ;
        const string PageName = "RewardService";

        public RewardsService(IRewardDAL rewardDAL, ILoggingService logServ)

        {
            _rewardDAL = rewardDAL;
            _logServ = logServ;
        }


        public async Task<ReturnResult> LoadRewardsDataTable()
        {
            
            try
            {

                result.body = await _rewardDAL.LoadRewardResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadRewardsDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetReward(int id)
        {

            try
            {
                result.body = await _rewardDAL.GetRewardById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetReward";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddReward(RewardResult model)
        {

            try
            {
                var x = await _rewardDAL.AddReward(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddReward";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateReward(RewardResult model)
        {
            try
            {
                var x = await _rewardDAL.UpdateReward(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateReward";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> DeleteReward(int id)
        {
            try
            {
                var x = await _rewardDAL.DeleteRewardById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteReward";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


    }
}