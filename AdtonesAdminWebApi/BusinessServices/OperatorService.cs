using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
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


        public OperatorService(IConfiguration configuration, IUserManagementService userService, IOperatorDAL opDAL)

        {
            _configuration = configuration;
            _userService = userService;
            _opDAL = opDAL;
        }


        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            try
            {
                    result.body = await _opDAL.LoadOperatorResultSet();
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "LoadOperatorDataTable"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "AddOperator"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "GetOperator"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "UpdateOperator"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "LoadOperatorMaxAdvertDataTable"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "AddOperatorMaxAdverts"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "GetOperatorMaxAdvert"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "UpdateOperatorMaxAdvert"
                };
                _logging.LogError();
                result.result = 0;
            }
            result.body = "Operator " + model.OperatorName + " updated successfully.";
            return result;
        }

    }
}