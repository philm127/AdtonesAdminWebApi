using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IUserDashboardDAL
    {
        Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboard(int operatorId=0);
        Task<IEnumerable<OperatorDashboardResult>> GetOperatorDashboard();
        Task<IEnumerable<SubscriberDashboardResult>> GetSubscriberDashboard(PagingSearchClass paging, string conn);
        Task<IEnumerable<AdminDashboardResult>> GetAdminDashboard();
        Task<IEnumerable<AdvertiserDashboardResult>> GetSalesExecDashboard();
        Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboardForSales(int userId = 0);
        Task<IEnumerable<AdvertiserDashboardResult>> GetSalesExecForAdminDashboard();
    }
}
