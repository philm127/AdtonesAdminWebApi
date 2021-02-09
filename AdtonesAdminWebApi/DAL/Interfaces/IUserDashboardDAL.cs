using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserDashboardDAL
    {
        Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboard(int operatorId=0);
        Task<IEnumerable<OperatorDashboardResult>> GetOperatorDashboard();
        Task<IEnumerable<SubscriberDashboardResult>> GetSubscriberDashboard();
        Task<IEnumerable<AdminDashboardResult>> GetAdminDashboard();
        Task<IEnumerable<AdvertiserDashboardResult>> GetSalesExecDashboard();
        Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboardForSales(int userId = 0);
        Task<IEnumerable<AdvertiserDashboardResult>> GetSalesExecForAdminDashboard();
    }
}
