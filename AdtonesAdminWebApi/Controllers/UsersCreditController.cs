using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using AdtonesAdminWebApi.Model;
using System.IO;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersCreditController : ControllerBase
    {
        private readonly IUsersCreditService _userService;

        public UsersCreditController(IUsersCreditService userService)
        {
            _userService = userService;
        }


        [HttpGet("v1/GetCreditData")]
        public async Task<ReturnResult> GetCreditData()
        {
            var tst = await _userService.LoadDataTable();
            return tst;
        }


        [HttpGet("v1/GetAddCreditUsersList")]
        public async Task<ReturnResult> GetAddCreditUsersList()
        {
            var tst = await _userService.GetAddCreditUsersList();
            return tst;
        }


        [Route("v1/AddCredit")]
        public async Task<ReturnResult> AddCredit(UserCreditFormModel _creditmodel)
        {
            var tst = await _userService.AddCredit(_creditmodel);
            return tst;
        }


        [Route("v1/GetCreditDetails")]
        public async Task<ReturnResult> GetCreditDetails(IdCollectionViewModel model)
        {
            var tst = await _userService.GetCreditDetails(model);
            return tst;
        }


        [Route("v1/UpdateCredit")]
        public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel model)
        {
            var tst = await _userService.UpdateCredit(model);
            return tst;
        }


        


        //[Route("v1/GetCreditDetailsUsersList")]
        //public async Task<ReturnResult> GetCreditDetailsUsersList()
        //{
        //    var tst = await _userService.GetCreditDetailsUsersList();
        //    return tst;
        //}


        //[Route("v1/UserCreditPayment")]
        //public async Task<ReturnResult> UserCreditPayment(IdCollectionViewModel model)
        //{
        //    var tst = await _userService.UserCreditpayment(model);
        //    return tst;
        //}

    }
}