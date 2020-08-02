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


        public FinanceTablesService(IConfiguration configuration, IFinanceTablesDAL invDAL)

        {
            _configuration = configuration;
            _invDAL = invDAL;
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "InvoiceService",
                    ProcedureName = "LoadData"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "InvoiceService",
                    ProcedureName = "LoadData"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "FinanceTablesService",
                    ProcedureName = "GetUserResult"
                };
                _logging.LogError();
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
