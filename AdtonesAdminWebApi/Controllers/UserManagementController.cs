using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Model;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AdtonesAdminWebApi.Controllers
{
    // [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;
        private readonly IPromotionalUsersService _promotionalService;

        public UserManagementController(IUserManagementService userService,IPromotionalUsersService promotionalService)
        {
            _userService = userService;
            _promotionalService = promotionalService;
        }


        [Route("v1/GetUserResultTest")]
        public async Task<IEnumerable<AdvertiserUserResult>> GetUserResultTest()
        {
            var res = await _userService.LoadDataTable();
            IEnumerable<AdvertiserUserResult> nv;
            nv = (IEnumerable<AdvertiserUserResult>)res.body;
            return nv;

        }


        [HttpGet("v1/GetUserResult")]
        public async Task<ReturnResult> GetUserResult()
        {
            return await _userService.LoadDataTable();
        }


        [Route("v1/GetCompanyDetails")]
        public async Task<ReturnResult> GetCompanyDetails([FromBodyAttribute]User users)
        {
            return await _userService.GetCompanyForm(users.UserId);
        }


        [Route("v1/UpdateCompanyDetails")]
        public async Task<ReturnResult> UpdateCompanyDetails([FromBodyAttribute]CompanyDetails company)
        {
            return await _userService.UpdateCompanyDetails(company);
        }


        [Route("v1/GetContactDetails")]
        public async Task<ReturnResult> GetContactDetails([FromBodyAttribute]User users)
        {
            return await _userService.GetContactForm(users.UserId);
        }


        [Route("v1/UpdateContactDetails")]
        public async Task<ReturnResult> UpdateContactDetails([FromBodyAttribute]Contacts contact)
        {
            return await _userService.UpdateContactForm(contact);
        }


        [Route("v1/GetUserProfile")]
        public async Task<ReturnResult> GetUserProfile([FromBodyAttribute]User users)
        {
            return await _userService.GetProfileForm(users.UserId);
        }


        [Route("v1/UpdateUserProfile")]
        public async Task<ReturnResult> UpdateUserProfile([FromBodyAttribute]User users)
        {
            return await _userService.UpdateProfileForm(users);
        }


        [Route("v1/ApproveORSuspendUser")]
        public async Task<ReturnResult> ApproveORSuspendUser([FromBodyAttribute] AdvertiserUserResult users)
        {
            return await _userService.ApproveORSuspendUser(users);
        }


        [Route("v1/UploadPromotionalUser")]
        public async Task<ReturnResult> UploadPromotionalUser([FromBodyAttribute] PromotionalUserFormModel model)
        {
            return await _promotionalService.SavePromotionalUser(model);
        }

    }
}

