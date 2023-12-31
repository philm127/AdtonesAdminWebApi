﻿using Domain.Model;
using Domain.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IAdvertiserFinancialDAL
    {
        Task<InvoicePDFEmailModel> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID);
        Task<int> InsertPaymentFromUser(AdvertiserCreditFormModel model);

        Task<int> AddUserCredit(AdvertiserCreditFormModel _creditmodel);
        Task<int> UpdateUserCredit(AdvertiserCreditFormModel _creditmodel);
        Task<AdvertiserCreditDetailModel> GetUserCreditDetail(int id);
        Task<IEnumerable<CreditPaymentHistory>> GetUserCreditPaymentHistory(int id);
        Task<decimal> GetCreditBalance(int id);
        Task<bool> CheckUserCreditExist(int userId);

        Task<int> UpdateCampaignCredit(CampaignCreditResult model);
        Task<int> InsertCampaignCredit(CampaignCreditResult model);

        Task<AdvertiserCreditPaymentResult> GetToPayDetails(int billingId);
        Task<decimal> GetCreditBalanceForInvoicePayment(int billingId);

        Task<int> UpdateInvoiceSettledDate(int billingId);
        Task<decimal> GetAvailableCredit(int userId);

    }
}
