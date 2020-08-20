using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Interfaces
{
    public interface IPermissionManagementDAL
    {
        Task<string> GetPermissionsByUserId(int userId);
        Task<int> UpdateUserPermissions(int userId, string permissions);
    }
}
