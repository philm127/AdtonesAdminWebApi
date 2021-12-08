using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class OperatorService : IOperatorService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagementService _userService;
        private readonly IOperatorDAL _opDAL;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "OperatorService";

        public OperatorService(IConfiguration configuration, IUserManagementService userService, IOperatorDAL opDAL, ILoggingService logServ)

        {
            _configuration = configuration;
            _userService = userService;
            _opDAL = opDAL;
            _logServ = logServ;
        }



        public async Task<ReturnResult> UpdateOperatorMaxAdverts(OperatorMaxAdvertsFormModel model)
        {

            try
            {
               var x = await _opDAL.UpdateOperatorMaxAdvert(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateOperatorMaxAdvert";
                await _logServ.LogError();
                
                result.result = 0;
            }
            result.body = "Operator " + model.OperatorName + " updated successfully.";
            return result;
        }

    }
}