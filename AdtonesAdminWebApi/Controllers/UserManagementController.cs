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
        private readonly IUserDashboardService _dashboardService;

        public UserManagementController(IUserManagementService userService,IPromotionalUsersService promotionalService,
                                            IUserDashboardService dashboardService)
        {
            _userService = userService;
            _promotionalService = promotionalService;
            _dashboardService = dashboardService;
        }


        [HttpGet("v1/GetUserResultTest")]
        public async Task<IEnumerable<AdvertiserDashboardResult>> GetUserResultTest()
        {
            var res = await _dashboardService.LoadAdvertiserDataTable();
            IEnumerable<AdvertiserDashboardResult> nv;
            nv = (IEnumerable<AdvertiserDashboardResult>)res.body;
            return nv;

        }


        [HttpGet("v1/GetUserResult")]
        public async Task<ReturnResult> GetAdvertiserTable()
        {
            return await _dashboardService.LoadAdvertiserDataTable();
        }


        [HttpGet("v1/GetCompanyDetails")]
        public async Task<ReturnResult> GetCompanyDetails([FromBodyAttribute]User users)
        {
            return await _userService.GetCompanyForm(users.UserId);
        }


        [HttpPut("v1/UpdateCompanyDetails")]
        public async Task<ReturnResult> UpdateCompanyDetails([FromBodyAttribute]CompanyDetails company)
        {
            return await _userService.UpdateCompanyDetails(company);
        }


        [HttpGet("v1/GetContactDetails")]
        public async Task<ReturnResult> GetContactDetails([FromBodyAttribute]User users)
        {
            return await _userService.GetContactForm(users.UserId);
        }


        [HttpPut("v1/UpdateContactDetails")]
        public async Task<ReturnResult> UpdateContactDetails([FromBodyAttribute]Contacts contact)
        {
            return await _userService.UpdateContactForm(contact);
        }


        [HttpGet("v1/GetUserProfile")]
        public async Task<ReturnResult> GetUserProfile([FromBodyAttribute]User users)
        {
            return await _userService.GetProfileForm(users.UserId);
        }


        [HttpPut("v1/UpdateUserProfile")]
        public async Task<ReturnResult> UpdateUserProfile([FromBodyAttribute]User users)
        {
            return await _userService.UpdateProfileForm(users);
        }


        [HttpPost("v1/AddOperatorUser")]
        public async Task<ReturnResult> AddOperatorUser([FromBodyAttribute] OperatorAdminFormModel model)
        {
            return await _userService.AddOperatorAdminUser(model);
        }


        [HttpPut("v1/ApproveORSuspendUser")]
        public async Task<ReturnResult> ApproveORSuspendUser([FromBodyAttribute] AdvertiserDashboardResult users)
        {
            return await _userService.ApproveORSuspendUser(users);
        }


        [HttpPost("v1/UploadPromotionalUser")]
        public async Task<ReturnResult> UploadPromotionalUser([FromBodyAttribute] PromotionalUserFormModel model)
        {
            return await _promotionalService.SavePromotionalUser(model);
        }


    }
}

