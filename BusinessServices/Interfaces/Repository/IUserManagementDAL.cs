﻿using Domain.Model;
using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Repository;
{
    public interface IUserManagementDAL
    {
        Task<bool> CheckIfUserExists(UserAddFormModel model);
        Task<bool> CheckIfContactExists(Contacts model);
        Task<int> AddNewUser(UserAddFormModel model);
        Task<int> AddNewUserToOperator(UserAddFormModel model);
        Task<int> AddNewContact(Contacts model);
        Task<int> AddNewContactToOperator(Contacts model);
        Task<int> UpdateUserStatus(AdvertiserDashboardResult model);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<int> UpdateCorpUser(string command, int userId);
        Task<Contacts> getContactByUserId(int userId);
        Task<OperatorAdminFormModel> getOperatorAdmin(int userId);
        Task<CompanyDetails> getCompanyDetails(int userId);
        Task<int> UpdateContact(Contacts model);
        Task<int> UpdateUser(User profile);
        Task<int> UpdateUserPermission(IdCollectionViewModel model);
        Task<int> DeleteNewUser(int userId);
        Task<int> InsertManagerToSalesExec(int manId, int execId);
        Task<int> GetOperatorIdByUserId(int userId);
        Task<IEnumerable<string>> GetAdvertOperatorAdmins(int roleId, int operatorId = 0);
        Task<ClientViewModel> GetClientDetails(int clientId);
    }
}
