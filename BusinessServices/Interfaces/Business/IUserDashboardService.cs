using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IUserDashboardService
    {
        Task<ReturnResult> LoadAdvertiserDataTable();
        Task<ReturnResult> LoadOperatorDataTable();
        Task<ReturnResult> LoadSubscriberDataTable(PagingSearchClass paging);
        Task<ReturnResult> LoadAdminDataTable();
        Task<ReturnResult> LoadSalesExecDataTable();
        Task<ReturnResult> LoadAdvertisersForSales(int userId);
        Task<ReturnResult> LoadSalesExecForAdminDataTable();
    }
}
