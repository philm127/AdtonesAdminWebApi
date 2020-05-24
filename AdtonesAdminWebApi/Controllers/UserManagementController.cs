using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Model;
using Microsoft.AspNetCore.Authorization;

namespace AdtonesAdminWebApi.Controllers
{
    [Authorize]
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


        [HttpGet("v1/GetAdvertiserTable")]
        public async Task<ReturnResult> GetAdvertiserTable()
        {
            return await _dashboardService.LoadAdvertiserDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorResult</returns>
        [HttpGet("v1/LoadOperatorDataTable")]
        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            return await _dashboardService.LoadOperatorDataTable();
        }


        [HttpGet("v1/GetSubscriberTable")]
        public async Task<ReturnResult> GetSubscriberTable()
        {
            return await _dashboardService.LoadSubscriberDataTable();
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


        [HttpGet("v1/GetContactDetails/{id}")]
        public async Task<ReturnResult> GetContactDetails(int id)
        {
            return await _userService.GetContactForm(id);
        }


        [HttpPut("v1/UpdateContactDetails")]
        public async Task<ReturnResult> UpdateContactDetails([FromBodyAttribute]Contacts contact)
        {
            return await _userService.UpdateContactForm(contact);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns>body contains User</returns>
        [HttpGet("v1/GetUserProfile/{id}")]
        public async Task<ReturnResult> GetUserProfile(int id)
        {
            return await _userService.GetProfileForm(id);
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

