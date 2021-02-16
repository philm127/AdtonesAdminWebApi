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
        private readonly ILoggingService _logServ;
        const string PageName = "ManagementReportDAL";

        public ManagementReportDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor,
                                    ILoggingService logServe)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServe;
        }



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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetReportInts";
                await _logServ.LogError();
                
                throw;

            }
        }

        /// <summary>
        /// For brevity gets two intergers of varying values depending on the SELECT script that is passed in
        /// </summary>
        /// <param name="search">The search based parameters</param>
        /// <param name="query">The SQL SELECT script to be used</param>
        /// <param name="ops">The operator Id</param>
        /// <param name="conn">The connection string to be used</param>
        /// <param name="useOpId">If is the main connection string uses the operator Id as passed otherwise will get the operator Id from the relevant connection string</param>
        /// <returns></returns>
        public async Task<TwoDigitsManRep> GetreportDoubleInts(ManagementReportsSearch search, string query, int ops, string conn, bool useOpId = false)
        {
            int op = 0;
            if (useOpId)
                op = ops;
            else
                op = await _connService.GetOperatorIdFromAdtoneId(ops);

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
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetReportDoubleInts";
                await _logServ.LogError();
                
                throw;

            }
        }


        public async Task<ManRepUsers> GetManReportsForUsers(ManagementReportsSearch search, string query, int ops, string conn)
        {
            
            //var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = ops});
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


        public async Task<IEnumerable<SpendCredit>> GetTotalCreditCost(ManagementReportsSearch search, string query, int op, string conn, int exp)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });
            

            if (search.country != null && search.country > 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op");
            sb = values.Item1;
            builder = values.Item2;

            if (exp == 1)
            {
                sb.Append("GROUP BY cur.CurrencyCode");
                builder.AddParameters(new { start = search.FromDate });
                builder.AddParameters(new { end = search.ToDate });

            }
            else if (exp == 2)
            {
                sb.Append("GROUP BY CurrencyCode");
                
            }
            else if (exp == 3)
            {
                sb.Append("GROUP BY cur.CurrencyCode");
                builder.AddParameters(new { start = "2010-01-01" });
                builder.AddParameters(new { end = "2030-01-01" });
            }
            // sb.Append("GROUP BY cp.CampaignProfileId,CurrencyCode,TotalCredit");
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
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTotalCreditCost";
                await _logServ.LogError();
                
                throw;
            }
        }


        public async Task<IEnumerable<SpendCredit>> GetTotalCreditCostProv(ManagementReportsSearch search, string query, int ops, string conn, int exp)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(query);
            builder.AddParameters(new { searchOperators = op });
            //builder.AddParameters(new { searchOperators = search.operators.ToArray() });


            if (search.country != null && search.country > 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op");
            sb = values.Item1;
            builder = values.Item2;

            if (exp == 1)
            {
                sb.Append("GROUP BY cur.CurrencyCode");
                builder.AddParameters(new { start = search.FromDate });
                builder.AddParameters(new { end = search.ToDate });

            }
            else if (exp == 2)
            {
                sb.Append("GROUP BY CurrencyCode");

            }
            else if (exp == 3)
            {
                sb.Append("GROUP BY cur.CurrencyCode");
                builder.AddParameters(new { start = "2010-01-01" });
                builder.AddParameters(new { end = "2030-01-01" });
            }
            // sb.Append("GROUP BY cp.CampaignProfileId,CurrencyCode,TotalCredit");
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
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetTotalCreditCostProv";
                await _logServ.LogError();
               
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
