using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface IPermissionManagementService
    {
        Task<ReturnResult> GetPermissionsByUser(int id);
    }
}
