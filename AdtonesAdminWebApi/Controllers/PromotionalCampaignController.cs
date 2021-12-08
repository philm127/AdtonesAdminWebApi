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
    public class PromotionalCampaignController : ControllerBase
    {
        private readonly IPromotionalCampaignService _campService;

        public PromotionalCampaignController(IPromotionalCampaignService campService)
        {
            _campService = campService;
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body success or error message</returns>
        [HttpPost("v1/AddPromoCampaign")]
        public async Task<ReturnResult> AddPromoCampaign([FromForm] PromotionalCampaignResult model)
        {
            return await _campService.AddPromotionalCampaign(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body success or error message</returns>
        [HttpPost("v1/AddPromoUser")]
        public async Task<ReturnResult> AddPromoUser([FromForm] PromotionalUserFormModel model)
        {
            return await _campService.SavePromotionalUser(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body success or error message</returns>
        [HttpPut("v1/UpdatePromoCampaignStatus")]
        public async Task<ReturnResult> UpdatePromoCampaignStatus(IdCollectionViewModel model)
        {
            return await _campService.UpdatePromotionalCampaignStatus(model);
        }


        /// <summary>
        /// Gets an available list of BatchId's to be used in adding a new Promo Batch
        /// takes in operator id to search provisioning DB
        /// </summary>
        /// <returns>body contains SharedSelectList of BatchId</returns>
        [HttpGet("v1/GetAvailableBatchId/{id}")]
        public async Task<ReturnResult> GetAvailableBatchId(int id)
        {
            return await _campService.GetPromoBatchIdList(id);
        }
    }
}
