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


        public async Task<ReturnResult> LoadOperatorConfigurationDataTable()
        {
            var select_query = @"SELECT OperatorConfigurationId,con.OperatorId,Days,con.IsActive,AddedDate AS CreatedDate,op.OperatorName,c.Name AS CountryName
                                FROM dbo.OperatorConfigurations AS con INNER JOIN Operators AS op ON op.OperatorId=con.OperatorId
                                INNER JOIN Country AS c ON c.Id=op.CountryId";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<OperatorConfigurationResult>(select_query);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "LoadOperatorConfigurationDataTable";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetOperatorConfig(int id)
        {
            var select_query = @"SELECT OperatorConfigurationId,con.OperatorId,Days,con.IsActive,AddedDate,op.OperatorName
                                FROM dbo.OperatorConfigurations AS con 
                                INNER JOIN Operators AS op ON op.OperatorId=con.OperatorId
                                WHERE OperatorConfigurationId=@Id";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<OperatorConfigurationResult>(select_query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetOperatorConfig";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddOperatorConfig(OperatorConfigurationResult model)
        {
            var insert_query = @"INSERT INTO OperatorConfigurations(OperatorId,Days,IsActive,AddedDate,UpdatedDate,AdtoneServerOperatorConfigurationId)
                                        VALUES(@OperatorId,@Days,@IsActive,GETDATE(),GETDATE(),@AdtoneServerOperatorConfigurationId);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    model.AdtoneServerOperatorConfigurationId = await connection.ExecuteScalarAsync<int>(insert_query, model);
                }

                var constr = await _connService.GetConnectionStringByOperator(model.OperatorId);


                using (var connection = new SqlConnection(constr))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(insert_query, model);
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "AddOperatorConfig";
                await _logServ.LogError();
                
                result.result = 0;
            }
            return result;
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