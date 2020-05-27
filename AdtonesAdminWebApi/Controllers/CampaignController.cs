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
        private readonly ICampaignService _campService;

        public CampaignController(IAdvertService advertService, ICampaignService campService)
        {
            _advertService = advertService;
            _campService = campService;
        }

        #region Advert

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
        /// <returns>body contains List AdvertCategoryResult</returns>
        [HttpGet("v1/GetAdvertCategoryList")]
        public async Task<ReturnResult> GetAdvertCategoryList()
        {
            return await _advertService.LoadAdvertCategoryDataTable();
        }


        #endregion


        #region Campaign


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignCreditResult</returns>
        [HttpGet("v1/GetCampaignCreditDataTable")]
        public async Task<ReturnResult> GetCampaignCreditDataTable()
        {
            return await _campService.LoadCampaignCreditDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDataTable")]
        public async Task<ReturnResult> GetCampaignDataTable()
        {
            return await _campService.LoadCampaignDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List PromotionalCampaignResult</returns>
        [HttpGet("v1/GetPromoCampaignDataTable")]
        public async Task<ReturnResult> GetPromoCampaignDataTable()
        {
            return await _campService.LoadPromoCampaignDataTable();
        }


        #endregion

    }
}
