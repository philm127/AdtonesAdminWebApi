using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AdtonesAdminWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RewardController : ControllerBase
    {
        private IRewardsService _rewardService;

        public RewardController(IRewardsService rewardService)
        {
            _rewardService = rewardService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains List RewardResult</returns>
        [HttpGet("v1/LoadRewardsDataTable")]
        public async Task<ReturnResult> LoadRewardsDataTable()
        {
            return await _rewardService.LoadRewardsDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains RewardResult</returns>
        [HttpGet("v1/GetReward")]
        public async Task<ReturnResult> GetReward(IdCollectionViewModel model)
        {
            return await _rewardService.GetReward(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains RewardResult</returns>
        [HttpPost("v1/AddReward")]
        public async Task<ReturnResult> AddReward(RewardResult model)
        {
            return await _rewardService.AddReward(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateReward")]
        public async Task<ReturnResult> UpdateReward(RewardResult model)
        {
            return await _rewardService.UpdateReward(model);
        }

    }
}