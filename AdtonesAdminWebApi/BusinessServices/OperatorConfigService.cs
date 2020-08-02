using AdtonesAdminWebApi.BusinessServices.Interfaces;
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
    public class OperatorConfigService : IOperatorConfigService
    {
        private readonly IConfiguration _configuration;

        ReturnResult result = new ReturnResult();

        public OperatorConfigService(IConfiguration configuration)

        {
            _configuration = configuration;
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "LoadOperatorConfigurationDataTable"
                };
                _logging.LogError();
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
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "GetOperatorConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddOperatorConfig(OperatorConfigurationResult model)
        {
            var insert_query = @"INSERT INTO OperatorConfigurations(OperatorId,Days,IsActive,AddedDate,UpdatedDate)
                                        VALUES(@OperatorId,@Days,true,GETDATE(),GETDATE());
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(insert_query, model);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "AddOperatorConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateOperatorConfig(OperatorConfigurationResult model)
        {
            var update_query = @"UPDATE OperatorConfigurations SET Days = @Days,IsActive = @IsActive 
                                            WHERE OperatorConfigurationId = @OperatorConfigurationId)";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(update_query, model);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "OperatorService",
                    ProcedureName = "UpdateOperatorConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


    }
}