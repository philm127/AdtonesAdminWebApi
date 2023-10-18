using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogonService _userService;
        private readonly ILoggingService _logServ;

        public LoginController(ILogonService userService, ILoggingService logServ)
        {
            _userService = userService;
            _logServ = logServ;
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
            try
            {
                _logServ.ErrorMessage = $"Entered Login";
                _logServ.StackTrace = "";
                _logServ.PageName = "LoginController";
                _logServ.ProcedureName = "Login";
                await _logServ.LogError();
                await _logServ.LogInfo();
                return await _userService.Login(userForm);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "LoginController";
                _logServ.ProcedureName = "Login";
                await _logServ.LogError();
                var result = new ReturnResult();
                result.result = 0;
                return result;
            }
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