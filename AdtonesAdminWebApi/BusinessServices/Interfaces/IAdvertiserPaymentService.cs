using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IAdvertiserPaymentService
    {
        Task<ReturnResult> GetOutstandingBalance(int id);
        Task<ReturnResult> GetInvoiceDetails(int id);
        Task<ReturnResult> SendInvoice(IdCollectionViewModel model);
        Task<ReturnResult> ReceivePayment(AdvertiserCreditFormModel model);
    }
}
