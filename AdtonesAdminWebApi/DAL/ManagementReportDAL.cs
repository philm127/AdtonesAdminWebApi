using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
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

namespace AdtonesAdminWebApi.DAL
{
    public class ManagementReportDAL : BaseDAL, IManagementReportDAL
    {

        public ManagementReportDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }



        public async Task<int> GetreportInts(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = new int[1];
            op[0] = ops;
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op.ToArray() });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
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
                                conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters, commandTimeout: 120));//, commandTimeout: 60
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = query,
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportDAL",
                    ProcedureName = "GetReportInts"
                };
                _logging.LogError();

                throw;

            }
        }


        public async Task<TwoDigitsManRep> GetreportDoubleInts(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = new int[1];
            op[0] = ops;
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op.ToArray() });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.DateFrom });
            builder.AddParameters(new { end = search.DateTo });

            if (search.country != null && search.country != 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }

            var values = CheckGeneralFile(sb, builder, pais: "op", ops: "op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<TwoDigitsManRep>(select.RawSql, select.Parameters, commandTimeout: 120));//, commandTimeout: 60
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = query,
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportDAL",
                    ProcedureName = "GetReportInts"
                };
                _logging.LogError();

                throw;

            }
        }


        public async Task<ManRepUsers> GetManReportsForUsers(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = new int[1];
            op[0] = ops;
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op.ToArray() });
            // builder.AddParameters(new { searchOperators = search.operators.ToArray() });
             builder.AddParameters(new { start = search.DateFrom });
             builder.AddParameters(new { end = search.DateTo });

            if (search.country != null && search.country != 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }

            var values = CheckGeneralFile(sb, builder, pais: "op", ops: "op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<ManRepUsers>(select.RawSql, select.Parameters, commandTimeout: 120));//, commandTimeout: 60
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignTableManReport> GetReportPlayLengths(ManagementReportsSearch search, string query, int ops, string conn)
        {
            int[] op = new int[1];
            op[0] = ops;// search.singleOperator.ToString().ToCharArray().Select(Convert.ToInt32).ToArray();
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op.ToArray() });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.DateFrom });
            builder.AddParameters(new { end = search.DateTo });

            if (search.country != null && search.country != 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }

            var values = CheckGeneralFile(sb, builder, pais: "op", ops: "op");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<CampaignTableManReport>(select.RawSql, select.Parameters));//, commandTimeout: 60
            }
            catch
            {
                throw;
            }
        }
        
        public async Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = new int[1];
            op[0] = ops;
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op.ToArray() });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
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

            sb.Append("GROUP BY cp.CampaignProfileId,CurrencyCode,TotalCredit");
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



    }
}
