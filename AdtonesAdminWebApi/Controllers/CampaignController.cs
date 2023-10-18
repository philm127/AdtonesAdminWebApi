using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using System;
using DocumentFormat.OpenXml.EMMA;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class CampaignController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly ICampaignService _campService;
        private readonly IPromotionalCampaignService _promoService;
        private readonly ILoggingService _logServ;
        private readonly ICampaignDAL _campDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "CampaignController";

        public CampaignController(IAdvertService advertService, ICampaignService campService, ILoggingService logServ,
            IPromotionalCampaignService promoService, ICampaignDAL campDAL)
        {
            _advertService = advertService;
            _campService = campService;
            _promoService = promoService;
            _logServ = logServ;
            _campDAL = campDAL;
        }


        #region Campaign


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetAdminOpAdminCampaignDataTable")]
        public async Task<ReturnResult> GetAdminOpAdminCampaignDataTable()
        {
            try
            {
                return await _campService.GetAdminOpAdminCampaignList();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadCampaignDataTable";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDataTableForSalesExec/{id}")]
        public async Task<ReturnResult> GetCampaignDataTableForSalesExec(int id = 0)
        {
            try
            {
                return await _campService.GetSalesCampaignList(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadCampaignDataTableSalesExec";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetCampaignDataTableForAdvertiser/{id}")]
        public async Task<ReturnResult> GetCampaignDataTableForAdvertiser(int id)
        {
            try
            {
                result.body = await _campDAL.GetCampaignResultSetByAdvertiser(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadCampaignDataTableAdvertiser";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignAdminResult</returns>
        [HttpGet("v1/GetCampaignDataTableByCampId/{id}")]
        public async Task<ReturnResult> GetCampaignDataTableByCampId(int id)
        {
            try
            {
                return await _campService.GetCampaignById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCampaignDataTableByCampId";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
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
            try
            {
                result.body = await _campDAL.InsertCampaignCategory(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddAdvertCategory";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        #endregion


    }
}
