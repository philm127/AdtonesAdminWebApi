using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MimeKit.Encodings;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class RewardDAL : BaseDAL, IRewardDAL
    {

        public RewardDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor) 
            : base(configuration, executers, connService, httpAccessor)
        {
        }


        public async Task<IEnumerable<RewardResult>> LoadRewardResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(RewardQuery.LoadRewardsDataTable);
            var values = CheckGeneralFile(sb, builder,pais:"op",ops:"r");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY r.AddedDate DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<RewardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<RewardResult> GetRewardById(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(RewardQuery.GetRewardDetail);
            builder.AddParameters(new { Id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<RewardResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> DeleteRewardById(int id)
        {

            var rewardDetail = await GetRewardById(id);
            var conns = await _connService.GetConnectionStringByOperator(rewardDetail.OperatorId);

            var sb = new StringBuilder();
            var sbOp = new StringBuilder();
            sb.Append(RewardQuery.DeleteReward);
            sbOp.Append(RewardQuery.DeleteReward);

            sb.Append(" WHERE RewardId=@Id");
            sbOp.Append(" WHERE AdtoneServerRewardId=@Id");

            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { Id = id }));

                return await _executers.ExecuteCommand(conns,
                                conn => conn.ExecuteScalar<int>(sbOp.ToString(), new { Id = id }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateReward(RewardResult model)
        {
            var rewardDetail = await GetRewardById(model.RewardId);
            var conns = await _connService.GetConnectionStringByOperator(rewardDetail.OperatorId);

            var sb = new StringBuilder();
            var sbOp = new StringBuilder();
            sb.Append(RewardQuery.UpdateReward);
            sbOp.Append(RewardQuery.UpdateReward);

            sb.Append(" WHERE RewardId=@Id");
            sbOp.Append(" WHERE AdtoneServerRewardId=@Id");

            try
            {
                int x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { Id = model.RewardId, RewardValue = model.RewardValue }));

                if (conns != null && conns.Length > 0)
                    return await _executers.ExecuteCommand(conns,
                                conn => conn.ExecuteScalar<int>(sbOp.ToString(), new { Id = model.RewardId, RewardValue = model.RewardValue }));

                return x;
            }
            catch
            {
                throw;
            }

        }


        public async Task<int> AddReward(RewardResult model)
        {
            var conns = await _connService.GetConnectionStringByOperator(model.OperatorId);

            var sb = new StringBuilder();
            var sbOp = new StringBuilder();
            sb.Append(RewardQuery.AddReward);
            sbOp.Append(RewardQuery.AddReward);

            try 
            {
                model.AdtoneServerRewardId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { OperatorId = model.OperatorId, RewardName = model.RewardName, RewardValue = model.RewardValue, AdtoneServerRewardId = model.AdtoneServerRewardId }));

                model.OperatorId = await _connService.GetOperatorIdFromAdtoneId(model.OperatorId, model.OperatorId);

                return await _executers.ExecuteCommand(conns,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { OperatorId = model.OperatorId, RewardName = model.RewardName, RewardValue = model.RewardValue, AdtoneServerRewardId = model.AdtoneServerRewardId }));
            }
            catch
            {
                throw;
            }
        }


        
    }
}
