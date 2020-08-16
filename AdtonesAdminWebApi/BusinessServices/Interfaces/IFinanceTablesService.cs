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
        Task<ReturnResult> LoadOutstandingInvoiceDataTable();
        Task<ReturnResult> LoadUserCreditDataTable();
        Task<ReturnResult> LoadCampaignCreditPeriodTable(int id = 0);
    }
}
