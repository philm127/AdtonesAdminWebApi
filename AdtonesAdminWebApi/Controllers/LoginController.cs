﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
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
            return await _userService.Login(userForm);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains User</returns>
        [AllowAnonymous]
        [HttpGet("v1/ForgotPassword/{email}")]
        public async Task<ReturnResult> ForgotPassword(string email)
        {
            return await _userService.ForgotPassword(email);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains User</returns>
        [AllowAnonymous]
        [HttpPut("v1/UpdatePassword")]
        public async Task<ReturnResult> UpdatePassword(PasswordModel model)
        {
            return await _userService.ChangePassword(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains User</returns>
        [AllowAnonymous]
        [HttpPut("v1/ResetPassword")]
        public async Task<ReturnResult> ResetPassword(PasswordModel model)
        {
            return await _userService.ResetPassword(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">email</param>
        /// <returns>body contains token string</returns>
        [Authorize]
        [HttpGet("v1/RefreshAccessToken/{id}")]
        public async Task<ReturnResult> RefreshAccessToken(string id)
        {
            return await _userService.RefreshAccessToken(id);
        }
        

    }
}