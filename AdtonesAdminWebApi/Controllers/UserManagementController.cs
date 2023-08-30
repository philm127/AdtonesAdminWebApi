using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using System;
using Microsoft.AspNetCore.Http;

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
        private readonly IUserDashboardDAL _dashboardDal;
        private readonly IUserManagementDAL _userDAL;
        private readonly IUserManagementAddUserService _userAddService;
        private readonly ISalesManagementDAL _salesDAL;
        private readonly ILoggingService _logServ;
        IHttpContextAccessor _httpAccessor;
        ReturnResult result = new ReturnResult();
        const string PageName = "UserManagementController";

        public UserManagementController(IUserManagementService userService,IPromotionalCampaignService promotionalService, IUserDashboardDAL dashboardDal,
                                            IUserDashboardService dashboardService, ISalesManagementService salesService, 
                                            IHttpContextAccessor httpAccessor, IUserManagementDAL userDAL, ISalesManagementDAL salesDAL,
                                            IUserManagementAddUserService userAddService)
        {
            _userService = userService;
            _promotionalService = promotionalService;
            _dashboardService = dashboardService;
            _salesService = salesService;
            _dashboardDal = dashboardDal;
            _httpAccessor = httpAccessor;
            _userDAL = userDAL;
            _salesDAL = salesDAL;
            _userAddService = userAddService;
        }


        [HttpGet("v1/GetAdvertiserTable")]
        public async Task<ReturnResult> GetAdvertiserTable()
        {
            var roleName = _httpAccessor.GetRoleFromJWT();
            var operatorId = 0;

            if (roleName.ToLower() == "OperatorAdmin".ToLower())
                operatorId = _httpAccessor.GetOperatorFromJWT();

            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboard(operatorId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertiserTable";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorResult</returns>
        [HttpGet("v1/LoadOperatorDataTable")]
        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetOperatorDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOperatorDashboard";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List OperatorResult</returns>
        [HttpGet("v1/LoadAdminDataTable")]
        public async Task<ReturnResult> LoadAdminDataTable()
        {
            try
            {
                result.body = await _dashboardDal.GetAdminDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdminDashboard";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpPut("v1/GetSubscriberTable")]
        public async Task<ReturnResult> GetSubscriberTable(PagingSearchClass paging)
        {
            return await _dashboardService.LoadSubscriberDataTable(paging);
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

        [AllowAnonymous]
        [HttpPost("v1/AddAdvertiser")]
        public async Task<ReturnResult> AddAdvertiser(UserAddFormModel model)
        {
            return await _userAddService.AddUser(model);
        }

        [HttpPost("v1/AddNewUser")]
        public async Task<ReturnResult> AddNewUser(UserAddFormModel model)
        {
            return await _userAddService.AddUser(model);
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
            try
            {
                result.body = await _userDAL.GetUserById(some.userId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetUserById";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
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
            try
            {
                result.body = await _salesDAL.GetsalesExecDDList();

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetSalesExecDDList";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpPut("v1/UpdateAllocationInfo")]
        public async Task<ReturnResult> UpdateAllocationInfo(SalesAdAllocationModel model)
        {
            return await _salesService.UpdateSalesExecAllocation(model);
        }


        [HttpGet("v1/GetSalesExecDataList")]
        public async Task<ReturnResult> GetSalesExecDataList()
        {
            try
            {
                result.body = await _dashboardDal.GetSalesExecDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadSalesExecDataTable";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetSalesExecForAdminDataList")]
        public async Task<ReturnResult> GetSalesExecForAdminDataList()
        {
            try
            {
                result.body = await _dashboardDal.GetSalesExecForAdminDashboard();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadSalesExecForAdminDataTable";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetAdvertiserDataListForSales/{id}")]
        public async Task<ReturnResult> GetAdvertiserDataListForSales(int id)
        {
            try
            {
                result.body = await _dashboardDal.GetAdvertiserDashboardForSales(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertiserDataListForSales";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>body contains Client</returns>
        [HttpGet("v1/GetClientProfile/{id}")]
        public async Task<ReturnResult> GetClientProfile(int id)
        {
            try
            {
                result.body = await _userDAL.GetClientDetails(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetClientProfile";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }
    }
}

