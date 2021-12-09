using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IFinanceTablesDAL
    {
        Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSet();
        Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSetForSales(int id = 0);
        Task<IEnumerable<AdvertiserBillingResultDto>> LoadAdvertiserBillingDetails(int id);
        Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceResultSet();
        Task<IEnumerable<OutstandingInvoiceResult>> LoadOutstandingInvoiceForSalesResultSet(int id = 0);
        Task<IEnumerable<InvoiceResult>> LoadInvoiceResultSetForAdvertiser(int id);
        Task<IEnumerable<AdvertiserCreditResultDto>> LoadUserCreditResultSet();
        Task<IEnumerable<AdvertiserCreditResultDto>> LoadUserCreditForSalesResultSet(int id = 0);
        Task<IEnumerable<CampaignCreditResult>> LoadCampaignCreditResultSet(int id = 0);
    }
}
