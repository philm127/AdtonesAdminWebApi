using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace AdtonesAdminWebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementController : ControllerBase
    {
        private readonly IUserManagementService _userService;
        private readonly IPromotionalCampaignService _promotionalService;
        private readonly IUserDashboardService _dashboardService;
        private readonly ISalesManagementService _salesService;

        public UserManagementController(IUserManagementService userService,IPromotionalCampaignService promotionalService,
                                            IUserDashboardService dashboardService, ISalesManagementService salesService)
        {
            _userService = userService;
            _promotionalService = promotionalService;
            _dashboardService = dashboardService;
            _salesService = salesService;
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


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorResult</returns>
        [HttpGet("v1/LoadAdminDataTable")]
        public async Task<ReturnResult> LoadAdminDataTable()
        {
            return await _dashboardService.LoadAdminDataTable();
        }


        [HttpGet("v1/GetSubscriberTable")]
        public async Task<ReturnResult> GetSubscriberTable()
        {
            return await _dashboardService.LoadSubscriberDataTable();
        }


        [HttpGet("v1/GetCompanyDetails/{userId}")]
        public async Task<ReturnResult> GetCompanyDetails(int userId)
        {
            return await _userService.GetCompanyForm(userId);
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


        [HttpPost("v1/AddNewUser")]
        public async Task<ReturnResult> AddNewUser(UserAddFormModel model)
        {
            return await _userService.AddUser(model);
        }


        [HttpPut("v1/UpdateUserStatus")]
        public async Task<ReturnResult> UpdateUserStatus([FromBodyAttribute] AdvertiserDashboardResult users)
        {
            return await _userService.UpdateUserStatus(users);
        }


        [HttpPost("v1/UploadPromotionalUser")]
        public async Task<ReturnResult> UploadPromotionalUser([FromBodyAttribute] PromotionalUserFormModel model)
        {
            return await _promotionalService.SavePromotionalUser(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="some"></param>
        /// <returns>body contains User model</returns>
        [HttpGet("v1/GetUserById")]
        public async Task<ReturnResult> GetUserById(IdCollectionViewModel some)
        {
            return await _userService.GetUserById(some.userId);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="some"></param>
        /// <returns>body contains User model</returns>
        [HttpPut("v1/UpdateUserPermission")]
        public async Task<ReturnResult> UpdateUserPermission(IdCollectionViewModel some)
        {
            return await _userService.UpdateUserPermission(some);
        }

        #region Sales Management

        /// <summary>
        /// 
        /// </summary>
        /// <param name="users"></param>
        /// <returns>body contains IEnumerable<AllocationList></returns>
        [HttpGet("v1/GetAllocatedUnallocated/{id}")]
        public async Task<ReturnResult> GetAllocatedUnallocated(int id)
        {
            return await _salesService.GetAllocatedAdvertisers(id);
        }


        [HttpGet("v1/GetSalesExecDDList")]
        public async Task<ReturnResult> GetSalesExecDDList()
        {
            return await _salesService.GetDDSalesExec();
        }


        [HttpPut("v1/UpdateAllocationInfo")]
        public async Task<ReturnResult> UpdateAllocationInfo(SalesAdAllocationModel model)
        {
            return await _salesService.UpdateSalesExecAllocation(model);
        }


        [HttpGet("v1/GetSalesExecDataList")]
        public async Task<ReturnResult> GetSalesExecDataList()
        {
            return await _dashboardService.LoadSalesExecDataTable();
        }


        [HttpGet("v1/GetAdvertiserDataListForSales/{id}")]
        public async Task<ReturnResult> GetAdvertiserDataListForSales(int id)
        {
            return await _dashboardService.LoadAdvertisersForSales(id);
        }


        #endregion
    }
}

