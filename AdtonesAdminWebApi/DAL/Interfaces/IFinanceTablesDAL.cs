using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IFinanceTablesDAL
    {
        Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSet();
        Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceResultSet();
        Task<IEnumerable<AdvertiserCreditResult>> LoadUserCreditResultSet();
        Task<IEnumerable<CampaignCreditResult>> LoadCampaignCreditResultSet(int id = 0);
    }
}
