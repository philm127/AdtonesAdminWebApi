using System;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
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
        private readonly ILoggingService _logServ;
        private readonly IRewardDAL _rewardDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "SystemConfigController";


        public SystemConfigController(ISystemConfigService configService, ILoggingService logServ,
                                IRewardDAL rewardDAL)
        {
            _configService = configService;
            _rewardDAL = rewardDAL;
            _logServ = logServ;
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
        public ReturnResult GetConfigTypeList()
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains RewardResult</returns>
        [HttpGet("v1/GetReward/{id}")]
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains RewardResult</returns>
        [HttpPost("v1/AddReward")]
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateReward")]
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

        [HttpDelete("v1/DeleteReward/{id}")]
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

        #endregion
    }
}