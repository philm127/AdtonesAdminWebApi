using Domain.Model;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IPermissionManagementDAL
    {
        Task<string> GetPermissionsByUserId(int userId);
        Task<int> UpdateUserPermissions(int userId, string permissions);
        Task<IEnumerable<PermissionChangeModel>> GetPermissionsByRoleId(int[] roles);
        Task<string> GetPermissionsForSelectList(int roleid);
    }
}
