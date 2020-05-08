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
    public class RewardsService : IRewardsService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public RewardsService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        public async Task<ReturnResult> LoadRewardsDataTable()
        {
            var select_query = @"SELECT RewardId,RewardName,CONVERT(DECIMAL(18,2),replace(RewardValue, ',', '')) AS RewardValue,
                                                    r.AddedDate,r.UpdatedDate,r.OperatorId,op.OperatorName
                                                      FROM Rewards AS r LEFT JOIN Operators AS op ON r.OperatorId=op.OperatorId
                                                      ORDER BY r.AddedDate DESC";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<RewardResult>(select_query);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardsService",
                    ProcedureName = "LoadRewardsDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetReward(IdCollectionViewModel model)
        {
            var select_query = @"SELECT RewardId,RewardName,CONVERT(DECIMAL(18,2),replace(RewardValue, ',', '')) AS RewardValue,
                                                    r.AddedDate,r.UpdatedDate,r.OperatorId,op.OperatorName
                                                      FROM Rewards AS r LEFT JOIN Operators AS op ON r.OperatorId=op.OperatorId
                                                       WHERE RewardId=@Id";

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryFirstOrDefaultAsync<RewardResult>(select_query, new { Id = model.id });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "RewardsService",
                    ProcedureName = "GetOperatorConfig"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddReward(RewardResult model)
        {
            var insert_query = @"INSERT INTO Rewards(OperatorId,RewardName,RewardValue,AddedDate,UpdatedDate)
                                        VALUES(@OperatorId,@RewardName,@RewardValue,GETDATE(),GETDATE());
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
                    PageName = "RewardsService",
                    ProcedureName = "AddReward"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateReward(RewardResult model)
        {
            var update_query = @"UPDATE Rewards SET RewardValue = @RewardValue,UpdatedDate = @UpdatedDate 
                                            WHERE RewardId = @RewardId)";

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
                    PageName = "RewardsService",
                    ProcedureName = "UpdateReward"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


    }
}