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
    public class SystemConfigService : ISystemConfigService
    {
        private readonly IConfiguration _configuration;
        
        ReturnResult result = new ReturnResult();


        public SystemConfigService(IConfiguration configuration, IUserManagementService userService)

        {
            _configuration = configuration;
        }


        public async Task<ReturnResult> LoadSystemConfigurationDataTable()
        {
            var select_query = @"SELECT SystemConfigId,SystemConfigKey,SystemConfigValue,CreatedDateTime AS CreatedDate,
                                                        ISNULL(SystemConfigType,'-') AS SystemConfigType
                                                        FROM SystemConfig";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<SystemConfigResult>(select_query);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SystemConfigService",
                    ProcedureName = "LoadSystemConfigurationDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetSystemConfig(int id)
        {
            var select_query = @"SELECT SystemConfigId,SystemConfigKey,SystemConfigValue,CreatedDateTime AS CreatedDate,
                                    ISNULL(SystemConfigType,'-') AS SystemConfigType
                                    FROM SystemConfig
                                    WHERE SystemConfigId=@Id";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<SystemConfigResult>(select_query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SystemConfigService",
                    ProcedureName = "GetOperatorConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddSystemConfig(SystemConfigResult model)
        {
            var insert_query = @"INSERT INTO SystemConfig(SystemConfigKey, SystemConfigValue, SystemConfigType,CreatedDateTime,UpdatedDateTime)
                                        VALUES(@SystemConfigKey, @SystemConfigValue, @SystemConfigType,GETDATE(),GETDATE());
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
                    PageName = "SystemConfigService",
                    ProcedureName = "AddSystemConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> DeleteSystemConfig(int id)
        {
            var delete_query = @"DELETE FROM SystemConfig WHERE SystemConfigId=@Id;";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(delete_query, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SystemConfigService",
                    ProcedureName = "DeleteSystemConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateSystemConfig(SystemConfigResult model)
        {
            var update_query = @"UPDATE SystemConfig SET SystemConfigKey = @SystemConfigKey, @SystemConfigValue=SystemConfigValue, 
                                                                       @SystemConfigType=SystemConfigType,UpdatedDateTime=GETDATE()
                                                                       WHERE SystemConfigId = @SystemConfigId)";

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
                    PageName = "SystemConfigService",
                    ProcedureName = "UpdateOperatorConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public ReturnResult GetSystemConfigType()
        {
            var selList = new List<SharedSelectListViewModel>();

            selList.Add(new SharedSelectListViewModel { Value = "Website", Text = "Website" });
            selList.Add(new SharedSelectListViewModel { Value = "ProvisioningService", Text = "ProvisioningService" });
            result.body = selList;
            return result;
        }


    }
}