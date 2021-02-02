using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.Services.Mailer;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class FinanceTablesService : IFinanceTablesService
    {
        private readonly IConfiguration _configuration;
        private readonly IFinanceTablesDAL _invDAL;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "FinanceTableService";

        public FinanceTablesService(IConfiguration configuration, IFinanceTablesDAL invDAL, ILoggingService logServ)

        {
            _configuration = configuration;
            _invDAL = invDAL;
            _logServ = logServ;
        }


        /// <summary>
        /// Populate the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadInvoiceDataTable()
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
                _logServ.ProcedureName = "LoadInvoiceData";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadInvoiceDataTableForSales(int id = 0)
        {
            try
            {
                result.body = await _invDAL.LoadInvoiceResultSetForSales(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadInvoiceDataTableForSales";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadOutstandingInvoiceDataTable()
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
                _logServ.ProcedureName = "LoadOutstandingInvoiceResultSet";
                await _logServ.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadOutstandingInvoiceForSalesDataTable(int id = 0)
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
                _logServ.ProcedureName = "LoadOutstandingInvoiceForSalesDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Populates the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadUserCreditDataTable()
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
                _logServ.ProcedureName = "LoadUserCreditDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadUserCreditDataTableForSales(int id = 0)
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
                _logServ.ProcedureName = "LoadUserCreditDataTableForSales";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> LoadCampaignCreditPeriodTable(int id = 0)
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
                _logServ.ProcedureName = "LoadCampaignCreditPeriodTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }



        public bool UpdateUserCredit(int userid, decimal receivedamount)
        {
            bool status = false;
            //var usercredit = _usercreditRepository.Get(top => top.UserId == userid);
            //if (usercredit != null)
            //{
            //    var AvailableCredit = usercredit.AvailableCredit;
            //    AvailableCredit = AvailableCredit + receivedamount;
            //    UpdateUserCreditCommand command = new UpdateUserCreditCommand();
            //    command.UserId = userid;
            //    command.AvailableCredit = AvailableCredit;
            //    ICommandResult result = _commandBus.Submit(command);
            //    if (result.Success)
            //    {
            //        status = true;
            //    }
            //}
            status = true;
            return status;
        }
    }
}
