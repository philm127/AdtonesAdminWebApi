using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SharedListController : ControllerBase
    {
        private readonly ISharedSelectListsService _sharedList;

        public SharedListController(ISharedSelectListsService sharedList)
        {
            _sharedList = sharedList;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetCountryList")]
        public async Task<ActionResult<ReturnResult>> GetCountryList()
        {
            return await _sharedList.GetCountryList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetRoleList")]
        public ReturnResult GetRoleList()
        {
            return  _sharedList.GetUserRole();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetUserPermissionList")]
        public async Task<ActionResult<ReturnResult>> GetUserPermissionList()
        {
            return await _sharedList.GetUsersWPermissions();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetUserStatusList")]
        public ReturnResult GetUserStatusList()
        {
            return _sharedList.GetUserStatus();
        }


        [HttpGet("v1/GetTicketStatusList")]
        public ReturnResult GetTicketStatusList()
        {
            return _sharedList.GetTicketStatus();
        }


        [HttpGet("v1/GetTicketSubjectList")]
        public async Task<ActionResult<ReturnResult>> GetTicketSubjectList()
        {
            return await _sharedList.GetTicketSubjectList();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetClientList/{id}")]
        public async Task<ActionResult<ReturnResult>> GetClientList(int id)
        {
            return await _sharedList.GetClientList(id);
        }


        [HttpGet("v1/FillPaymentTypeDropDown")]
        public async Task<ActionResult<ReturnResult>> FillPaymentTypeDropDown()
        {
            return await _sharedList.FillPaymentTypeDropdown();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetOperatorList/{id}")]
        public async Task<ActionResult<ReturnResult>> GetOperatorList(int id = 0)
        {
            return await _sharedList.GetOperatorList(id);
        }


        /// <summary>
        /// Uses the IdCollectionViewModel to get id from a model rather than the url
        /// </summary>
        /// <param name="some">some contains currencyId among other simple Id's</param>
        /// <returns>body contains List SharedSelectListViewModel
        /// or a single one if id entered</returns>
        [HttpGet("v1/GetCurrencyList/{id}")]
        public async Task<ActionResult<ReturnResult>> GetCurrencyList(int id)
        {
            //_sharedList.CurrentUserId = int.Parse(User.FindFirst("userId")?.Value);
            // _sharedList.RoleName = User.FindFirst(ClaimTypes.Role)?.Value;
            return await _sharedList.GetCurrencyList(id);
        }


        [HttpGet("v1/FillOrganisationTypeDropDown")]
        public async Task<ActionResult<ReturnResult>> FillOrganisationTypeDropDown()
        {
            return await _sharedList.GetOrganisationTypeDropDown();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">countryId</param>
        /// <returns></returns>
        [HttpGet("v1/FillAdvertCategoryDropDown/{id}")]
        public async Task<ActionResult<ReturnResult>> FillAdvertCategoryDropDown(int id)
        {
            return await _sharedList.GetAdvertCategoryDropDown(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">countryId</param>
        /// <returns></returns>
        [HttpGet("v1/FillCampaignCategoryDropDown/{id}")]
        public async Task<ActionResult<ReturnResult>> FillCampaignCategoryDropDown(int id)
        {
            return await _sharedList.GetCampaignCategoryDropDown(id);
        }

        /// <summary>
        /// Gets all the selcted lists required Country, Currency and Users
        /// </summary>
        /// <returns>body contains 3 List SharedSelectListViewModel
        /// Country, Currency and Credit Users</returns>
        [HttpGet("v1/GetUserDetailCreditList")]
        public async Task<ActionResult<ReturnResult>> GetUserDetailCreditList()
        {
            return await _sharedList.GetUserDetailCreditList();
        }


        /// <summary>
        /// Gets the advertisers who have a credit account
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetAddUserCreditList")]
        public async Task<ActionResult<ReturnResult>> GetAddUserCreditList()
        {
            return await _sharedList.GetAddCreditUsersList();
        }


        /// <summary>
        /// Gets the advertisers who currently do not have a credit yet
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel
        /// or a single one if id entered</returns>
        [HttpGet("v1/GetUserCreditList")]
        public async Task<ActionResult<ReturnResult>> GetUserCreditList()
        {
            return await _sharedList.GetUserCreditList();
        }


        /// <summary>
        /// Gets the users who are not subscribers and has role tagged on
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel
        /// or a single one if id entered</returns>
        [HttpGet("v1/GetUsersnRoles")]
        public async Task<ActionResult<ReturnResult>> GetUsersnRoles()
        {
            return await _sharedList.GetUsersnRoles();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/FillUserPaymentDropdown")]
        public async Task<ActionResult<ReturnResult>> FillUserPaymentDropdown()
        {
            return await _sharedList.FillUserPaymentDropdown();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/FillCampaignDropdown/{id}")]
        public async Task<ActionResult<ReturnResult>> FillCampaignDropdown(int id=0)
        {
            return await _sharedList.FillCampaignDropdown(id);
        }


    }
}
