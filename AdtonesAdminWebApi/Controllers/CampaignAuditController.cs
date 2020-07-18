using System.IO;
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
    public class CampaignAuditController : ControllerBase
    {
        private readonly ICampaignAuditService _auditService;
        private readonly IManagementReportService _manService;

        public CampaignAuditController(ICampaignAuditService auditService, IManagementReportService manService)
        {
            _auditService = auditService;
            _manService = manService;
        }


        [HttpGet("v1/GetManagementReport")]
        public async Task<ReturnResult> GetManagementReport(ManagementReportsSearch search)
        {
            return await _manService.GetReportData(search);
        }


        [HttpGet("v1/GenerateManReport")]
        public async Task<IActionResult> GenerateManReport(ManagementReportsSearch search)
        {
            string fileName = "Management Report.csv";
            using (MemoryStream MyMemoryStream = new MemoryStream())
            {
                var wb = await _manService.GenerateExcelReport(search);
                wb.SaveAs(MyMemoryStream);
                //Return xlsx/csv Excel File  
                return File(MyMemoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetDashboardSummaryByOperator/{id}")]
        public async Task<ReturnResult> GetDashboardSummaryByOperator(int id = 0)
        {
            return await _auditService.GetDashboardSummariesByOperator(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetDashboardSummaryByCampaign/{id}")]
        public async Task<ReturnResult> GetDashboardSummaryByCampaign(int id = 0)
        {
            return await _auditService.GetCampaignDashboardSummariesOperatorByCampaign(id);
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
        [HttpPut("v1/GetPlayDetailsForOperatorByCampaign")]
        public async Task<ReturnResult> GetPlayDetailsForOperatorByCampaign(PagingSearchClass paging)
        {
            return await _auditService.GetPlayDetailsForOperatorByCampaign(paging);
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