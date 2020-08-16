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

        public RewardsService(IRewardDAL rewardDAL)

        {
            _rewardDAL = rewardDAL;
        }


        public async Task<ReturnResult> LoadRewardsDataTable()
        {
            
            try
            {

                result.body = await _rewardDAL.LoadRewardResultSet();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardsService",
                    ProcedureName = "LoadRewardsDataTable"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardsService",
                    ProcedureName = "GetOperatorConfig"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardsService",
                    ProcedureName = "AddReward"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardsService",
                    ProcedureName = "UpdateReward"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardService",
                    ProcedureName = "DeleteReward"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


    }
}