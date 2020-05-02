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
        Task<ReturnResult> GetCountryList();
        ReturnResult GetUserRole();
        ReturnResult GetUserStatus();
        Task<ReturnResult> GetOperatorList(int countryId = 0);
        Task<ReturnResult> GetCurrencyList(int currencyId = 0);
        Task<ReturnResult> GetUserById(int userId);
    }
}
