using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Linq;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
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
        public async Task<ReturnResult> GetCountryList()
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
        [HttpGet("v1/GetUserStatusList")]
        public ReturnResult GetUserStatusList()
        {
            return _sharedList.GetUserStatus();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="some"></param>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetOperatorList")]
        public async Task<ReturnResult> GetOperatorList([FromBodyAttribute] IdCollectionViewModel some)
        {
            return await _sharedList.GetOperatorList(some.countryId);
        }


        /// <summary>
        /// Uses the IdCollectionViewModel to get id from a model rather than the url
        /// </summary>
        /// <param name="some">some contains currencyId among other simple Id's</param>
        /// <returns>body contains List SharedSelectListViewModel
        /// or a single one if id entered</returns>
        [HttpGet("v1/GetCurrencyList")]
        public async Task<ReturnResult> GetCurrencyList([FromBodyAttribute]IdCollectionViewModel some)
        {
            //_sharedList.CurrentUserId = int.Parse(User.FindFirst("userId")?.Value);
            // _sharedList.RoleName = User.FindFirst(ClaimTypes.Role)?.Value;
            return await _sharedList.GetCurrencyList(some.currencyId);
        }


    }
}
