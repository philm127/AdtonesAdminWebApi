using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IFinanceTablesDAL
    {
        Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSet();
        Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSetForSales(int id = 0);
        Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceResultSet();
        Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceForSalesResultSet(int id = 0);
        Task<IEnumerable<AdvertiserCreditResult>> LoadUserCreditResultSet();
        Task<IEnumerable<AdvertiserCreditResult>> LoadUserCreditForSalesResultSet(int id = 0);
        Task<IEnumerable<CampaignCreditResult>> LoadCampaignCreditResultSet(int id = 0);
    }
}
