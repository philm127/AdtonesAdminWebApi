using Microsoft.AspNetCore.Mvc;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using System;
using System.Collections.Generic;
using AdtonesAdminWebApi.Enums;
using System.Linq;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SharedListController : ControllerBase
    {
        private readonly ISharedSelectListsService _sharedList;
        private readonly ILoggingService _logServ;
        private readonly ISharedSelectListsDAL _sharedDal;
        ReturnResult result = new ReturnResult();
        const string PageName = "SharedListController";

        public SharedListController(ISharedSelectListsService sharedList, ISharedSelectListsDAL sharedDal, ILoggingService logServ)
        {
            _sharedList = sharedList;
            _sharedDal = sharedDal;
            _logServ = logServ;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetCountryList")]
        public async Task<ActionResult<ReturnResult>> GetCountryList()
        {
            try
            {
                result.body = await _sharedDal.GetCountry();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetCountryList");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetCountryListForAdvertiserSignUp")]
        public async Task<ActionResult<ReturnResult>> GetCountryListForAdvertiserSignUp()
        {
            try
            {
                result.body = await _sharedDal.GetCountryForAdSignUp();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetCountryListForAdvertiserSignUp");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetRoleList")]
        public ReturnResult GetRoleList()
        {
            IEnumerable<UserRole> userroleTypes = Enum.GetValues(typeof(UserRole))
                                                     .Cast<UserRole>();
            result.body = (from action in userroleTypes
                           select new SharedSelectListViewModel
                           {
                               Text = action.ToString(),
                               Value = ((int)action).ToString()
                           }).ToList();
            return result;
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
            IEnumerable<UserStatus> userTypes = Enum.GetValues(typeof(UserStatus))
                                                     .Cast<UserStatus>();
            result.body = (from action in userTypes
                           select new SharedSelectListViewModel
                           {
                               Text = action.ToString(),
                               Value = ((int)action).ToString()
                           }).ToList();
            return result;
        }


        [HttpGet("v1/GetTicketStatusList")]
        public ReturnResult GetTicketStatusList()
        {
            IEnumerable<QuestionStatus> userTypes = Enum.GetValues(typeof(QuestionStatus))
                                                     .Cast<QuestionStatus>();
            result.body = (from action in userTypes
                           select new SharedSelectListViewModel
                           {
                               Text = action.ToString(),
                               Value = ((int)action).ToString()
                           }).ToList();
            return result;
        }


        [HttpGet("v1/GetTicketSubjectList")]
        public async Task<ActionResult<ReturnResult>> GetTicketSubjectList()
        {
            try
            {
                result.body = await _sharedDal.GetTicketSubjectList();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetTicketSubjectList");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetClientList/{id}")]
        public async Task<ActionResult<ReturnResult>> GetClientList(int id)
        {
            try
            {
                result.body = await _sharedDal.GetClientList(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetClientList");

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/FillPaymentTypeDropDown")]
        public async Task<ActionResult<ReturnResult>> FillPaymentTypeDropDown()
        {
            try
            {
                result.body = await _sharedDal.GetPaymentTypeList();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "FillPaymentTypeDropdown");

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">CountryId</param>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetOperatorList/{id}")]
        public async Task<ActionResult<ReturnResult>> GetOperatorList(int id = 0)
        {
            var countryId = id;
            try
            {
                result.body = await _sharedDal.GetOperators(countryId);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetOperatorList");

                result.result = 0;
            }
            return result;
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
            var currencyId = id;
            try
            {
                result.body = await _sharedDal.GetCurrency(currencyId);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetCurrencyList");
                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/FillOrganisationTypeDropDown")]
        public async Task<ActionResult<ReturnResult>> FillOrganisationTypeDropDown()
        {
            try
            {
                result.body = await _sharedDal.GetOrganisationTypes();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetOrganisationTypeDropDown");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">countryId</param>
        /// <returns></returns>
        [HttpGet("v1/FillAdvertCategoryDropDown/{id}")]
        public async Task<ActionResult<ReturnResult>> FillAdvertCategoryDropDown(int id)
        {
            var countryId = id;
            try
            {
                result.body = await _sharedDal.GetAdvertCategory(countryId);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetAdvertCategoryDropDown");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">countryId</param>
        /// <returns></returns>
        [HttpGet("v1/FillCampaignCategoryDropDown/{id}")]
        public async Task<ActionResult<ReturnResult>> FillCampaignCategoryDropDown(int id)
        {
            var countryId = id;
            try
            {
                result.body = await _sharedDal.GetCampaignCategory(countryId);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetCampaignCategoryDropDown");
                result.result = 0;
            }
            return result;
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
            try
            {
                result.body = await _sharedDal.AddCreditUsers();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetAddCreditUsersList");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Gets the advertisers who currently do not have a credit yet
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel
        /// or a single one if id entered</returns>
        [HttpGet("v1/GetUserCreditList")]
        public async Task<ActionResult<ReturnResult>> GetUserCreditList()
        {
            try
            {
                result.body = await _sharedDal.GetCreditUsers();
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetUserCreditList");
                result.result = 0;
            }
            return result;
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
            try
            {
                result.body = await _sharedDal.GetUserPaymentList(0);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "FillUserPaymentDropdown");
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/FillCampaignDropdown/{id}")]
        public async Task<ActionResult<ReturnResult>> FillCampaignDropdown(int id=0)
        {
            try
            {
                result.body = await _sharedDal.GetCamapignList(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "FillCampaignDropdown");
                result.result = 0;
            }
            return result;
        }


    }
}
