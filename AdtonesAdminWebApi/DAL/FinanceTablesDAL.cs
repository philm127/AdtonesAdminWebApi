﻿using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.DTOs;
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


        public async Task<IEnumerable<AdvertiserCreditResultDto>> LoadUserCreditResultSet()
        {
            string LoadUserCreditDataTable = @"SELECT u.Id,u.UserId,usrs.Email,usrs.FullName,usrs.Organisation,u.CreatedDate,bil.CurrencyCode,
                                                 u.AssignCredit AS Credit,u.AvailableCredit,ISNULL(bil.TotalAmount,0) AS TotalUsed,ISNULL(pay.Amount,0) AS TotalPaid,
                                                 (ISNULL(bil.TotalAmount,0) - ISNULL(pay.Amount,0)) AS RemainingAmount,ISNULL(ctry.Name,'None') AS CountryName
                                                 FROM UsersCredit AS u
                                                 LEFT JOIN 
                                                 (SELECT UserId,SUM(TotalAmount) AS TotalAmount,CurrencyCode FROM Billing WHERE PaymentMethodId=1 GROUP BY UserId,CurrencyCode) bil
                                                 ON u.UserId=bil.UserId
                                                 LEFT JOIN
                                                 (SELECT UserId,SUM(Amount) AS Amount from UsersCreditPayment GROUP BY UserId) pay
                                                 ON u.UserId=pay.UserId
                                                 LEFT JOIN
                                                 (SELECT UserId,Email,CONCAT(FirstName,' ',LastName) AS FullName,Organisation FROM Users) usrs
                                                 ON usrs.UserId=u.UserId
                                                LEFT JOIN Currencies AS ad ON u.CurrencyId=ad.CurrencyId
                                                LEFT JOIN Country AS ctry ON ctry.Id=ad.CountryId";
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(LoadUserCreditDataTable);
            sb.Append(" WHERE 1=1 ");
            var values = CheckGeneralFile(sb, builder, pais: "ad");
            sb = values.Item1;
            builder = values.Item2;
            sb.Append(" ORDER BY u.Id DESC;");
            var select = builder.AddTemplate(sb.ToString());

            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AdvertiserCreditResultDto>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdvertiserCreditResultDto>> LoadUserCreditForSalesResultSet(int id = 0)
        {
            string LoadUserCreditForSalesDataTable = @"SELECT u.Id,u.UserId,usrs.Email,usrs.FullName,usrs.Organisation,u.CreatedDate,bil.CurrencyCode,
                                                 u.AssignCredit AS Credit,u.AvailableCredit,ISNULL(bil.TotalAmount,0) AS TotalUsed,ISNULL(pay.Amount,0) AS TotalPaid,
                                                CASE WHEN sexcs.FirstName IS NULL THEN 'UnAllocated' 
                                                    ELSE CONCAT(sexcs.FirstName,' ',sexcs.LastName) END AS SalesExec,sexcs.UserId AS SUserId,s
                                                 (ISNULL(bil.TotalAmount,0) - ISNULL(pay.Amount,0)) AS RemainingAmount,ISNULL(ctry.Name,'None') AS CountryName
                                                 FROM UsersCredit AS u
                                                 LEFT JOIN 
                                                 (SELECT UserId,SUM(TotalAmount) AS TotalAmount,CurrencyCode FROM Billing WHERE PaymentMethodId=1 GROUP BY UserId,CurrencyCode) bil
                                                 ON u.UserId=bil.UserId
                                                 LEFT JOIN
                                                 (SELECT UserId,SUM(Amount) AS Amount from UsersCreditPayment GROUP BY UserId) pay
                                                 ON u.UserId=pay.UserId
                                                 LEFT JOIN
                                                 (SELECT UserId,Email,CONCAT(FirstName,' ',LastName) AS FullName,Organisation FROM Users) usrs
                                                 ON usrs.UserId=u.UserId
                                                LEFT JOIN Currencies AS ad ON u.CurrencyId=ad.CurrencyId
                                                LEFT JOIN Country AS ctry ON ctry.Id=ad.CountryId
                                                LEFT JOIN Advertisers_SalesTeam AS sales ON usrs.UserId=sales.AdvertiserId 
                                                LEFT JOIN Users AS sexcs ON sexcs.UserId=sales.SalesExecId";
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(LoadUserCreditForSalesDataTable);
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
                                conn => conn.Query<AdvertiserCreditResultDto>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<AdvertiserBillingResultDto>> LoadAdvertiserBillingDetails(int id)
        {
            string GetOutstandingInvoiceForSalesResultSet = @"SELECT bil.Id AS BillingId,bil.UserId,bil.InvoiceNumber,bil.PONumber,
                                                                CONCAT(@siteAddress,'/Invoice/Adtones_invoice_',bil.InvoiceNumber,'.pdf') AS InvoicePath,
                                                                ISNULL(cl.Name, '-') AS ClientName,bil.CampaignProfileId,cp.CampaignName,
                                                                bil.PaymentDate AS CreatedDate,bil.TotalAmount,bil.Status,bil.SettledDate,
                                                                bil.PaymentMethodId,pay.Name AS PaymentMethod,bil.CurrencyCode
                                                                FROM Billing AS bil LEFT JOIN Client AS cl ON cl.Id=bil.ClientId
                                                                INNER JOIN CampaignProfile AS cp ON cp.CampaignProfileId=bil.CampaignProfileId
                                                                INNER JOIN PaymentMethod AS pay ON pay.Id=bil.PaymentMethodId
                                                                LEFT JOIN Contacts AS con ON bil.UserId=con.UserId WHERE bil.UserId=@Id ORDER BY u.Id DESC;";
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<AdvertiserBillingResultDto>(GetOutstandingInvoiceForSalesResultSet,
                                                                new
                                                                {
                                                                    siteAddress = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress"),
                                                                    id = id
                                                                }
                                                                ));
            }
            catch
            {
                throw;
            }
        }


        public async Task<InvoicePDFEmailDto> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoicePDFEmailDto>(FinancialTablesQuery.GetInvoiceToPDF, 
                                                                                new { billingId = billingId, ucpId = UsersCreditPaymentID }));
            }
            catch
            {
                throw;
            }
        }

    }
}
