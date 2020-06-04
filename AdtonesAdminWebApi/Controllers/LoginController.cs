using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogonService _userService;

        public LoginController(ILogonService userService)
        {
            _userService = userService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userForm"></param>
        /// <returns>body contains User</returns>
        [AllowAnonymous]
        [HttpPost("v1/Login")]
        public async Task<ReturnResult> Login(User userForm)
        {
            var tst = await _userService.Login(userForm);
            return tst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains User</returns>
        [AllowAnonymous]
        [HttpPost("v1/ForgotPassword")]
        public async Task<ReturnResult> ForgotPassword(IdCollectionViewModel model)
        {
            var tst = await _userService.ForgotPassword(model.Email);
            return tst;
        }
    }
}