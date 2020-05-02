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

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class SharedListController : ControllerBase
    {
        private readonly ISharedSelectListsService _sharedList;
        private readonly ILogger<SharedListController> _logger;

        public SharedListController(ISharedSelectListsService sharedList, ILogger<SharedListController> logger)
        {
            _sharedList = sharedList;
            _logger = logger;
        }


        [Route("v1/GetCountryList")]
        public async Task<ReturnResult> GetCountryList()
        {
            return await _sharedList.GetCountryList();
        }


        [Route("v1/GetRoleList")]
        public ReturnResult GetRoleList()
        {
            return  _sharedList.GetUserRole();
        }


        [Route("v1/GetUserStatusList")]
        public ReturnResult GetUserStatusList()
        {
            return _sharedList.GetUserStatus();
        }

        [Route("v1/GetOperatorList")]
        public async Task<ReturnResult> GetOperatorList([FromBodyAttribute] IdCollectionViewModel some)
        {
            return await _sharedList.GetOperatorList(some.countryId);
        }

        /// <summary>
        /// Uses the IdCollectionViewModel to get id from a model rather than the url
        /// </summary>
        /// <param name="some">some contains currencyId among other simple Id's</param>
        /// <returns>An IEnumerable collection to populate drop down list. 
        /// This is a list of currencies or a single one</returns>
        [Route("v1/GetCurrencyList")]
        public async Task<ReturnResult> GetCurrencyList([FromBodyAttribute]IdCollectionViewModel some)
        {
            return await _sharedList.GetCurrencyList(some.currencyId);
        }


        [Route("v1/GetUserById")]
        public async Task<ReturnResult> GetUserById(IdCollectionViewModel some)
        {
            return await _sharedList.GetUserById(some.userId);
        }


    }
}
