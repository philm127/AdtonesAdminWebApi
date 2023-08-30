using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUserManagementAddUserService _userService;
        private readonly IUserManagementAddUserDAL _userDAL;
        ReturnResult result = new ReturnResult();

        public RegisterController(IUserManagementAddUserService userService, IUserManagementAddUserDAL userDAL)
        {
            _userService = userService;
            _userDAL = userDAL;
        }

        [HttpPost("v1/AddNewUser")]
        public async Task<ReturnResult> AddNewUser(UserAddFormModel model)
        {
            return await _userService.AddUser(model, true);
        }


        [HttpGet("v1/VerifyEmail/{code}")]
        public async Task<ReturnResult> VerifyEmail(string code)
        {
            result.body = await _userDAL.VerifyEmail(code);
            return result;
        }
    }
}
