using AdtonesAdminWebApi.BusinessServices.Interfaces;
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
    public class AreaService : IAreaService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public AreaService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        // Listing Area
        public async Task<ReturnResult> LoadDataTable()
        {

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<AreaResult>(@"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName 
                                                                                    FROM Areas AS a INNER JOIN Country AS c
                                                                                    ON a.CountryId=c.Id
                                                                                    WHERE a.IsActive=1
                                                                                    ORDER BY a.AreaId DESC");
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "FillAreaResult"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddArea(AreaResult areamodel)
        {
            areamodel.IsActive = true;
            try
            {
                if (CheckAreaExists(areamodel))
                {
                    result.result = 0;
                    result.error = areamodel.AreaName + " Record Exists.";
                    return result;
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "AddArea-CheckUnique"
                };
                _logging.LogError();
                result.result = 0;
            }

            try
            {
                string query = @"INSERT INTO Areas(AreaName,IsActive,CountryId) 
                                VALUES(@AreaName,@IsActive,@CountryId)";
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var cnt = await connection.ExecuteAsync(query, areamodel);

                    if (cnt != 1)
                    {
                        result.result = 0;
                        result.error = areamodel.AreaName + " Record was not inserted.";
                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "AddArea"
                };
                _logging.LogError();
                result.result = 0;
                result.error = areamodel.AreaName + " Record was not inserted.";
                return result;
            }
        }


        public async Task<ReturnResult> GetArea(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<AreaResult>(@"SELECT a.AreaId, a.AreaName,a.CountryId,c.Name as CountryName,a.IsActive 
                                                                                    FROM Areas AS a INNER JOIN Country AS c
                                                                                    ON a.CountryId=c.Id
                                                                                    WHERE a.AreaId = @areaid",
                                                                                    new { areaid = id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "GetArea"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateArea(AreaResult areamodel)
        {
                if (CheckAreaExists(areamodel))
                {
                    result.result = 0;
                    result.error = areamodel.AreaName + " Record Exists.";
                    return result;
                }
                areamodel.IsActive = true;

            try
            {
                string query = @"UPDATE Areas SET AreaName=@AreaName,IsActive=@IsActive,CountryId=@CountryId WHERE AreaId=@AreaId";
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    var cnt = await connection.ExecuteAsync(query, areamodel);

                    if (cnt != 1)
                    {
                        result.result = 0;
                        result.error = areamodel.AreaName + " Record failed to update.";
                        return result;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "UpdateArea"
                };
                _logging.LogError();
                result.result = 0;
                result.error = areamodel.AreaName + " Record was not updated.";
                return result;
            }
        }


        public async Task<ReturnResult> DeleteArea(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.ExecuteAsync(@"DELETE FROM Areas WHERE a.AreaId = @areaid",
                                                                                    new { areaid = id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "DeleteArea"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        private bool CheckAreaExists(AreaResult areamodel)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    return connection.ExecuteScalar<bool>(@"SELECT COUNT(1) FROM Areas WHERE LOWER(AreaName) = @areaname
                                                                  AND CountryId=@countryId",
                                                                  new { areaname = areamodel.AreaName.Trim().ToLower(), countryId = areamodel.CountryId });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AreaService",
                    ProcedureName = "CheckAreaExists"
                };
                _logging.LogError();
                return false;
            }
        }

        
    }
}
