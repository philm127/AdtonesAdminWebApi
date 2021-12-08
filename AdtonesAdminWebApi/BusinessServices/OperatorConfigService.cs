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
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class OperatorConfigService : IOperatorConfigService
    {
        private readonly IConfiguration _configuration;
        private readonly IConnectionStringService _connService;
        ReturnResult result = new ReturnResult();
        private readonly ILoggingService _logServ;
        const string PageName = "OperatorConfigService";

        public OperatorConfigService(IConfiguration configuration, IConnectionStringService connService, ILoggingService logServ)

        {
            _configuration = configuration;
            _connService = connService;
            _logServ = logServ;
        }


        public async Task<ReturnResult> UpdateOperatorConfig(OperatorConfigurationResult model)
        {
            var sb = new StringBuilder();
            var sb2 = new StringBuilder();
            sb.Append("UPDATE OperatorConfigurations SET Days = @Days,IsActive = @IsActive ");
            sb2.Append("UPDATE OperatorConfigurations SET Days = @Days,IsActive = @IsActive ");

            sb.Append("WHERE OperatorConfigurationId = @OperatorConfigurationId");
            sb2.Append("WHERE AdtoneServerOperatorConfigurationId = @OperatorConfigurationId");
            

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(sb.ToString(), model);
                }

                var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);

                using (var connection = new SqlConnection(constr))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(sb2.ToString(), model);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "UpdateOperatorConfig";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


    }
}