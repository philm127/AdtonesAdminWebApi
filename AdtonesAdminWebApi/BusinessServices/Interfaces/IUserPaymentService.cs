using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IUserPaymentService
    {
        Task<ReturnResult> LoadPaymentDataTable();
        Task<ReturnResult> FillCampaignDropdown();
        Task<ReturnResult> FillUserPaymentDropdown();
        Task<ReturnResult> GetOutstandingBalance(int id);
        Task<ReturnResult> GetInvoiceDetails(int id);
        Task<ReturnResult> ReceivePayment(AdvertiserCreditPaymentFormModel model);
    }
}
