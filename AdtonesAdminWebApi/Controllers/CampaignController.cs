using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly IAdvertCategoryService _advertCategoryService;
        private readonly ICampaignService _campService;
        private readonly IPromotionalCampaignService _promoService;

        public CampaignController(IAdvertService advertService, ICampaignService campService,
            IPromotionalCampaignService promoService, IAdvertCategoryService advertCategoryService)
        {
            _advertService = advertService;
            _campService = campService;
            _promoService = promoService;
            _advertCategoryService = advertCategoryService;
        }


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


        [HttpGet("v1/GetCampaignDataTableForAdvertiser{id}")]
        public async Task<ReturnResult> GetCampaignDataTableForAdvertiser(int id)
        {
            return await _campService.LoadCampaignDataTableAdvertiser(id);
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPost("v1/AddCampaignCategory")]
        public async Task<ReturnResult> AddCampaignCategory(CampaignCategoryResult model)
        {
            return await _campService.AddCampaignCategory(model);
        }


        #endregion


    }
}
