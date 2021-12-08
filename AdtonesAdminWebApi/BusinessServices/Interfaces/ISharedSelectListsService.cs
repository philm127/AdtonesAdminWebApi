using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices.Interfaces
{
    public interface ISharedSelectListsService
    {
        Task<ReturnResult> GetUserDetailCreditList();
        Task<ReturnResult> GetUsersnRoles();
        Task<ReturnResult> GetUsersWPermissions();
    }
}
