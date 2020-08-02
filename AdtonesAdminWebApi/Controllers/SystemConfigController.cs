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
    public class SystemConfigController : ControllerBase
    {
        private ISystemConfigService _configService;
        private IRewardsService _rewardService;

        public SystemConfigController(ISystemConfigService configService, IRewardsService rewardService)
        {
            _configService = configService;
            _rewardService = rewardService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains List SystemConfigResult</returns>
        [HttpGet("v1/LoadSystemConfigurationDataTable")]
        public async Task<ReturnResult> LoadSystemConfigurationDataTable()
        {
            return await _configService.LoadSystemConfigurationDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains SystemConfigResult</returns>
        [HttpGet("v1/GetSystemConfig/{id}")]
        public async Task<ReturnResult> GetSystemConfig(int id)
        {
            return await _configService.GetSystemConfig(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains SystemConfigResult</returns>
        [HttpGet("v1/GetConfigTypeList")]
        public async Task<ReturnResult> GetConfigTypeList()
        {
            return _configService.GetSystemConfigType();
        }


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpPost("v1/AddSystemConfig")]
        public async Task<ReturnResult> AddSystemConfig(SystemConfigResult model)
        {
            return await _configService.AddSystemConfig(model);
        }


        [HttpDelete("v1/DeleteSystemConfig/{id}")]
        public async Task<ReturnResult> DeleteSystemConfig(int id)
        {
            return await _configService.DeleteSystemConfig(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateSystemConfig")]
        public async Task<ReturnResult> UpdateSystemConfig(SystemConfigResult model)
        {
            return await _configService.UpdateSystemConfig(model);
        }


        #region Rewards



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
        [HttpGet("v1/GetReward/{id}")]
        public async Task<ReturnResult> GetReward(int id)
        {
            return await _rewardService.GetReward(id);
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

        [HttpDelete("v1/DeleteReward/{id}")]
        public async Task<ReturnResult> DeleteReward(int id)
        {
            return await _rewardService.DeleteReward(id);
        }

        #endregion
    }
}