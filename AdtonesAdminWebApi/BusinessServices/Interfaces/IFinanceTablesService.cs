using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IFinanceTablesService
    {
        Task<ReturnResult> LoadInvoiceDataTable();
        Task<ReturnResult> LoadInvoiceDataTableForSales(int id = 0);
        Task<ReturnResult> LoadOutstandingInvoiceDataTable();
        Task<ReturnResult> LoadOutstandingInvoiceForSalesDataTable(int id = 0);
        Task<ReturnResult> LoadUserCreditDataTable();
        Task<ReturnResult> LoadUserCreditDataTableForSales(int id = 0);
        Task<ReturnResult> LoadCampaignCreditPeriodTable(int id = 0);
    }
}
