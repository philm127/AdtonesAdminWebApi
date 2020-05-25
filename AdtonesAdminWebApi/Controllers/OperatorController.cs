using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorConfigService _operatorConfigService;
        private readonly IOperatorService _operatorService;
        

        public OperatorController(IOperatorConfigService operatorConfigService, IOperatorService operatorService)
        {
            _operatorConfigService = operatorConfigService;
            _operatorService = operatorService;
        }


        #region Operator Config


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorConfigurationResult</returns>
        [HttpGet("v1/LoadOperatorConfigurationDataTable")]
        public async Task<ReturnResult> LoadOperatorConfigurationDataTable()
        {
            return await _operatorConfigService.LoadOperatorConfigurationDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorConfigurationResult</returns>
        [HttpGet("v1/GetOperatorConfig")]
        public async Task<ReturnResult> GetOperatorConfig(IdCollectionViewModel model)
        {
            return await _operatorConfigService.GetOperatorConfig(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains nothing</returns>
        [HttpPost("v1/AddOperatorConfig")]
        public async Task<ReturnResult> AddOperatorConfig(OperatorConfigurationResult model)
        {
            return await _operatorConfigService.AddOperatorConfig(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateOperatorConfig")]
        public async Task<ReturnResult> UpdateOperatorConfig(OperatorConfigurationResult model)
        {
            return await _operatorConfigService.UpdateOperatorConfig(model);
        }


        #endregion

        #region Operator Service

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorResultModel</returns>
        [HttpGet("v1/GetOperators")]
        public async Task<ReturnResult> GetOperators()
        {
            return await _operatorService.LoadOperatorDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPost("v1/AddOperator")]
        public async Task<ReturnResult> AddOperator(OperatorFormModel operatormodel)
        {
            return await _operatorService.AddOperator(operatormodel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorFormModel</returns>
        [HttpGet("v1/GetOperator")]
        public async Task<ReturnResult> GetOperator(IdCollectionViewModel model)
        {
            return await _operatorService.GetOperator(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPut("v1/UpdateOperator")]
        public async Task<ReturnResult> UpdateOperator(OperatorFormModel operatormodel)
        {
            return await _operatorService.UpdateOperator(operatormodel);
        }


        #region Operator Max Adverts

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorMaxAdvertsFormModel</returns>
        [HttpGet("v1/LoadOperatorMaxAdvertDataTable")]
        public async Task<ReturnResult> LoadOperatorMaxAdvertDataTable()
        {
            return await _operatorService.LoadOperatorMaxAdvertDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPost("v1/AddOperatorMaxAdverts")]
        public async Task<ReturnResult> AddOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatormodel)
        {
            return await _operatorService.AddOperatorMaxAdverts(operatormodel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorMaxAdvertsFormModel</returns>
        [HttpGet("v1/GetOperatorMaxAdvert")]
        public async Task<ReturnResult> GetOperatorMaxAdvert(IdCollectionViewModel model)
        {
            return await _operatorService.GetOperatorMaxAdvert(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPut("v1/UpdateOperatorMaxAdverts")]
        public async Task<ReturnResult> UpdateOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatormodel)
        {
            return await _operatorService.UpdateOperatorMaxAdverts(operatormodel);
        }


        #endregion


        #endregion

    }
}