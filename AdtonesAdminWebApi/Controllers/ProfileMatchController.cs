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
    public class ProfileMatchController : ControllerBase
    {
        private readonly IProfileMatchInfoService _profileService;


        public ProfileMatchController(IProfileMatchInfoService profileService)
        {
            _profileService = profileService;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List ProfileMatchInformationFormModel</returns>
        [HttpGet("v1/GetProfileMatchData")]
        public async Task<ReturnResult> GetProfileMatchData()
        {
            return await _profileService.LoadDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains ProfileMatchInformationFormModel</returns>
        [HttpGet("v1/GetProfileMatchInfoById")]
        public async Task<ReturnResult> GetProfileMatchInfoById(IdCollectionViewModel model)
        {
            return await _profileService.GetProfileInfo(model.id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateProfileMatchInfo")]
        public async Task<ReturnResult> UpdateProfileMatchInfo(ProfileMatchInformationFormModel model)
        {
            return await _profileService.UpdateProfileInfo(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>bosy contains nothing</returns>
        [HttpPost("v1/AddProfileMatchInfo")]
        public async Task<ReturnResult> AddProfileMatchInfo(ProfileMatchInformationFormModel model)
        {
            return await _profileService.AddProfileInfo(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpDelete("v1/DeleteProfileMatchLabel")]
        public async Task<ReturnResult> DeleteProfileMatchLabel(IdCollectionViewModel model)
        {
            return await _profileService.DeleteProfileLabel(model.id);
        }


        [HttpGet("v1/GetProfileTypeList")]
        public ReturnResult GetProfileTypeList()
        {
            return  _profileService.FillProfileType();
        }

    }
}