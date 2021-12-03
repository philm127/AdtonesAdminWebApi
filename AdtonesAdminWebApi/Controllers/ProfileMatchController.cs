using System;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class ProfileMatchController : ControllerBase
    {
        private readonly IProfileMatchInfoService _profileService;
        private readonly ILoggingService _logServ;
        private readonly IProfileMatchInfoDAL _profDAL;
        private readonly IUserProfileService _userService;
        ReturnResult result = new ReturnResult();
        const string PageName = "ProfileMatchController";


        public ProfileMatchController(ILoggingService logServ, IProfileMatchInfoDAL profDAL, IProfileMatchInfoService profileService,
                                    IUserProfileService userService)
        {
            _profileService = profileService;
            _logServ = logServ;
            _profDAL = profDAL;
            _userService = userService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List ProfileMatchInformationFormModel</returns>
        [HttpGet("v1/GetProfileMatchData")]
        public async Task<ReturnResult> GetProfileMatchData()
        {
            try
            {

                result.body = await _profDAL.LoadProfileResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "FillProfileMatchInformationResult";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains ProfileMatchInformationFormModel</returns>
        [HttpGet("v1/GetProfileMatchInfoById/{id}")]
        public async Task<ReturnResult> GetProfileMatchInfoById(int id)
        {
            return await _profileService.GetProfileInfo(id);
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
        [HttpDelete("v1/DeleteProfileMatchLabel/{id}")]
        public async Task<ReturnResult> DeleteProfileMatchLabel(int id)
        {
            try
            {
                var x = await _profDAL.DeleteProfileLabelById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "DeleteProfileLabel";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
            return result;
        }


        [HttpGet("v1/GetProfileTypeList")]
        public ReturnResult GetProfileTypeList()
        {
            return  _profileService.FillProfileType();
        }


        [HttpPost("v1/GetUserProfilePreference")]
        public async Task<ReturnResult> GetUserProfilePreference(IdCollectionViewModel model)
        {
            return await _userService.GetUserProfilePreference(model.userId,model.id);
        }

    }
}