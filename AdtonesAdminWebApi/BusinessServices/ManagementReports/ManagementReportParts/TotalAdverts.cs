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

    public interface ITotalAdverts
    {
        Task<TwoDigitsManRep> GetData(ManagementReportsSearch search, int op, string conn);
    }

    public class TotalAdverts : BaseDAL, ITotalAdverts
    {
        private readonly ILoggingService _logServ;

        public TotalAdverts(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
        }


        public async Task<TwoDigitsManRep> GetData(ManagementReportsSearch search, int op, string conn)
        {
            TwoDigitsManRep advertData = new TwoDigitsManRep();

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(TotalAdsProvisioned);
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
                _logServ.PageName = "TotalAdverts";
                _logServ.ProcedureName = "GetData";
                await _logServ.LogError();
                return advertData;
            }
        }


        private static string TotalAdsProvisioned => @"SELECT ISNULL(SUM(CASE WHEN ad.AdvertId>0 THEN 1 ELSE 0 END),0) AS TotalItem,
                                                        ISNULL(SUM(CASE WHEN CreatedDateTime BETWEEN @start AND @end 
                                                                                THEN 1 ELSE 0 END),0) AS NumItem 
                                                        FROM Advert AS ad INNER JOIN CampaignAdverts AS cad ON cad.advertid=ad.AdvertId
                                                        INNER JOIN Operators AS op ON op.OperatorId=ad.OperatorId
                                                        WHERE cad.CampaignProfileId IN (SELECT DISTINCT CampaignProfileId from CampaignAudit)
                                                        AND ad.OperatorId=@searchOperators ";
    }
}
