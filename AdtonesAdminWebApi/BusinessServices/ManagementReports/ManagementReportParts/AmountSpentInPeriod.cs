using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.ManagementReports.ManagementReportParts
{
    public interface IAmountSpentInPeriod
    {
        Task<TotalCostCredit> GetTotalAmountSpentInPeriod(ManagementReportsSearch search, int ops, string conn);
    }

    public class AmountSpentInPeriod : BaseDAL, IAmountSpentInPeriod
    {
        private readonly ILoggingService _logServ;
        private readonly ICalculateConvertedSpendCredit _convertedSpend;

        public AmountSpentInPeriod(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                            IHttpContextAccessor httpAccessor, ICalculateConvertedSpendCredit convertedSpend)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
            _convertedSpend = convertedSpend;
        }


        public async Task<TotalCostCredit> GetTotalAmountSpentInPeriod(ManagementReportsSearch search, int ops, string conn)
        {
            var dataIEnum = await GetData(search, ops, conn);
            return _convertedSpend.Calculate(dataIEnum.ToList(), search);
        }


        private async Task<IEnumerable<SpendCredit>> GetData(ManagementReportsSearch search, int ops, string conn)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(GetAmountSpent);
            builder.AddParameters(new { searchOperators = op });

            if (search.country != null && search.country > 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op");
            sb = values.Item1;
            builder = values.Item2;

            sb.Append("GROUP BY cur.CurrencyCode");
            builder.AddParameters(new { start = search.FromDate });
            builder.AddParameters(new { end = search.ToDate });

            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(conn,
                                conn => conn.Query<SpendCredit>(select.RawSql, select.Parameters));
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "AmountSpentInPeriod";
                _logServ.ProcedureName = "GetData";
                await _logServ.LogError();

                throw;
            }
        }


        private static string GetAmountSpent => @"SELECT ISNULL(SUM(cp.TotalCost),0) AS TotalCost,0 AS TotalCredit,cur.CurrencyCode 
                                                    FROM CampaignProfile AS ca INNER JOIN (
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM (
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit2 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit3 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit4 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit5 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit6 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit7 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit8 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit9 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    UNION ALL
                                                    SELECT CampaignProfileId, ISNULL(SUM(TotalCost),0) AS TotalCost FROM CampaignAudit10 WHERE StartTime BETWEEN @start AND @end GROUP BY CampaignProfileId
                                                    ) as x
                                                    GROUP BY CampaignProfileId ) AS cp
                                                    ON cp.CampaignProfileId=ca.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=ca.CountryId
                                                    INNER JOIN Currencies AS cur ON op.CurrencyId=cur.CurrencyId
                                                    WHERE op.OperatorId=@searchOperators ";

    }
}
