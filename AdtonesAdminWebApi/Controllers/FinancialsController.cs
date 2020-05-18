using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class FinancialsController : ControllerBase
    {

        private readonly IInvoiceService _invoiceService;
        private readonly IUserPaymentService _paymentService;
        private readonly IUsersCreditService _creditService;


        public FinancialsController(IInvoiceService invoiceService, IUserPaymentService paymentService, IUsersCreditService creditService)
        {
            _invoiceService = invoiceService;
            _paymentService = paymentService;
            _creditService = creditService;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List InvoiceResult</returns>
        [HttpGet("v1/GetInvoiceData")]
        public async Task<ReturnResult> GetInvoiceData()
        {
            return await _invoiceService.LoadInvoiceDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Only returns result 1 or 0</returns>
        [HttpPost("v1/SendInvoice")]
        public async Task<ReturnResult> SendInvoice(IdCollectionViewModel model)
        {
            return await _invoiceService.SendInvoice(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List InvoiceResult</returns>
        [HttpGet("v1/GetOutstandingInvoiceData")]
        public async Task<ReturnResult> GetOutstandingInvoiceData()
        {
            return await _paymentService.LoadPaymentDataTable();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/FillCampaignDropdown")]
        public async Task<ReturnResult> FillCampaignDropdown()
        {
            return await _paymentService.FillCampaignDropdown();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/FillUserPaymentDropdown")]
        public async Task<ReturnResult> FillUserPaymentDropdown()
        {
            return await _paymentService.FillUserPaymentDropdown();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains a decimal value</returns>
        [HttpGet("v1/GetOutstandingBalance")]
        public async Task<ReturnResult> GetOutstandingBalance(IdCollectionViewModel model)
        {
            return await _paymentService.GetOutstandingBalance(model.id);
        }


        /// <summary>
        /// Is actually a list of outstanding invoices against a campaign
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetInvoiceDropdown")]
        public async Task<ReturnResult> GetInvoiceDropdown(IdCollectionViewModel model)
        {
            return await _paymentService.GetInvoiceDetails(model.id);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains success message</returns>
        [HttpPost("v1/ReceivePayment")]
        public async Task<ReturnResult> ReceivePayment(UserCreditPaymentFormModel model)
        {
            return await _paymentService.ReceivePayment(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Body contains List UserCreditResult</returns>
        [HttpGet("v1/GetCreditData")]
        public async Task<ReturnResult> GetCreditData()
        {
            var tst = await _creditService.LoadDataTable();
            return tst;
        }


        /// <summary>
        /// When Add Credit selected this populates dropdown with credit users
        /// </summary>
        /// <returns>body contains List SharedSelectListViewModel</returns>
        [HttpGet("v1/GetAddCreditUsersList")]
        public async Task<ReturnResult> GetAddCreditUsersList()
        {
            var tst = await _creditService.GetAddCreditUsersList();
            return tst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_creditmodel"></param>
        /// <returns>body contains nothing</returns>
        [HttpPost("v1/AddCredit")]
        public async Task<ReturnResult> AddCredit(UserCreditFormModel _creditmodel)
        {
            var tst = await _creditService.AddCredit(_creditmodel);
            return tst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains UsersCreditFormModel</returns>
        [HttpGet("v1/GetCreditDetails")]
        public async Task<ReturnResult> GetCreditDetails(IdCollectionViewModel model)
        {
            var tst = await _creditService.GetCreditDetails(model);
            return tst;
        }


        /// <summary>
        /// Updates FROM the User Credit Details screen 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains nothing</returns>
        [HttpPut("v1/UpdateCredit")]
        public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel model)
        {
            var tst = await _creditService.UpdateCredit(model);
            return tst;
        }



        /// TODO: Await design decisiones
        
        //[Route("v1/GetCreditDetailsUsersList")]
        //public async Task<ReturnResult> GetCreditDetailsUsersList()
        //{
        //    var tst = await _creditService.GetCreditDetailsUsersList();
        //    return tst;
        //}


        //[Route("v1/UserCreditPayment")]
        //public async Task<ReturnResult> UserCreditPayment(IdCollectionViewModel model)
        //{
        //    var tst = await _creditService.UserCreditpayment(model);
        //    return tst;
        //}

    }
}