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
        Task<ReturnResult> GetOutstandingBalance(IdCollectionViewModel model);
        Task<ReturnResult> GetInvoiceDetails(IdCollectionViewModel model);
        Task<ReturnResult> ReceivePayment(UserCreditPaymentFormModel model);
    }
}
