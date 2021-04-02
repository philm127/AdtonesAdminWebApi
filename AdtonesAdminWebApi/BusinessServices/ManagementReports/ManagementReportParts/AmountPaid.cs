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

    public interface IAmountPaid
    {
        Task<TotalCostCredit> GetTotalAmountPaid(ManagementReportsSearch search, int ops, string conn);
    }

    public class AmountPaid : BaseDAL, IAmountPaid
    {
        private readonly ILoggingService _logServ;
        private readonly ICalculateConvertedSpendCredit _convertedSpend;

        public AmountPaid(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                            IHttpContextAccessor httpAccessor, ICalculateConvertedSpendCredit convertedSpend)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
            _convertedSpend = convertedSpend;
        }


        public async Task<TotalCostCredit> GetTotalAmountPaid(ManagementReportsSearch search, int ops, string conn)
        {
            var dataIEnum = await GetData(search, ops, conn);
            return _convertedSpend.Calculate(dataIEnum.ToList(), search);
        }


        private async Task<IEnumerable<SpendCredit>> GetData(ManagementReportsSearch search, int ops, string conn)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(GetAmountPayment);
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
                _logServ.PageName = "AmountPaid";
                _logServ.ProcedureName = "GetData";
                await _logServ.LogError();

                throw;
            }
        }


        private static string GetAmountPayment => @"SELECT 0 AS TotalCost,SUM(ISNULL(Amount,0)) AS TotalCredit,cur.CurrencyCode 
                                                FROM UsersCreditPayment AS cp
                                                INNER JOIN Contacts AS con ON cp.UserId=con.UserId
                                                INNER JOIN Operators AS op ON op.CountryId=con.CountryId                                            
                                                INNER JOIN Currencies AS cur ON cur.CurrencyId=con.CurrencyId
                                                WHERE cp.CreatedDate BETWEEN @start AND @end AND op.OperatorId=@searchOperators ";
    }
}