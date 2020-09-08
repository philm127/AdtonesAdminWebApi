using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
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


        [HttpPut("v1/GetManagementReport")]
        public async Task<ReturnResult> GetManagementReport(ManagementReportsSearch search)
        {
            return await _manService.GetReportData(search);
        }


        [HttpPut("v1/GenerateManReport")]
        public async Task<IActionResult> GenerateManReport(ManagementReportsSearch search)
        {
            string fileName = "Management_Report.xlsx";// + DateTime.Now.ToString("yyyy -MM-dd HH':'mm':'ss") + ".xlsx";
            var wb = await _manService.GenerateExcelReport(search);
            byte[] filebyte;
            try
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    filebyte = memoryStream.ToArray();
                    //string s = Convert.ToBase64String(filebyte);
                    //var FileName = ContentDispositionHeaderValue.Parse(fileName.Trim('"'));

                    //var result = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    Content = new ByteArrayContent(memoryStream.ToArray())
                    //};
                    //result.Content.Headers.ContentDisposition =
                    //    new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    //    {
                    //        FileName = fileName
                    //    };
                    //result.Content.Headers.ContentType =
                    //    new MediaTypeHeaderValue("application/vnd.ms-excel");

                    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "CampaignAuditController",
                    ProcedureName = "GenerateManReport"
                };
                _logging.LogError();
                return null;
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