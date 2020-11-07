using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
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

        public FinanceTablesDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor) : 
                                                base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetInvoiceResultSet);
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
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


        public async Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSetForSales(int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetInvoiceForSalesResultSet);
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
            if (id == 0)
            {
                var values = CheckGeneralFile(sb, builder, pais: "con");
                sb = values.Item1;
                builder = values.Item2;
            }
            else
            {
                sb.Append(" AND sales.IsActive=1 ");
                sb.Append(" AND sales.SalesExecId=@Sid ");
                builder.AddParameters(new { Sid = id });
            }
            sb.Append(" ORDER BY bil.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());


                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<InvoiceResult>(select.RawSql, select.Parameters));
        }


        public async Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceResultSet()
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetOutstandingInvoiceResultSet);
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
            var values = CheckGeneralFile(sb, builder, pais: "cp",advs:"bil");
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


        public async Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceForSalesResultSet(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetOutstandingInvoiceForSalesResultSet);
            builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress") });
            if (id == 0)
            {
                var values = CheckGeneralFile(sb, builder, pais: "con");
                sb = values.Item1;
                builder = values.Item2;
            }
            else
            {
                sb.Append(" AND sales.IsActive=1 ");
                sb.Append(" AND sales.SalesExecId=@Sid ");
                builder.AddParameters(new { Sid = id });
            }
            sb.Append(" ORDER BY bil.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            return await _executers.ExecuteCommand(_connStr,
                            conn => conn.Query<OutstandingInvoiceResult>(select.RawSql, select.Parameters));
        }


        public async Task<IEnumerable<CampaignCreditResult>> LoadCampaignCreditResultSet(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.GetCampaignCreditPeriodData);
            if (id > 0)
            {
                sb.Append(" WHERE CampaignCreditPeriodId=@Id ");
                builder.AddParameters(new { Id = id });
            }
            var values = CheckGeneralFile(sb, builder, pais: "con", ops:"op", advs:"ccp");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY ccp.CreatedDate DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignCreditResult>(select.RawSql, select.Parameters));
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
            sb.Append(" WHERE 1=1 ");
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


        public async Task<IEnumerable<AdvertiserCreditResult>> LoadUserCreditForSalesResultSet(int id = 0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(FinancialTablesQuery.LoadUserCreditForSalesDataTable);
            if (id == 0)
            {
                sb.Append(" WHERE 1=1 ");
                var values = CheckGeneralFile(sb, builder, pais: "ad");
                sb = values.Item1;
                builder = values.Item2;
            }
            else
            {
                sb.Append(" WHERE sales.IsActive=1 ");
                sb.Append(" AND sales.SalesExecId=@Sid ");
                builder.AddParameters(new { Sid = id });
            }
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
