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