using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CreateUpdateCampaignController : ControllerBase
    {

        private readonly ICreateUpdateCampaignService _campService;

        public CreateUpdateCampaignController(ICreateUpdateCampaignService campService)
        {
            _campService = campService;
        }


        #region Create


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CountryResult</returns>
        //[HttpGet("v1/GetInitialData/{id}")]
        //public async Task<ReturnResult> GetInitialData(int id)
        //{
        //    return await _countryService.GetInitialData(id);
        //}





        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains number 1 on success</returns>
        [HttpPost("v1/CreateCampaignAdvert")]
        public async Task<ReturnResult> CreateCampaignAdvert([FromForm] NewAdvertFormModel model)
        {
            return await _campService.CreateNewCampaign_Advert(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains number 1 on success</returns>
        [HttpPost("v1/CreateCampaignClient")]
        public async Task<ReturnResult> CreateCampaignClient([FromForm] NewAdvertFormModel model)
        {
            // return await _countryService.AddCountry(countrymodel);
            var tst = model;
            var tst2 = tst;
            var model2 = new ReturnResult();

            return model2;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains number 1 on success</returns>
        [HttpPost("v1/CreateNewCampaign")]
        public async Task<ReturnResult> CreateNewCampaign(NewCampaignProfileFormModel model)
        {
            return await _campService.CreateNewCampaign(model);
        }

        #endregion

    }
}