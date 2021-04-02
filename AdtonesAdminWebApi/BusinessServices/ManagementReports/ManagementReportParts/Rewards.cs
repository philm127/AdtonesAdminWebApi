using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports.ManagementReportParts
{
    public interface IRewards
    {
        Task<RewardsManModel> GetRewardData(ManagementReportsSearch search, int ops, string conn);
    }

    public class Rewards : BaseDAL, IRewards
    {
        private readonly ILoggingService _logServ;
        public Rewards(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }


        public async Task<RewardsManModel> GetRewardData(ManagementReportsSearch search, int ops, string conn)
        {
            string rewardQuery = string.Empty;
            rewardQuery = ops == 1 ? TotalSafRewardsQuery : TotalExpRewardsQuery;
            RewardsManModel totRewards = new RewardsManModel();

            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(rewardQuery);
            builder.AddParameters(new { searchOperators = op });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.FromDate });
            builder.AddParameters(new { end = search.ToDate });

            if (search.country != null && search.country != 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }

            var values = CheckGeneralFile(sb, builder, pais: "op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(conn,
                                conn => conn.QueryFirstOrDefault<RewardsManModel>(select.RawSql, select.Parameters));
            }
            catch(Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "Rewards";
                _logServ.ProcedureName = "GetRewardData";
                await _logServ.LogError();
                return totRewards;
            }
            
        }


        private static string TotalSafRewardsQuery => @"SELECT SUM(x.IsRewardReceived) AS IsRewardReceivedTot,COUNT(DISTINCT x.UserProfileId) as UserProfileIdTot,
                                                    SUM(y.IsRewardReceived) AS IsRewardReceivedNum,COUNT(DISTINCT y.UserProfileId) as UserProfileIdNum
                                                    FROM UserProfile AS p
                                                    LEFT JOIN (
                                                                SELECT SUM(CAST(IsRewardReceived as integer)) AS IsRewardReceived,UserProfileId
                                                                FROM UserProfileAdvertsReceiveds WHERE IsRewardReceived=1 GROUP BY UserProfileId) AS x
												    ON x.UserProfileId=p.UserProfileId
                                                    LEFT JOIN (
                                                                SELECT SUM(CAST(IsRewardReceived as integer)) AS IsRewardReceived,UserProfileId
                                                                FROM UserProfileAdvertsReceiveds WHERE DateTimePlayed BETWEEN @start AND @end
                                                                    AND IsRewardReceived=1
												                    GROUP BY UserProfileId) AS y
                                                    ON y.UserProfileId=p.UserProfileId
                                                    INNER JOIN Users u ON u.UserId=p.UserId
                                                    INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                    WHERE u.OperatorId=@searchOperators ";



        private static string TotalExpRewardsQuery => @"SELECT ISNULL(SUM(x.TotalItem),0) AS IsRewardReceivedTot,COUNT(DISTINCT x.UserId) as UserProfileIdTot,
                                                   ISNULL(SUM(y.TotalItem),0) AS IsRewardReceivedNum,COUNT(DISTINCT y.UserId) as UserProfileIdNum
                                                    FROM Users As u
                                                    LEFT JOIN 
                                                        ( SELECT COUNT(ClaimRewardAuditId) AS TotalItem,UserId
                                                          FROM ClaimRewardAudit GROUP BY UserId) AS x
                                                    ON u.UserId=x.UserId
                                                    LEFT JOIN 
                                                        ( SELECT COUNT(ClaimRewardAuditId) AS TotalItem,UserId
                                                            FROM ClaimRewardAudit WHERE EntryDateTimeUtc BETWEEN @start AND @end 
                                                            GROUP BY UserId) AS y
                                                    ON u.UserId=y.UserId
                                                    INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId
                                                    WHERE u.OperatorId=@searchOperators ";

    }
}
