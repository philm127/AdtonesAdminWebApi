using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
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


        public CountryController(ICountryAreaService countryService)
        {
            _countryService = countryService;
        }


        #region Country


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CountryResult</returns>
        [HttpGet("v1/GetCountryData")]
        public async Task<ReturnResult> GetCountryData()
        {
            var tst = await _countryService.LoadDataTable();
            return tst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains CountryResult</returns>
        [HttpGet("v1/GetCountry/{id}")]
        public async Task<ReturnResult> GetCountry(int id)
        {
            var tst = await _countryService.GetCountry(id);
            return tst;
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
            return await _countryService.LoadAreaDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains AreaResult</returns>
        [HttpGet("v1/GetAreaById/{id}")]
        public async Task<ReturnResult> GetAreaById(int id)
        {
            return await _countryService.GetArea(id);
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
            return await _countryService.UpdateArea(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body is empty</returns>
        [HttpDelete("v1/DeleteAreaById/{id}")]
        public async Task<ReturnResult> DeleteAreaById(int id)
        {
            return await _countryService.DeleteArea(id);
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
            return await _countryService.GetMinBid(id);
        }

    }
}