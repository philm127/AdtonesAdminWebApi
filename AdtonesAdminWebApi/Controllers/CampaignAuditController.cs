using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.BusinessServices.ManagementReports;
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
        //private readonly ICreateExelManagementReport _excelService;

        public CampaignAuditController(ICampaignAuditService auditService, IManagementReportService manService)//, ICreateExelManagementReport excelService)
        {
            _auditService = auditService;
            _manService = manService;
            //_excelService = excelService;
        }


        [HttpPut("v1/GetManagementReport")]
        public async Task<ReturnResult> GetManagementReport(ManagementReportsSearch search)
        {
            return await _manService.GetReportData(search);
        }


        //[HttpPut("v1/GenerateManReport")]
        //public async Task<IActionResult> GenerateManReport(ManagementReportsSearch search)
        //{
        //    string fileName = "Management_Report.xlsx";// + DateTime.Now.ToString("yyyy -MM-dd HH':'mm':'ss") + ".xlsx";
        //    byte[] filebyte = await _excelService.GenerateExcelManagementReport(search);
        //    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        //}


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


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/GetCampDashboardSummarySalesManager")]
        public async Task<ReturnResult> GetCampDashboardSummarySalesManager()
        {
            return await _auditService.GetDashboardSummaryForSalesManager();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("v1/GetCampDashboardSummarySalesExec/{id}")]
        public async Task<ReturnResult> GetCampDashboardSummarySalesExec(int id = 0)
        {
            return await _auditService.GetDashboardSummaryForSalesExec(id);
        }

    }
}