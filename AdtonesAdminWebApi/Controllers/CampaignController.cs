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


        [HttpGet("v1/GetAdvertListForSales/{id}")]
        public async Task<ReturnResult> GetAdvertListForSales(int id = 0)
        {
            return await _advertService.LoadAdvertDataTableForSales(id);
        }


        /// <summary>
        /// The optional id refers to the selection made from advertisers screen
        /// so later will use status to only return unapproved.
        /// Otherwise all adverts returned
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertListById/{id}")]
        public async Task<ReturnResult> GetAdvertListById(int id = 0)
        {
            return await _advertService.LoadAdvertDataTableById(id);
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/DeleteAdvertCategory")]
        public async Task<ReturnResult> DeleteAdvertCategory(IdCollectionViewModel model)
        {
            return await _advertService.DeleteAdvertCategory(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/UpdateAdvertCategory")]
        public async Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            return await _advertService.UpdateAdvertCategory(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpGet("v1/GetAdvertCategoryDetails/{id}")]
        public async Task<ReturnResult> GetAdvertCategoryDetails(int id)
        {
            return await _advertService.GetAdvertCategoryDetails(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPost("v1/AddAdvertCategory")]
        public async Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model)
        {
            return await _advertService.AddAdvertCategory(model);
        }


        #endregion


        #region Campaign


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDataTable/{id}")]
        public async Task<ReturnResult> GetCampaignDataTable(int id=0)
        {
            return await _campService.LoadCampaignDataTable(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDataTableForSalesExec/{id}")]
        public async Task<ReturnResult> GetCampaignDataTableForSalesExec(int id = 0)
        {
            return await _campService.LoadCampaignDataTableSalesExec(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDataTableByCampId/{id}")]
        public async Task<ReturnResult> GetCampaignDataTableByCampId(int id)
        {
            return await _campService.LoadCampaignDataTableById(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/UpdateCampaignStatus")]
        public async Task<ReturnResult> UpdateCampaignStatus(IdCollectionViewModel model)
        {
            return await _campService.UpdateCampaignStatus(model);
        }


        #endregion


    }
}
