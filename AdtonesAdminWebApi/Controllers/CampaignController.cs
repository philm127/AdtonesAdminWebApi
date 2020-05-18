using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly IAdvertService _advertService;

        public CampaignController(IAdvertService advertService)
        {
            _advertService = advertService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertList")]
        public async Task<ReturnResult> GetAdvertList()
        {
            return await _advertService.LoadAdvertDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertDetail")]
        public async Task<ReturnResult> GetAdvertDetail(IdCollectionViewModel model)
        {
            return await _advertService.LoadAdvertDataTable(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains AdvertCategoryResult</returns>
        [HttpGet("v1/GetAdvertCategoryList")]
        public async Task<ReturnResult> GetAdvertCategoryList()
        {
            return await _advertService.LoadAdvertCategoryDataTable();
        }

    }
}
