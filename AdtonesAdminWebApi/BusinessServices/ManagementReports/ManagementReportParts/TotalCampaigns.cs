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
    public interface ITotalCampaigns
    {
        Task<TwoDigitsManRep> GetData(ManagementReportsSearch search, int op, string conn);
    }

    public class TotalCampaigns : BaseDAL, ITotalCampaigns
    {
        private readonly ILoggingService _logServ;

        public TotalCampaigns(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }


        public async Task<TwoDigitsManRep> GetData(ManagementReportsSearch search, int op, string conn)
        {
            TwoDigitsManRep campaignData = new TwoDigitsManRep();

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(TotalLiveCampaign);
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
                                conn => conn.QueryFirstOrDefault<TwoDigitsManRep>(select.RawSql, select.Parameters, commandTimeout: 120));//, commandTimeout: 60
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "TotalCampaigns";
                _logServ.ProcedureName = "GetData";
                await _logServ.LogError();
                return campaignData;
            }
        }

        private static string TotalLiveCampaign => @"SELECT ISNULL(SUM(CASE WHEN cp.CampaignProfileId>0 THEN 1 ELSE 0 END),0) AS TotalItem,
                                                    ISNULL(SUM(CASE WHEN CreatedDateTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumItem
                                                    FROM CampaignProfile AS cp
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                    WHERE cp.CampaignProfileId IN
                                                    (" + TotalCampaignDistinct + @")
                                                    AND op.OperatorId=@searchOperators ";

        private static string TotalCampaignDistinct => @"SELECT DISTINCT CampaignProfileId FROM (
                                                        SELECT CampaignProfileId FROM CampaignAudit
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit2
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit3
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit4
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit5
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit6
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit7
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit8
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit9
                                                            UNION ALL
                                                            SELECT CampaignProfileId FROM CampaignAudit10
                                                        ) as x";
    }
}
