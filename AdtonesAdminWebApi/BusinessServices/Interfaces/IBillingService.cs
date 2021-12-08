using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IBillingService
    {
        Task<ReturnResult> PaywithUserCredit(UserPaymentCommand model);

        Task<ReturnResult> SendInvoice(IdCollectionViewModel model);
        Task<ReturnResult> ReceivePayment(AdvertiserCreditFormCommand model);

    }
}
