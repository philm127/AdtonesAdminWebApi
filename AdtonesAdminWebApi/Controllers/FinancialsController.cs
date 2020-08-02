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

        private readonly IFinanceTablesService _invoiceService;
        private readonly IAdvertiserPaymentService _paymentService;
        private readonly IAdvertiserCreditService _creditService;


        public FinancialsController(IFinanceTablesService invoiceService, IAdvertiserPaymentService paymentService, IAdvertiserCreditService creditService)
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
            return await _paymentService.SendInvoice(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List InvoiceResult</returns>
        [HttpGet("v1/GetOutstandingInvoiceData")]
        public async Task<ReturnResult> GetOutstandingInvoiceData()
        {
            return await _invoiceService.LoadOutstandingInvoiceDataTable();
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
        public async Task<ReturnResult> ReceivePayment(AdvertiserCreditFormModel model)
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
            var tst = await _invoiceService.LoadUserCreditDataTable();
            return tst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_creditmodel"></param>
        /// <returns>body contains nothing</returns>
        [HttpPost("v1/AddCredit")]
        public async Task<ReturnResult> AddCredit(AdvertiserCreditFormModel _creditmodel)
        {
            var tst = await _creditService.AddCredit(_creditmodel);
            return tst;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains UsersCreditFormModel</returns>
        [HttpGet("v1/GetCreditDetails/{id}")]
        public async Task<ReturnResult> GetCreditDetails(int id)
        {
            return await _creditService.GetCreditDetails(id);
        }


        ///// <summary>
        ///// Updates FROM the User Credit Details screen 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns>body contains nothing</returns>
        //[HttpPut("v1/UpdateCredit")]
        //public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel model)
        //{
        //    var tst = await _creditService.UpdateCredit(model);
        //    return tst;
        //}



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