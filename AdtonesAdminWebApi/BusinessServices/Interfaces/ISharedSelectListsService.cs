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
        Task<ReturnResult> GetAddCreditUsersList();
        Task<ReturnResult> GetOrganisationTypeDropDown();
        Task<ReturnResult> GetUserCreditList();
        Task<ReturnResult> GetUserDetailCreditList();
        Task<ReturnResult> GetUsersnRoles();
        Task<ReturnResult> FillUserPaymentDropdown();
        Task<ReturnResult> FillCampaignDropdown(int id=0);
        Task<ReturnResult> GetUsersWPermissions();
        Task<ReturnResult> GetAdvertCategoryDropDown();
        Task<ReturnResult> GetClientList(int userId = 0);
    }
}
