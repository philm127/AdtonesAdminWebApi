using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileMatchController : ControllerBase
    {
        private readonly IProfileMatchInfoService _profileService;


        public ProfileMatchController(IProfileMatchInfoService profileService)
        {
            _profileService = profileService;

        }


        [Route("v1/GetProfileMatchData")]
        public async Task<ReturnResult> GetProfileMatchData()
        {
            return await _profileService.LoadDataTable();
        }


        [Route("v1/GetProfileMatchInfoById")]
        public async Task<ReturnResult> GetProfileMatchInfoById(IdCollectionViewModel model)
        {
            return await _profileService.GetProfileInfo(model);
        }


        [Route("v1/UpdateProfileMatchInfo")]
        public async Task<ReturnResult> UpdateProfileMatchInfo(ProfileMatchInformationFormModel model)
        {
            return await _profileService.UpdateProfileInfo(model);
        }


        [Route("v1/AddProfileMatchInfo")]
        public async Task<ReturnResult> AddProfileMatchInfo(ProfileMatchInformationFormModel model)
        {
            return await _profileService.AddProfileInfo(model);
        }


        [Route("v1/DeleteProfileMatchLabel")]
        public async Task<ReturnResult> DeleteProfileMatchLabel(IdCollectionViewModel model)
        {
            return await _profileService.DeleteProfileLabel(model);
        }


        [Route("v1/GetProfileTypeList")]
        public ReturnResult GetProfileTypeList()
        {
            return  _profileService.FillProfileType();
        }

    }
}