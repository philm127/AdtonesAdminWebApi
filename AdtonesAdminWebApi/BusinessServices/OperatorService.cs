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
    public class OperatorService
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManagementService _userService;
        ReturnResult result = new ReturnResult();


        public OperatorService(IConfiguration configuration, IUserManagementService userService)

        {
            _configuration = configuration;
            _userService = userService;
        }


        public async Task<ReturnResult> LoadOperatorDataTable()
        {
            var select_query = @"SELECT OperatorId,OperatorName,ISNULL(co.CountryName, '-') AS CountryName,op.IsActive,
                                EmailCost,SmaCost,cu.CurrencyCode
                                FROM Operators AS op LEFT JOIN Country co ON op.CountryId=co.Id
                                LEFT JOIN Currencies cu ON cu.CurrencyId = op.CurrencyId";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<OperatorResult>(select_query);
                }
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
                bool exists = false;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM Operators WHERE LOWER(OperatorName) = @op;",
                                                                  new { op = operatormodel.OperatorName.ToLower() });
                }
                if (exists)
                {
                    result.error = operatormodel.OperatorName + " Record Exists.";
                    result.result = 0;
                    return result;
                }

                var insert_query = @"INSERT INTO Operators(OperatorName,CountryId,IsActive,EmailCost,SmsCost,CurrencyId)
                                                    VALUES(@OperatorName,@CountryId,true,@EmailCost,@SmsCost,@CurrencyId);
                                      SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteScalarAsync<int>(insert_query, operatormodel);
                }
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


        public async Task<ReturnResult> GetOperator(IdCollectionViewModel model)
        {
            var select_query = @"SELECT op.OperatorId,OperatorName,co.Name AS CountryName,cu.CurrencyCode,AdtoneServerOperatorId,
	                                IsActive,EmailCost,SmsCost,op.CurrencyId
                                    FROM Operators AS op LEFT JOIN Country AS co ON op.CountryId=co.Id
                                    LEFT JOIN Currencies AS cu ON op.CurrencyId=cu.CurrencyId WHERE OperatorId=@Id";
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<OperatorFormModel>(select_query,
                                                                                        new { Id = model.id });
                }
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
                var update_query = @"UPDATE Operators SET IsActive=@IsActive,EmailCost=@EmailCost,SmsCost=@SmsCost 
                                                                WHERE OperatorId=@OperatorId";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(update_query, operatormodel);
                }
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
            var select_query = @"SELECT OperatorMaxAdvertId,KeyName,KeyValue,Addeddate,maxad.OperatorId,op.OperatorName
                                 FROM OperatorMaxAdverts AS maxad INNER JOIN Operators AS op ON op.OperatorId=maxad.OperatorId";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<OperatorMaxAdvertsFormModel>(select_query);
                }
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


        public async Task<ReturnResult> AddOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatorMaxAdvertsFormModel)
        {
            try
            {
                bool exists = false;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    exists = await connection.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM OperatorMaxAdverts 
                                                                        WHERE LOWER(KeyName) = @keyname AND OperatorId=@opid;",
                                                                  new { keyname = operatorMaxAdvertsFormModel.KeyName.ToLower(), opid = operatorMaxAdvertsFormModel.OperatorId });
                }
                if (exists)
                {
                    result.error = operatorMaxAdvertsFormModel.KeyName + " Record Exists.";
                    result.result = 0;
                    return result;
                }

                var insert_query = @"INSERT INTO OperatorMaxAdverts(KeyName,KeyValue,Addeddate,Updateddate,OperatorId)
                                                    VALUES(@KeyName,@KeyValue,GETDATE(),GETDATE(),@OperatorId);
                                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(insert_query, operatorMaxAdvertsFormModel);
                }
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
            var select_query = @"SELECT OperatorMaxAdvertId,KeyName,KeyValue,op.OperatorName
                                 FROM OperatorMaxAdverts AS maxad INNER JOIN Operators AS op ON op.OperatorId=maxad.OperatorId";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<OperatorMaxAdvertsFormModel>(select_query);
                }
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


        public async Task<ReturnResult> UpdateOperatorMaxAdverts(OperatorMaxAdvertsFormModel operatorMaxAdvertsFormModel)
        {

            var update_query = @"UPDATE OperatorMaxAdverts SET KeyValue=@KeyValue,Updateddate=GETDATE() 
                                                        WHERE OperatorMaxAdvertId=@OperatorMaxAdvertId";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    var x = await connection.ExecuteScalarAsync<int>(update_query, operatorMaxAdvertsFormModel);
                }
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
            result.body = "Operator " + operatorMaxAdvertsFormModel.OperatorName + " updated successfully.";
            return result;
        }


        public async Task<ReturnResult> LoadOperatorConfigurationDataTable()
        {
            var select_query = @"SELECT OperatorConfigurationId,con.OperatorId,Days,con.IsActive,AddedDate,op.OperatorName
                                FROM dbo.OperatorConfigurations AS con INNER JOIN Operators AS op ON op.OperatorId=con.OperatorId";

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


        public async Task<ReturnResult> GetOperatorConfig(IdCollectionViewModel model)
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
                    result.body = await connection.QueryFirstOrDefaultAsync<OperatorConfigurationResult>(select_query, new { Id = model.id });
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