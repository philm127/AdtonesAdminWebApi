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

    public interface ITotalListened
    {
        Task<TwoDigitsManRep> GetData(ManagementReportsSearch search, int op, string conn);
    }

    public class TotalListened : BaseDAL, ITotalListened
    {
        private readonly ILoggingService _logServ;

        public TotalListened(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }


        public async Task<TwoDigitsManRep> GetData(ManagementReportsSearch search, int op, string conn)
        {
            TwoDigitsManRep listenData = new TwoDigitsManRep();

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(TotalUsersListened);
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
                                conn => conn.QueryFirstOrDefault<TwoDigitsManRep>(select.RawSql, select.Parameters, commandTimeout: 120));
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "TotalListened";
                _logServ.ProcedureName = "GetData";
                await _logServ.LogError();
                return listenData;
            }
        }


        private static string TotalUsersListened => @"SELECT COUNT(y.UserId) AS TotalItem,COUNT(z.UserId) AS NumItem FROM
                                                Users AS u
                                                INNER JOIN
                                                    (SELECT up.UserId FROM UserProfile AS up
	                                                    WHERE up.UserProfileId IN (SELECT DISTINCT UserProfileId FROM CampaignAudit)
                                                     ) AS y
                                                ON y.UserId=u.UserId
                                                LEFT JOIN
                                                    (SELECT up.UserId FROM UserProfile AS up
	                                                    WHERE up.UserProfileId IN (SELECT DISTINCT UserProfileId FROM CampaignAudit 
                                                                                         WHERE StartTime BETWEEN @start AND @end)
                                                        ) AS z
                                                ON z.UserId=u.UserId
                                                INNER JOIN Operators AS op ON op.OperatorId=u.OperatorId 
                                                WHERE u.RoleId=2 
                                                AND u.OperatorId=@searchOperators ";
    }
}
