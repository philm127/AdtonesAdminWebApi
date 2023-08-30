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


        [HttpPost("v1/CheckIfCampaignNameExists")]
        public async Task<ReturnResult> CheckIfCampaignNameExists(NewCampaignProfileFormModel campaignName)
        {
            return await _campService.CheckIfCampaignNameExists(campaignName);
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


        [HttpPost("v1/UpdateCampaignDetails")]
        public async Task<ReturnResult> UpdateCampaignDetails(NewCampaignProfileFormModel model)
        {
            return await _campService.UpdateCampaignDetails(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">countryid</param>
        /// <returns>Empty profile models</returns>
        [HttpGet("v1/GetInitialData/{id}")]
        public async Task<ReturnResult> GetInitialData(int id)
        {
            return await _campService.GetInitialData(id);
        }


        /// <summary>
        /// Gets populated profile models
        /// </summary>
        /// <param name="id">id is CampaignProfileId</param>
        /// <returns>Populated profile models</returns>
        [HttpGet("v1/GetProfileData/{id}")]
        public async Task<ReturnResult> GetProfileData(int id)
        {
            return await _campService.GetProfileData(id);
        }


        [HttpPost("v1/InsertProfileInformation")]
        public async Task<ReturnResult> InsertProfileInformation(NewAdProfileMappingFormModel model)
        {
            return await _campService.InsertProfileInformation(model);
        }

        #endregion


        [HttpGet("v1/GetCampaignData/{id}")]
        public async Task<ReturnResult> GetCampaignData(int id)
        {
            return await _campService.GetCampaignData(id);
        }

    }
}