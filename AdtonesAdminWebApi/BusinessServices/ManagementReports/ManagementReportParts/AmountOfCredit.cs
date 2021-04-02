using AdtonesAdminWebApi.DAL;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
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

    public interface IAmountOfCredit
    {
        Task<TotalCostCredit> GetTotalAmountOfCredit(ManagementReportsSearch search, int ops, string conn);
        Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currency);
        string GetCurrencySymbol(string currencyId);
    }

    public class AmountOfCredit : BaseDAL, IAmountOfCredit
    {
        private readonly ILoggingService _logServ;
        private readonly ICalculateConvertedSpendCredit _convertedSpend;

        public AmountOfCredit(ILoggingService logServ, IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService,
                            IHttpContextAccessor httpAccessor, ICalculateConvertedSpendCredit convertedSpend)
            : base(configuration, executers, connService, httpAccessor)
        {
            _logServ = logServ;
            _convertedSpend = convertedSpend;
        }


        public async Task<TotalCostCredit> GetTotalAmountOfCredit(ManagementReportsSearch search, int ops, string conn)
        {
            var dataIEnum = await GetData(search, ops, conn);
            return _convertedSpend.Calculate(dataIEnum.ToList(), search);
        }


        public async Task<Currency> GetCurrencyUsingCurrencyIdAsync(int? currency)
        {
            return await _convertedSpend.GetCurrencyUsingCurrencyIdAsync(currency);
        }


        public string GetCurrencySymbol(string currencyId)
        {
            return _convertedSpend.GetCurrencySymbol(currencyId);
        }


        private async Task<IEnumerable<SpendCredit>> GetData(ManagementReportsSearch search, int ops, string conn)
        {
            var op = await _connService.GetOperatorIdFromAdtoneId(ops);
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(GetAmountCredit);
            builder.AddParameters(new { searchOperators = op });

            if (search.country != null && search.country > 0)
            {
                sb.Append(" AND op.CountryId = @country ");
                builder.AddParameters(new { country = search.country });
            }
            var values = CheckGeneralFile(sb, builder, pais: "op");
            sb = values.Item1;
            builder = values.Item2;

            sb.Append("GROUP BY CurrencyCode");

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
                _logServ.PageName = "AmountOfCredit";
                _logServ.ProcedureName = "GetData";
                await _logServ.LogError();

                throw;
            }
        }


        private static string GetAmountCredit => @"SELECT 0 AS TotalCost,SUM(ISNULL(TotalCredit,0)) AS TotalCredit,CurrencyCode 
                                                FROM CampaignProfile AS cp
                                                INNER JOIN Operators AS op ON op.CountryId=cp.CountryId
                                                WHERE op.OperatorId=@searchOperators ";
    }
}
