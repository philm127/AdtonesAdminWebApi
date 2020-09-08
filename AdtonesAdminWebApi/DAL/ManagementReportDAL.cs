using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{
    public class ManagementReportDAL : BaseDAL, IManagementReportDAL
    {

        public ManagementReportDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }



        public async Task<int> GetreportInts(ManagementReportsSearch search, string query)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.DateFrom });
            builder.AddParameters(new { end = search.DateTo });

            if(search.country != null && search.country != 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op",ops:"op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<int>> GetAllOperators()
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<int>(ManagementReportQuery.GetAllOperators));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<string>> GetOperatorNames(ManagementReportsSearch search)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<string>(ManagementReportQuery.GetOperatorNameById, new
                                {
                                    searchOperators = search.operators.ToArray()
                                }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.DateFrom });
            builder.AddParameters(new { end = search.DateTo });

            if (search.country != null && search.country > 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op", ops: "op");
            sb = values.Item1;
            builder = values.Item2;

            sb.Append("GROUP BY ca.CampaignProfileId,CurrencyCode,TotalCredit");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<SpendCredit>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }

    }
}
