using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserDashboardDAL
    {
        Task<IEnumerable<AdvertiserDashboardResult>> GetAdvertiserDashboard(string command, int operatorId=0);
        Task<IEnumerable<OperatorDashboardResult>> GetOperatorDashboard(string command);
        Task<IEnumerable<SubscriberDashboardResult>> GetSubscriberDashboard(string command);
    }
}
