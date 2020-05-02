using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdtonesAdminWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialsController : ControllerBase
    {

        private readonly IInvoiceService _invoiceService;
        private readonly IUserPaymentService _paymentService;


        public FinancialsController(IInvoiceService invoiceService, IUserPaymentService paymentService)
        {
            _invoiceService = invoiceService;
            _paymentService = paymentService;

        }


        [Route("v1/GetInvoiceData")]
        public async Task<ReturnResult> GetInvoiceData()
        {
            return await _invoiceService.LoadDataTable();
        }


        [Route("v1/SendInvoice")]
        public async Task<ReturnResult> SendInvoice(IdCollectionViewModel model)
        {
            return await _invoiceService.SendInvoice(model);
        }


        [Route("v1/GetOutstandingInvoiceData")]
        public async Task<ReturnResult> GetOutstandingInvoiceData()
        {
            return await _paymentService.LoadDataTable();
        }


        [Route("v1/FillCampaignDropdown")]
        public async Task<ReturnResult> FillCampaignDropdown()
        {
            return await _paymentService.FillCampaignDropdown();
        }


        [Route("v1/FillUserPaymentDropdown")]
        public async Task<ReturnResult> FillUserPaymentDropdown()
        {
            return await _paymentService.FillUserPaymentDropdown();
        }


        [Route("v1/GetOutstandingBalance")]
        public async Task<ReturnResult> GetOutstandingBalance(IdCollectionViewModel model)
        {
            return await _paymentService.GetOutstandingBalance(model);
        }


        [Route("v1/GetInvoiceDetails")]
        public async Task<ReturnResult> GetInvoiceDetails(IdCollectionViewModel model)
        {
            return await _paymentService.GetInvoiceDetails(model);
        }

        
        [Route("v1/ReceivePayment")]
        public async Task<ReturnResult> ReceivePayment(UserCreditPaymentFormModel model)
        {
            return await _paymentService.ReceivePayment(model);
        }

    }
}