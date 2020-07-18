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
        private readonly IPromotionalCampaignService _promoService;

        public CampaignController(IAdvertService advertService, ICampaignService campService,
            IPromotionalCampaignService promoService)
        {
            _advertService = advertService;
            _campService = campService;
            _promoService = promoService;
        }

        #region Advert

        /// <summary>
        /// The optional id refers to the selection made from advertisers screen
        /// so later will use status to only return unapproved.
        /// Otherwise all adverts returned
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertList/{id}")]
        public async Task<ReturnResult> GetAdvertList(int id=0)
        {
            return await _advertService.LoadAdvertDataTable(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertDetail/{id}")]
        public async Task<ReturnResult> GetAdvertDetail(int id)
        {
            return await _advertService.LoadAdvertDetails(id);
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/UpdateAdvertStatus")]
        public async Task<ReturnResult> UpdateAdvertStatus(UserAdvertResult model)
        {
            return await _advertService.ApproveORRejectAdvert(model);
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
        [HttpGet("v1/GetCampaignDataTable/{id}")]
        public async Task<ReturnResult> GetCampaignDataTable(int id=0)
        {
            return await _campService.LoadCampaignDataTable(id);
        }


        #endregion


    }
}
