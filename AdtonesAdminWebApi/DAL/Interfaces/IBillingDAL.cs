using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IBillingDAL
    {
        Task<int> AddBillingRecord(BillingPaymentModel command);
        Task<InvoiceForPDF> GetInvoiceDetailsForPDF(int billingId);
        Task<int> GetCreditPeriod(int campaignId);
        Task<BillingPaymentModel> GetCampaignBillingData(int campaignId);
    }
}
