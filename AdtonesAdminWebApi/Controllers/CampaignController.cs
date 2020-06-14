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
        private readonly ICampaignAuditService _auditService;

        public CampaignController(IAdvertService advertService, ICampaignService campService, ICampaignAuditService auditService)
        {
            _advertService = advertService;
            _campService = campService;
            _auditService = auditService;
        }

        #region Advert

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDashboardSummariesOperators/{id}")]
        public async Task<ReturnResult> GetCampaignDashboardSummariesOperators(int id = 0)
        {
            return await _auditService.GetCampaignDashboardSummariesOperators(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpPut("v1/GetPlayDetailsForOperatorByCampaign")]
        public async Task<ReturnResult> GetPlayDetailsForOperatorByCampaign(PagingSearchClass paging)//queryParameters)
        {
            return await _auditService.GetPlayDetailsForOperatorByCampaign(paging);
        }



        


    }
}
