using AdtonesAdminWebApi.Common;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class BaseTestDAL //: IBaseTestDAL
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();

        public BaseTestDAL(IConfiguration config)
        {
            _configuration = config;
        }

        public async Task<ReturnResult> GetAll(string sql_query,QModel<T> model)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    result.body = await connection.QueryAsync<SharedSelectListViewModel>(@"SELECT Id AS Value,Name AS Text FROM Country");
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SharedSelectListsService",
                    ProcedureName = "GetCountryList"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<object> GetAll<T>(string sql_query, object model, dynamic retModel, object clause=null)
        {
//            var shop = GetAll(typeof(T),retModel);
            try
            {
                var getType = retModel.GetType();
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    return = await connection.QueryAsync<getType>(sql_query);

                    {
                    }
            }
            catch
            {
                throw;
            }
        }
    }
}