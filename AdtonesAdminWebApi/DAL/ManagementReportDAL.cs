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
using System.Text.Json;
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
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.FromDate });
            builder.AddParameters(new { end = search.ToDate });

            if(search.country != null && search.country != 0)
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
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
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
                var _logging = new ErrorLogging()
                {
                    LogLevel = select.RawSql,
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "ManagementReportDAL",
                    ProcedureName = "GetReportDoubleInts"
                };
                _logging.LogError();

                throw;

            }
        }


        public async Task<ManRepUsers> GetManReportsForUsers(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op});
            // builder.AddParameters(new { searchOperators = search.operators.ToArray() });
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
                                conn => conn.QueryFirstOrDefault<ManRepUsers>(select.RawSql, select.Parameters, commandTimeout: 120));//, commandTimeout: 60
            }
            catch
            {
                throw;
            }
        }


        public async Task<CampaignTableManReport> GetReportPlayLengths(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op});
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
            catch
            {
                throw;
            }
        }


        public async Task<RewardsManModel> GetReportRewards(ManagementReportsSearch search, string query, int ops, string conn)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
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
                                conn => conn.QueryFirstOrDefault<RewardsManModel>(select.RawSql, select.Parameters));//, commandTimeout: 60
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query, int ops, string conn, bool exp)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            builder.AddParameters(new { start = search.FromDate });
            builder.AddParameters(new { end = search.ToDate });

            if (search.country != null && search.country > 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op");
            sb = values.Item1;
            builder = values.Item2;

            if(!exp)
                sb.Append("GROUP BY cp.CampaignProfileId,CurrencyCode,TotalCredit");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(conn,
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


        public (StringBuilder sbuild, SqlBuilder build) CheckGeneralFileManReports(StringBuilder sb, SqlBuilder builder, string pais = null, string ops = null,
                                string advs = null, string test = null)
        {
            var genFile = string.Empty;
            genFile = GetPermissionsByUserId().Result;

            List<PermissionModel> gen = JsonSerializer.Deserialize<List<PermissionModel>>(genFile);

            var page = gen.Find(u => u.pageName == "GeneralAccess");

            var els = page.elements.ToList();

            bool queryUsed = false;


            // operators plural as operator is a key word

            var testcty = els.Find(x => x.name == "country").arrayId;

            if (testcty != null)
            {
                int[] country = els.Find(x => x.name == "country").arrayId.ToArray();
                if (country.Length > 0 && pais != null)
                {
                    sb.Append($" AND {pais}.CountryId IN @country ");
                    builder.AddParameters(new { country = country.ToArray() });
                    queryUsed = true;
                }
            }

            //var testopo = els.Find(x => x.name == "operator").arrayId;
            //if (testopo != null)
            //{
            //    int[] operators = els.Find(x => x.name == "operator").arrayId.ToArray();
            //    if (operators.Length > 0 && ops != null && opid > 0 && ops.Contains(opid.ToString()))
            //    {
            //        sb.Append($" AND {ops}.OperatorId = @operators ");
            //        builder.AddParameters(new { operators = opid });
            //        queryUsed = true;
            //    }
            //}

            //var testads = els.Find(x => x.name == "advertiser").arrayId;
            //if (testads != null)
            //{
            //    int[] advertiser = els.Find(x => x.name == "advertiser").arrayId.ToArray();
            //    if (advertiser.Length > 0 && advs != null)
            //    {
            //        sb.Append($" AND {advs}.UserId IN @advertiser ");
            //        builder.AddParameters(new { advertiser = advertiser.ToArray() });
            //        queryUsed = true;
            //    }
            //}

            // If original string does not have Where clause this will add it.
            if (!sb.ToString().ToLower().Contains(" where ") && queryUsed)
            {
                var find = " and ";
                var replace = " WHERE ";
                int Place = sb.ToString().ToLower().IndexOf(find);
                string result = sb.ToString().Remove(Place, find.Length).Insert(Place, replace);
                sb.Clear();
                sb.Append(result);
            }

            return (sb, builder);
        }



    }
}
