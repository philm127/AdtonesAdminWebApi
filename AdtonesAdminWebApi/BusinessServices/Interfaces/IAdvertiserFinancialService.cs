using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertiserFinancialService
    {
        // Task<ReturnResult> GetOutstandingBalance(int id);
        // Task<ReturnResult> GetInvoiceDetails(int id);
        Task<ReturnResult> SendInvoice(IdCollectionViewModel model);
        Task<ReturnResult> ReceivePayment(AdvertiserCreditFormModel model);

        Task<ReturnResult> AddCredit(AdvertiserCreditFormModel _usercredit);
        Task<ReturnResult> GetCreditDetails(int id);
        Task<ReturnResult> AddCampaignCredit(CampaignCreditResult model);
        Task<ReturnResult> UpdateCampaignCredit(CampaignCreditResult model);
        Task<ReturnResult> GetToPayDetails(int billingId);
    }
}
