using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IBillingDAL
    {
        Task<int> AddBillingRecord(UserPaymentCommand command);
        Task<InvoiceForPDFDto> GetInvoiceDetailsForPDF(int billingId);
        Task<int> GetCreditPeriod(int campaignId);
        Task<BillingPaymentDto> GetCampaignBillingData(int campaignId);
        Task<BillingPaymentDto> GetAdvertiserBillingData(int userId);

        Task<InvoicePDFEmailDto> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID);
        Task<int> InsertPaymentFromUser(AdvertiserCreditFormCommand model);

        
        Task<IEnumerable<CreditPaymentHistoryDto>> GetUserCreditPaymentHistory(int id);
        Task<decimal> GetCreditBalance(int id);

        Task<int> UpdateCampaignCreditPeriod(CampaignCreditPeriodCommand model);
        Task<int> InsertCampaignCreditPeriod(CampaignCreditPeriodCommand model);

        Task<AdvertiserCreditPaymentDto> GetToPayDetails(int billingId);
        Task<decimal> GetCreditBalanceForInvoicePayment(int billingId);

        Task<int> UpdateInvoiceSettledDate(int billingId);
        
    }
}
