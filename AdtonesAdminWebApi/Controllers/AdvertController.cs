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
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertService _advertService;
        private readonly IAdvertCategoryService _advertCategoryService;

        public AdvertController(IAdvertService advertService, IAdvertCategoryService advertCategoryService)
        {
            _advertService = advertService;
            _advertCategoryService = advertCategoryService;
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
            return await _advertService.GetAdvertDataTable(id);
        }


        [HttpGet("v1/GetAdvertListForSales/{id}")]
        public async Task<ReturnResult> GetAdvertListForSales(int id = 0)
        {
            return await _advertService.GetAdvertDataTableForSales(id);
        }

        [HttpGet("v1/GetAdvertListForAdvertiser/{id}")]
        public async Task<ReturnResult> GetAdvertListForAdvertiser(int id)
        {
            return await _advertService.GetAdvertDataTableForAdvertiser(id);
        }


        [HttpGet("v1/GetAdvertListForAdvertiserSummary/{id}")]
        public async Task<ReturnResult> GetAdvertListForAdvertiserSummary(int id)
        {
            return await _advertService.GetAdvertDataTableForAdvertiserSummary(id);
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
            return await _advertService.GetAdvertDataTableById(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertDetail/{id}")]
        public async Task<ReturnResult> GetAdvertDetail(int id)
        {
            return await _advertService.GetAdvertDetails(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List UserAdvertResult</returns>
        [HttpGet("v1/GetAdvertDetailByCampaignId/{id}")]
        public async Task<ReturnResult> GetAdvertDetailByCamapaignId(int id)
        {
            return await _advertService.GetAdvertDetailsByCampaignId(id);
        }

        [HttpPost("v1/CheckIfAdvertNameExists")]
        public async Task<ReturnResult> CheckIfAdvertNameExists(NewAdvertFormModel advertName)
        {
            return await _advertService.CheckIfAdvertNameExists(advertName);
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
            return await _advertCategoryService.GetAdvertCategoryDataTable();
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
            return await _advertCategoryService.DeleteAdvertCategory(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/UpdateAdvertCategory")]
        public async Task<ReturnResult> UpdateAdvertCategory(AdvertCategoryResult model)
        {
            return await _advertCategoryService.UpdateAdvertCategory(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpGet("v1/GetAdvertCategoryDetails/{id}")]
        public async Task<ReturnResult> GetAdvertCategoryDetails(int id)
        {
            return await _advertCategoryService.GetAdvertCategoryDetails(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPost("v1/AddAdvertCategory")]
        public async Task<ReturnResult> AddAdvertCategory(AdvertCategoryResult model)
        {
            return await _advertCategoryService.AddAdvertCategory(model);
        }


        #endregion

    }
}
