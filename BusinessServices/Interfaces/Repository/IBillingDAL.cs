using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IBillingDAL
    {
        Task<int> AddBillingRecord(BillingPaymentModel command);
        Task<InvoiceForPDF> GetInvoiceDetailsForPDF(int billingId);
        Task<int> GetCreditPeriod(int campaignId);
        Task<BillingPaymentModel> GetCampaignBillingData(int campaignId);
    }
}
