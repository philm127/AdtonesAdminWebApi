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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OperatorController : ControllerBase
    {
        private readonly ILoggingService _logServ;
        private readonly IOperatorDAL _opDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "OperatorController";


        public OperatorController(ILoggingService logServ, IOperatorDAL opDAL)
        {
            _logServ = logServ;
            _opDAL = opDAL;
        }


        #region Operator Config


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorConfigurationResult</returns>
        [HttpGet("v1/LoadOperatorConfigurationDataTable")]
        public async Task<ReturnResult> LoadOperatorConfigurationDataTable()
        {
            try
            {
                result.body = await _opDAL.LoadOperatorConfigurationDataTable();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "LoadOperatorConfigurationDataTable");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorConfigurationResult</returns>
        [HttpGet("v1/GetOperatorConfig/{id}")]
        public async Task<ReturnResult> GetOperatorConfig(int id)
        {
            try
            {
                result.body = await _opDAL.GetOperatorConfig(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetOperatorConfig");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains nothing</returns>
        [HttpPost("v1/AddOperatorConfig")]
        public async Task<ReturnResult> AddOperatorConfig(OperatorConfigurationResult model)
        {
            try
            {
                result.body = await _opDAL.AddOperatorConfig(model);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "AddOperatorConfig");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateOperatorConfig")]
        public async Task<ReturnResult> UpdateOperatorConfig(OperatorConfigurationResult model)
        {
            try
            {
                result.body = await _opDAL.UpdateOperatorConfig(model);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "UpdateOperatorConfig");

                result.result = 0;
            }
            return result;
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
            try
            {
                result.body = await _opDAL.LoadOperatorResultSet();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetOperators");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPost("v1/AddOperator")]
        public async Task<ReturnResult> AddOperator(OperatorFormModel operatormodel)
        {
            try
            {
                if (await _opDAL.CheckOperatorExists(operatormodel))
                {
                    result.error = operatormodel.OperatorName + " Record Exists.";
                    result.result = 0;
                    return result;
                }

                result.body = await _opDAL.AddOperator(operatormodel);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "AddOperator");

                result.result = 0;
            }
            result.body = "Operator " + operatormodel.OperatorName + " added successfully.";
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorFormModel</returns>
        [HttpGet("v1/GetOperator/{id}")]
        public async Task<ReturnResult> GetOperator(int id)
        {
            try
            {
                result.body = await _opDAL.GetOperatorById(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetOperator");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPut("v1/UpdateOperator")]
        public async Task<ReturnResult> UpdateOperator(OperatorFormModel operatormodel)
        {
            try
            {
                var x = await _opDAL.UpdateOperator(operatormodel);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "UpdateOperator");
                result.result = 0;
            }
            result.body = "Operator " + operatormodel.OperatorName + " updated successfully.";
            return result;
        }


        #region Operator Max Adverts

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorMaxAdvertsFormModel</returns>
        [HttpGet("v1/LoadOperatorMaxAdvertDataTable")]
        public async Task<ReturnResult> LoadOperatorMaxAdvertDataTable()
        {
            try
            {
                result.body = await _opDAL.LoadOperatorMaxAdvertResultSet();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "LoadOperatorMaxAdvertDataTable");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPost("v1/AddOperatorMaxAdverts")]
        public async Task<ReturnResult> AddOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatormodel)
        {
            try
            {
                if (await _opDAL.CheckMaxAdvertExists(operatormodel))
                {
                    result.error = operatormodel.KeyName + " Record Exists.";
                    result.result = 0;
                    return result;
                }

                var x = await _opDAL.AddOperatorMaxAdvert(operatormodel);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "AddOperatorMaxAdverts");

                result.result = 0;
            }
            result.body = "Added successfully.";
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains OperatorMaxAdvertsFormModel</returns>
        [HttpGet("v1/GetOperatorMaxAdvert/{id}")]
        public async Task<ReturnResult> GetOperatorMaxAdvert(int id)
        {
            try
            {
                result.body = await _opDAL.GetOperatorMaxAdvertById(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetOperatorMaxAdvert");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains success message</returns>
        [HttpPut("v1/UpdateOperatorMaxAdverts")]
        public async Task<ReturnResult> UpdateOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatormodel)
        {
            try
            {
                var x = await _opDAL.UpdateOperatorMaxAdvert(operatormodel);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "UpdateOperatorMaxAdverts");

                result.result = 0;
            }
            result.body = "Operator " + operatormodel.OperatorName + " updated successfully.";
            return result;
        }


        #endregion


        #endregion

    }
}