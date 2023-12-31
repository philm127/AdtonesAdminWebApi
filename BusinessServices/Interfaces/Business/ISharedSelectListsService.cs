﻿using Domain.Model;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface ISharedSelectListsService
    {
        Task<ReturnResult> GetCountryList();
        ReturnResult GetUserRole();
        ReturnResult GetUserStatus();
        ReturnResult GetTicketStatus();
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
        Task<ReturnResult> GetAdvertCategoryDropDown(int id);
        Task<ReturnResult> GetClientList(int userId = 0);
        Task<ReturnResult> GetCampaignCategoryDropDown(int countryId = 0);
        Task<ReturnResult> FillPaymentTypeDropdown();
        Task<ReturnResult> GetTicketSubjectList();
    }
}
