using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using Domain.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BillingDAL : BaseDAL, IBillingDAL
    {


        public BillingDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        public async Task<int> AddBillingRecord(BillingPaymentModel command)
        {
            int billId = 0;
            try
            {
                billId = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(BillingQuery.InsertIntoBilling, new
                                    {
                                        UserId = command.AdvertiserId,
                                        CampaignProfileId = command.CampaignProfileId,
                                        PaymentMethodId = command.PaymentMethodId,
                                        Status = command.Status,
                                        TaxPercantage = command.TaxPercantage,
                                        InvoiceNumber = command.InvoiceNumber,
                                        PONumber = command.PONumber,
                                        Fundamount = command.Fundamount,
                                        TotalAmount = command.TotalAmount,
                                        SettledDate = command.SettledDate,
                                        CurrencyCode = command.CurrencyCode,
                                        AdtoneServerBillingId = command.AdtoneServerBillingId
                                    }));

                var strLst = await _connService.GetConnectionStringsByUserId(command.AdvertiserId);
                if (strLst != null && strLst.Count > 0)
                {
                    try
                    {
                        foreach (var constr in strLst)
                        {
                            var campId = await _connService.GetCampaignProfileIdFromAdtoneIdByConn(command.CampaignProfileId, constr);

                            var userId = await _executers.ExecuteCommand(constr,
                                                                        conn => conn.ExecuteScalar<int>("SELECT UserId FROM Users WHERE AdtoneServerUserId=@Id",
                                                                                                            new { Id = command.AdvertiserId }));

                            var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(BillingQuery.InsertIntoBilling, new
                                        {
                                            UserId = userId,
                                            CampaignProfileId = campId,
                                            PaymentMethodId = command.PaymentMethodId,
                                            Status = command.Status,
                                            InvoiceNumber = command.InvoiceNumber,
                                            PONumber = command.PONumber,
                                            Fundamount = command.Fundamount,
                                            TaxPercantage = command.TaxPercantage,
                                            TotalAmount = command.TotalAmount,
                                            SettledDate = command.SettledDate,
                                            CurrencyCode = command.CurrencyCode,
                                            AdtoneServerBillingId = billId
                                        }));
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
                throw;
            }

            return billId;
        }


        public async Task<InvoiceForPDF> GetInvoiceDetailsForPDF(int billingId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoiceForPDF>(BillingQuery.GetInvoiceDetailsForPDF,
                                                                                new { Id = billingId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> GetCreditPeriod(int campaignId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<int>(BillingQuery.GetCreditPeriod,
                                                                                new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<BillingPaymentModel> GetCampaignBillingData(int campaignId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<BillingPaymentModel>(BillingQuery.GetCampaignBillingData,
                                                                                new { Id = campaignId }));
            }
            catch
            {
                throw;
            }
        }
    }
}
