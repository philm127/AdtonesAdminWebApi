﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
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

        private readonly IBillingService _billService;
        private readonly IUserCreditService _creditService;
        private readonly IBillingDAL _billDAL;
        private readonly IUserCreditDAL _creditDAL;
        private readonly ILoggingService _logServ;
        ReturnResult result = new ReturnResult();
        const string PageName = "FinancialsController";
        private readonly IFinanceTablesDAL _invDAL;

        public FinancialsController(IBillingDAL billDAL, IBillingService billService, IUserCreditService creditService,
                                    ILoggingService logServ, IFinanceTablesDAL invDAL, IUserCreditDAL creditDAL)
        {
            _billService = billService;
            _creditService = creditService;
            _billDAL = billDAL;
            _logServ = logServ;
            _invDAL = invDAL;
            _creditDAL = creditDAL;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List InvoiceResult</returns>
        [HttpGet("v1/GetInvoiceData")]
        public async Task<ReturnResult> GetInvoiceData()
        {
            try
            {
                result.body = await _invDAL.LoadInvoiceResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetInvoiceData";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetInvoicListForSales/{id}")]
        public async Task<ReturnResult> GetInvoicListForSales(int id=0)
        {
            try
            {
                result.body = await _invDAL.LoadInvoiceResultSetForSales(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetInvoicListForSales");
                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetInvoicListForAdvertiser/{id}")]
        public async Task<ReturnResult> GetInvoicListForAdvertiser(int id)
        {
            try
            {
                result.body = await _invDAL.LoadInvoiceResultSetForAdvertiser(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetInvoicListForAdvertiser");
                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetBillingListForAdvertiser/{id}")]
        public async Task<ReturnResult> GetBillingListForAdvertiser(int id)
        {
            try
            {
                result.body = await _invDAL.LoadAdvertiserBillingDetails(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetBillingListForAdvertiser";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetOutstandingInvoicListForSales/{id}")]
        public async Task<ReturnResult> GetOutstandingInvoicListForSales(int id = 0)
        {
            try
            {
                result.body = await _invDAL.LoadOutstandingInvoiceForSalesResultSet(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOutstandingInvoicListForSales";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        [HttpGet("v1/GetAdvertiserCreditListForSales/{id}")]
        public async Task<ReturnResult> GetAdvertiserCreditListForSales(int id = 0)
        {
            try
            {
                result.body = await _invDAL.LoadUserCreditForSalesResultSet(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetAdvertiserCreditListForSales";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Only returns result 1 or 0</returns>
        [HttpPost("v1/SendInvoice")]
        public async Task<ReturnResult> SendInvoice(IdCollectionViewModel model)
        {
            return await _billService.SendInvoice(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List InvoiceResult</returns>
        [HttpGet("v1/GetOutstandingInvoiceData")]
        public async Task<ReturnResult> GetOutstandingInvoiceData()
        {
            try
            {
                result.body = await _invDAL.LoadOutstandingInvoiceResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOutstandingInvoiceData";
                await _logServ.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains List CampaignCreditResult</returns>
        [HttpGet("v1/GetCampaignCreditPeriodData/{id}")]
        public async Task<ReturnResult> GetCampaignCreditPeriodData(int id = 0)
        {
            try
            {
                result.body = await _invDAL.LoadCampaignCreditResultSet(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCampaignCreditPeriodData";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPut("v1/UpdateCampaignCreditPeriod")]
        public async Task<ReturnResult> UpdateCampaignCreditPeriod(CampaignCreditPeriodCommand model)
        {
            try
            {
                // Need to do this to get OperatorId
                result.body = await _billDAL.UpdateCampaignCreditPeriod(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateCampaignCreditPeriod";
                await _logServ.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>body contains empty result</returns>
        [HttpPost("v1/InsertCampaignCreditPeriod")]
        public async Task<ReturnResult> InsertCampaignCreditPeriod(CampaignCreditPeriodCommand model)
        {
            try
            {
                // Need to do this to get OperatorId
                result.body = await _billDAL.InsertCampaignCreditPeriod(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddCampaignCreditPeriod";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns>body contains a decimal value</returns>
        //[HttpGet("v1/GetOutstandingBalance/{id}")]
        //public async Task<ReturnResult> GetOutstandingBalance(int id)
        //{
        //    return await _paymentService.GetOutstandingBalance(id);
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>body contains a decimal value</returns>
        [HttpGet("v1/GetToPayDetails/{id}")]
        public async Task<ReturnResult> GetToPayDetails(int id)
        {
            try
            {
                var details = await _billDAL.GetToPayDetails(id);
                details.OutstandingAmount = await _billDAL.GetCreditBalanceForInvoicePayment(id);
                result.body = details;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetToPayDetails";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        ///// <summary>
        ///// Is actually a list of outstanding invoices against a campaign
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns>body contains List SharedSelectListViewModel</returns>
        //[HttpGet("v1/GetInvoiceDropdown/{id}")]
        //public async Task<ReturnResult> GetInvoiceDropdown(int id)
        //{
        //    return await _paymentService.GetInvoiceDetails(id);
        //}

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains success message</returns>
        [HttpPost("v1/ReceivePayment")]
        public async Task<ReturnResult> ReceivePayment(AdvertiserCreditFormCommand model)
        {
            return await _billService.ReceivePayment(model);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns>Body contains List UserCreditResult</returns>
        [HttpGet("v1/GetCreditData")]
        public async Task<ReturnResult> GetCreditData()
        {
            try
            {
                result.body = await _invDAL.LoadUserCreditResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetCreditData";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_creditmodel"></param>
        /// <returns>body contains nothing</returns>
        [HttpPost("v1/AddCredit")]
        public async Task<ReturnResult> AddCredit(AdvertiserCreditFormCommand _creditmodel)
        {
            return await _creditService.AddUserCredit(_creditmodel);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>body contains UsersCreditFormModel</returns>
        [HttpGet("v1/GetCreditDetails/{id}")]
        public async Task<ReturnResult> GetCreditDetails(int id)
        {
            string select_query = string.Empty;

            try
            {
                var creddet = await _creditDAL.GetUserCreditDetail(id);
                var credhist = await _billDAL.GetUserCreditPaymentHistory(id);
                creddet.PaymentHistory = credhist.ToList();

                result.body = creddet;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CreditDetails";
                await _logServ.LogError();

                result.result = 0;
                return result;
            }
            return result;
        }


        ///// <summary>
        ///// Updates FROM the User Credit Details screen 
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns>body contains nothing</returns>
        //[HttpPut("v1/UpdateCredit")]
        //public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel model)
        //{
        //    return await _creditService.UpdateCredit(model);
        //}



        /// TODO: Await design decisiones

        //[Route("v1/GetCreditDetailsUsersList")]
        //public async Task<ReturnResult> GetCreditDetailsUsersList()
        //{
        //    return await _creditService.GetCreditDetailsUsersList();
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>body contains BillingPaymentModel</returns>
        [HttpGet("v1/GetPaymentData/{id}")]
        public async Task<ReturnResult> GetPaymentData(int id)
        {
            try
            {
                result.body = await _billDAL.GetCampaignBillingData(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetPaymentData");
                result.result = 0;
                result.body = ex.Message.ToString();
                return result;
            }
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>body contains BillingPaymentModel</returns>
        [HttpGet("v1/GetPaymentDataAdvertiser/{id}")]
        public async Task<ReturnResult> GetPaymentDataAdvertiser(int id)
        {
            try
            {
                result.body = await _billDAL.GetAdvertiserBillingData(id);
            }
            catch (Exception ex)
            {
                await _logServ.LoggingError(ex, PageName, "GetPaymentDataAdvertiser");
                result.result = 0;
                result.body = ex.Message.ToString();
                return result;
            }
            return result;
        }


        [Route("v1/PayWithUserCredit")]
        public async Task<ReturnResult> PayWithUserCredit(UserPaymentCommand model)
        {
            return await _billService.PaywithUserCredit(model);
        }

    }
}