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
    public interface ITotalPlays
    {
        Task<CampaignTableManReport> GetPlayData(ManagementReportsSearch search, int ops, string conn);
    }


    public class TotalPlays : BaseDAL, ITotalPlays
    {
        private readonly ILoggingService _logServ;
        public TotalPlays(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }


        public async Task<CampaignTableManReport> GetPlayData(ManagementReportsSearch search, int ops, string conn)
        {
            CampaignTableManReport playData = new CampaignTableManReport();
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(TotalPlayStuff);
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
                                conn => conn.QueryFirstOrDefault<CampaignTableManReport>(select.RawSql, select.Parameters));//, commandTimeout: 60
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = "TotalPlays";
                _logServ.ProcedureName = "GetPlayData";
                await _logServ.LogError();
                return playData;
            }
        }


        private static string TotalPlayStuff => @"SELECT
                                                    ISNULL(SUM(TotOfPlaySixOver),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(TotOfPlayUnderSix),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(TotPlaylength),0) AS TotPlaylength,
                                                    ISNULL(SUM(TotOfSMS),0) AS TotOfSMS,
                                                    ISNULL(SUM(TotOfEmail),0) AS TotOfEmail,
                                                    ISNULL(SUM(TotCancelled),0) AS TotCancelled,
                                                    ISNULL(SUM(NumOfPlaySixOver),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(NumOfPlayUnderSix),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(Playlength),0) AS Playlength,
                                                    ISNULL(SUM(NumOfSMS),0) AS NumOfSMS,
                                                    ISNULL(SUM(NumOfEmail),0) AS NumOfEmail,
                                                    ISNULL(SUM(NumCancelled),0) AS NumCancelled
                                                    FROM (
                                                    SELECT CampaignProfileId,ISNULL(SUM(TotOfPlaySixOver),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(TotOfPlayUnderSix),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(TotPlaylength),0) AS TotPlaylength,
                                                    ISNULL(SUM(TotOfSMS),0) AS TotOfSMS,
                                                    ISNULL(SUM(TotOfEmail),0) AS TotOfEmail,
                                                    ISNULL(SUM(TotCancelled),0) AS TotCancelled,
                                                    ISNULL(SUM(NumOfPlaySixOver),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(NumOfPlayUnderSix),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(Playlength),0) AS Playlength,
                                                    ISNULL(SUM(NumOfSMS),0) AS NumOfSMS,
                                                    ISNULL(SUM(NumOfEmail),0) AS NumOfEmail,
                                                    ISNULL(SUM(NumCancelled),0) AS NumCancelled
                                                    FROM (
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit GROUP BY CampaignProfileId

                                                    UNION ALL
                                                    " + PlayStuffUnion + @"
                                                    FROM CampaignAudit2 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                    " + PlayStuffUnion + @"
                                                    FROM CampaignAudit3 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit4 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit5 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit6 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit7 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit8 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit9 GROUP BY CampaignProfileId

                                                    UNION ALL
                                                     " + PlayStuffUnion + @"
                                                    FROM CampaignAudit10 GROUP BY CampaignProfileId
                                                    ) as x
                                                    GROUP BY CampaignProfileId ) AS ca
                                                    INNER JOIN CampaignProfile AS cp ON ca.CampaignProfileId=cp.CampaignProfileId
                                                    INNER JOIN Operators AS op ON op.CountryId=cp.CountryId 
                                                    WHERE op.OperatorId=@searchOperators ";


        private static string PlayStuffUnion => @"SELECT CampaignProfileId,ISNULL(SUM(CASE WHEN PlayLengthTicks >= 6000 THEN 1 ELSE 0 END),0) AS TotOfPlaySixOver,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks < 6000 THEN 1 ELSE 0 END),0) AS TotOfPlayUnderSix,
                                                    ISNULL(SUM(ISNULL(PlayLengthTicks,0)),0) AS TotPlaylength,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND SMS IS NOT NULL) THEN 1 ELSE 0 END),0) AS TotOfSMS,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND EmailCost != 0) THEN 1 ELSE 0 END),0) AS TotOfEmail,
                                                    ISNULL(SUM(CASE WHEN Status= 'cancelled' THEN 1 ELSE 0 END),0) AS TotCancelled,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks>=6000 AND StartTime 
                                                                        BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumOfPlaySixOver,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks < 6000 AND StartTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumOfPlayUnderSix,
                                                    ISNULL(SUM(CASE WHEN PlayLengthTicks> 0 AND StartTime BETWEEN @start AND @end THEN PlayLengthTicks ELSE 0 END),0) AS Playlength,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND SMS IS NOT NULL AND StartTime BETWEEN @start AND @end) THEN 1 ELSE 0 END),0) AS NumOfSMS,
                                                    ISNULL(SUM(CASE WHEN (PlayLengthTicks >= 6000 AND EmailCost != 0 AND StartTime BETWEEN @start AND @end) THEN 1 ELSE 0 END),0) AS NumOfEmail,
                                                    ISNULL(SUM(CASE WHEN Status= 'cancelled' AND StartTime BETWEEN @start AND @end THEN 1 ELSE 0 END),0) AS NumCancelled";


    }
}
