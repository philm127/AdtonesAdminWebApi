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
        private readonly ICampaignAuditService _auditService;

        public PromotionalCampaignController(IPromotionalCampaignService campService, ICampaignAuditService auditService)
        {
            _campService = campService;
            _auditService = auditService;
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
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetPromoDashboardSummary/{id}")]
        public async Task<ReturnResult> GetPromoDashboardSummary(int id = 0)
        {
            return await _auditService.GetPromoCampaignDashboardSummary(id);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpPut("v1/GetPromoPlayDetailsByCampaign")]
        public async Task<ReturnResult> GetPromoPlayDetailsByCampaign(PagingSearchClass paging)
        {
            return await _auditService.GetPromoPlayDetails(paging);
        }

    }
}
