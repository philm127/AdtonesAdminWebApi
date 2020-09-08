using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IAdvertiserPaymentDAL
    {
        Task<InvoicePDFEmailModel> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID);
        Task<int> InsertPaymentFromUser(AdvertiserCreditFormModel model);

    }
}
