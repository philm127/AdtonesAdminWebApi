using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using System;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly ILoggingService _logServ;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertCategoryDAL _adCatDAL;
        ReturnResult result = new ReturnResult();
        const string PageName = "AdvertController";

        public AdvertController(IAdvertService advertService, ILoggingService logServ,
                                IAdvertDAL advertDAL, IAdvertCategoryDAL adCatDAL)
        {
            _advertService = advertService;
            _logServ = logServ;
            _advertDAL = advertDAL;
            _adCatDAL = adCatDAL;
        }



        #region Advert

        /// <summary>
        /// The optional id refers to the selection made from advertisers screen
        /// so later will use status to only return unapproved.
        /// Otherwise all adverts returned
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertList/{id}")]
        public async Task<ReturnResult> GetAdvertList(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertResultSet(id);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDataTable";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        [HttpGet("v1/GetAdvertListForSales/{id}")]
        public async Task<ReturnResult> GetAdvertListForSales(int id = 0)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertForSalesResultSet(id);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDataTableForSales";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }

        [HttpGet("v1/GetAdvertListForAdvertiser/{id}")]
        public async Task<ReturnResult> GetAdvertListForAdvertiser(int id)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertForAdvertiserResultSet(id);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDataTableForAdvertiser";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        [HttpGet("v1/GetAdvertListForAdvertiserSummary/{id}")]
        public async Task<ReturnResult> GetAdvertListForAdvertiserSummary(int id)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertForAdvertiserDashboard(id);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDataTableForAdvertiserSummary";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
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
            try
            {
                result.body = await _advertDAL.GetAdvertResultSetById(id);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDataTableById";
                await _logServ.LogError();
                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertDetail/{id}")]
        public async Task<ReturnResult> GetAdvertDetail(int id)
        {
            try
            {
                result.body = await _advertDAL.GetAdvertDetail(id);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDetail";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertDetailByCampaignId/{id}")]
        public async Task<ReturnResult> GetAdvertDetailByCamapaignId(int id)
        {
            try
            {
                var advertId = await _advertDAL.GetAdvertIdByCampid(id);
                result.body = await _advertDAL.GetAdvertDetail(advertId);
                return result;

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertDetailsByCampaignId";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }

        [HttpPost("v1/CheckIfAdvertNameExists")]
        public async Task<ReturnResult> CheckIfAdvertNameExists(NewAdvertFormModel model)
        {
            var AdvertNameexists = await _advertDAL.CheckAdvertNameExists(model.AdvertName, model.AdvertiserId);

            if (AdvertNameexists)
            {
                result.result = 0;
                result.error = "The Advert Name already exists";
                return result;
            }
            else
                return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains number 1 on success</returns>
        [HttpPost("v1/CreateCampaignAdvert")]
        public async Task<ReturnResult> CreateCampaignAdvert([FromForm] NewAdvertFormModel model)
        {
            return await _advertService.CreateNewCampaign_Advert(model);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost("v1/UpdateAdvert")]
        public async Task<ReturnResult> UpdateAdvert([FromForm] NewAdvertFormModel model)
        {
            return await _advertService.UpdateAdvert(model);
        }


        #endregion

        #region AdvertCategory

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List AdvertCategoryResult</returns>
        [HttpGet("v1/GetAdvertCategoryList")]
        public async Task<ReturnResult> GetAdvertCategoryList()
        {
            try
            {
                result.body = await _adCatDAL.GetAdvertCategoryList();
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertCategoryDataTable";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
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
            try
            {
                result.body = await _adCatDAL.RemoveAdvertCategory(model);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteAdvertCategory";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/UpdateAdvertCategory")]
        public async Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            try
            {
                result.body = await _adCatDAL.UpdateAdvertCategory(model);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateAdvertCategory";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpGet("v1/GetAdvertCategoryDetails/{id}")]
        public async Task<ReturnResult> GetAdvertCategoryDetails(int id)
        {
            try
            {
                result.body = await _adCatDAL.GetAdvertCategoryDetails(id);
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertCategoryDetails";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPost("v1/AddAdvertCategory")]
        public async Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model)
        {
            try
            {
                result.body = await _adCatDAL.InsertAdvertCategory(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddAdvertCategory";
                await _logServ.LogError();

            }
            return result;
        }


        #endregion

    }
}
