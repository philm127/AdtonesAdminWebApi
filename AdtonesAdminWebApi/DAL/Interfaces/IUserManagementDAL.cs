using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IUserManagementDAL
    {
        Task<int> UpdateUserStatus(string command, AdvertiserDashboardResult model);
        Task<User> GetUserById(string command, int id);
        Task<int> UpdateCorpUser(string command, int userId);

    }
}
