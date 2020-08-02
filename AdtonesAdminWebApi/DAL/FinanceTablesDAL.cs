﻿using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class FinanceTablesDAL : BaseDAL, IFinanceTablesDAL
    {

        public FinanceTablesDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService) : base(configuration, executers, connService)
        { }


        public async Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetInvoiceResultSet);
            var values = CheckGeneralFile(sb, builder, pais: "camp");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY bil.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<InvoiceResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetOutstandingInvoiceResultSet);
            var values = CheckGeneralFile(sb, builder, pais: "cp");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY bil.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<OutstandingInvoiceResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdvertiserCreditResult>> LoadUserCreditResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.LoadUserCreditDataTable);
            var values = CheckGeneralFile(sb, builder, pais: "ad");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY u.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AdvertiserCreditResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<InvoicePDFEmailModel> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoicePDFEmailModel>(FinancialTablesQuery.GetInvoiceToPDF, 
                                                                                new { billingId = billingId, ucpId = UsersCreditPaymentID }));
            }
            catch
            {
                throw;
            }
        }

    }
}
