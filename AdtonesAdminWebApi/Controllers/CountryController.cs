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
    [Authorize]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly ICountryAreaService _countryService;
        private readonly ILoggingService _logServ;
        private readonly ICountryAreaDAL _caDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "CountryController";


        public CountryController(ICountryAreaService countryService, ICountryAreaDAL caDAL, ILoggingService logServ)
        {
            _countryService = countryService;
            _logServ = logServ;
            _caDAL = caDAL;
        }


        #region Country


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CountryResult</returns>
        [HttpGet("v1/GetCountryData")]
        public async Task<ReturnResult> GetCountryData()
        {
            try
            {
                result.body = await _caDAL.LoadCountryResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadDataTable";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains CountryResult</returns>
        [HttpGet("v1/GetCountry/{id}")]
        public async Task<ReturnResult> GetCountry(int id)
        {
            try
            {
                result.body = await _caDAL.GetCountryById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCountry";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="countrymodel"></param>
        /// <returns>body contains number 1 on success</returns>
        [HttpPost("v1/AddCountry")]
        public async Task<ReturnResult> AddCountry([FromForm] CountryResult countrymodel)
        {
            return await _countryService.AddCountry(countrymodel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="countrymodel"></param>
        /// <returns>body contains number 1 on success</returns>
        [HttpPut("v1/UpdateCountry")]
        public async Task<ReturnResult> UpdateCountry([FromForm]CountryResult countrymodel)
        {
            return await _countryService.UpdateCountry(countrymodel);
        }


        #endregion


        #region Areas


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List AreaResult</returns>
        [HttpGet("v1/GetAreaData")]
        public async Task<ReturnResult> GetAreaData()
        {
            try
            {
                result.body = await _caDAL.LoadAreaResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillAreaResult";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains AreaResult</returns>
        [HttpGet("v1/GetAreaById/{id}")]
        public async Task<ReturnResult> GetAreaById(int id)
        {
            try
            {
                result.body = await _caDAL.GetAreaById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetArea";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body is empty</returns>
        [HttpPost("v1/AddArea")]
        public async Task<ReturnResult> AddArea([FromBody]AreaResult model)
        {
            return await _countryService.AddArea(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body is empty</returns>
        [HttpPut("v1/UpdateArea")]
        public async Task<ReturnResult> UpdateArea([FromBody]AreaResult model)
        {
            try
            {
                var cnt = await _caDAL.UpdateArea(model);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateArea";
                await _logServ.LogError();

                result.result = 0;
                result.error = model.AreaName + " Record was not updated.";
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body is empty</returns>
        [HttpDelete("v1/DeleteAreaById/{id}")]
        public async Task<ReturnResult> DeleteAreaById(int id)
        {
            try
            {
                var x = await _caDAL.DeleteAreaById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteArea";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        #endregion



        /// <summary>
        /// Gets Minimum Bid value for a campaign
        /// </summary>
        /// <param name="id">countryId</param>
        /// <returns>body contains decimal value</returns>
        [HttpGet("v1/GetMinimumBid/{id}")]
        public async Task<ReturnResult> GetMinimumBid(int id)
        {
            var countryId = id;
            try
            {
                result.body = await _caDAL.GetMinBidByCountry(countryId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetMinBid";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }

    }
}