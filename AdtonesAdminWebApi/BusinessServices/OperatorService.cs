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


        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            try
            {
                    result.body = await _opDAL.LoadOperatorResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadOperatorDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddOperator(OperatorFormModel operatormodel)
        {
            try
            {
                if (await _opDAL.CheckOperatorExists(operatormodel))
                {
                    result.error = operatormodel.OperatorName + " Record Exists.";
                    result.result = 0;
                    return result;
                }

                result.body = await _opDAL.AddOperator(operatormodel);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddOperator";
                await _logServ.LogError();
                
                result.result = 0;
            }
            result.body = "Operator " + operatormodel.OperatorName + " added successfully.";
            return result;
        }


        public async Task<ReturnResult> GetOperator(int id)
        {
            try
            {
                result.body = await _opDAL.GetOperatorById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOperator";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;

        }


        public async Task<ReturnResult> UpdateOperator(OperatorFormModel operatormodel)
        {
            try
            {
                var x = await _opDAL.UpdateOperator(operatormodel);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateOperator";
                await _logServ.LogError();
                
                result.result = 0;
            }
            result.body = "Operator " + operatormodel.OperatorName + " updated successfully.";
            return result;
        }


        public async Task<ReturnResult> LoadOperatorMaxAdvertDataTable()
        {

            try
            {
                result.body = await _opDAL.LoadOperatorMaxAdvertResultSet();
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadOperatorMaxAdvertDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddOperatorMaxAdverts(OperatorMaxAdvertsFormModel model)
        {
            try
            {
                if (await _opDAL.CheckMaxAdvertExists(model))
                {
                    result.error = model.KeyName + " Record Exists.";
                    result.result = 0;
                    return result;
                }

                var x = await _opDAL.AddOperatorMaxAdvert(model);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddOperatorMaxAdverts";
                await _logServ.LogError();
                
                result.result = 0;
            }
            result.body = "Added successfully.";
            return result;
        }


        public async Task<ReturnResult> GetOperatorMaxAdvert(int id)
        {
            try
            {
                result.body = await _opDAL.GetOperatorMaxAdvertById(id);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOperatorMaxAdvert";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;

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