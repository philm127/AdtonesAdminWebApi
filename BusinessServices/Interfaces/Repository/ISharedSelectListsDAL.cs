﻿using Domain.Model;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface ISharedSelectListsDAL
    {
        Task<IEnumerable<SharedSelectListViewModel>> GetCurrency(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetCountry(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetOperators(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetCreditUsers(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> AddCreditUsers(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetCamapignList(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetUserPaymentList(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetInvoiceList(int id = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetOrganisationTypes();
        Task<IEnumerable<object>> GetUserPermissionsWRoles();
        Task<IEnumerable<object>> GetUsersnRoles();
        Task<IEnumerable<SharedSelectListViewModel>> GetAdvertCategory(int countryId);
        Task<IEnumerable<SharedSelectListViewModel>> GetClientList(int userId);
        Task<IEnumerable<SharedSelectListViewModel>> GetCampaignCategory(int countryId = 0);
        Task<IEnumerable<SharedSelectListViewModel>> GetPaymentTypeList();
        Task<IEnumerable<SharedSelectListViewModel>> GetTicketSubjectList();
    }
}
