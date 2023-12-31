﻿using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using Domain.Model;
using AdtonesAdminWebApi.Services;
using Domain.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{

    public class UserManagementDAL : IUserManagementDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IConnectionStringService _connService;

        public UserManagementDAL(IConfiguration configuration, IExecutionCommand executers, IHttpContextAccessor httpAccessor,
                                   IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _httpAccessor = httpAccessor;
            _connService = connService;
        }



        public async Task<bool> CheckIfUserExists(UserAddFormModel model)
        {
            bool exists = false;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                exists = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<bool>(UserManagementQuery.CheckUserExists, new { email = model.Email.ToLower() }));
            }
            return exists;
        }


        public async Task<bool> CheckIfContactExists(Contacts model)
        {
            bool exists = false;

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                exists = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<bool>(UserManagementQuery.CheckContactExists, new { mobile = model.MobileNumber }));
            }
            return exists;
        }


        public async Task<int> AddNewUser(UserAddFormModel model)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(UserManagementQuery.AddNewUser, model));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> GetOperatorIdByUserId(int userId)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(UserManagementQuery.GetOperatorIdFromUserId, new { Id = userId }));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddNewUserToOperator(UserAddFormModel model)
        {
            int x = 0;
            try
            {
                var conns = new List<string>();
                if (model.RoleId == (int)Domain.Enums.UserRole.Admin || model.RoleId == (int)Domain.Enums.UserRole.AdvertAdmin ||
                    model.RoleId == (int)Domain.Enums.UserRole.ProfileAdmin || model.RoleId == (int)Domain.Enums.UserRole.UserAdmin ||
                    model.CountryId == 0)
                    conns = await _connService.GetConnectionStrings();
                else
                    conns = await _connService.GetConnectionStringsByCountry(model.CountryId);


                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                        x = await _executers.ExecuteCommand(constr,
                             conn => conn.ExecuteScalar<int>(UserManagementQuery.AddNewUserToOperator, model));
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddNewContact(Contacts model)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(UserManagementQuery.AddNewContact, model));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> AddNewContactToOperator(Contacts model)
        {
            int x = 0;
            try
            {
                var conns = new List<string>();
                if (model.RoleId == (int)Domain.Enums.UserRole.Admin || model.RoleId == (int)Domain.Enums.UserRole.AdvertAdmin ||
                    model.RoleId == (int)Domain.Enums.UserRole.ProfileAdmin || model.RoleId == (int)Domain.Enums.UserRole.UserAdmin ||
                    model.CountryId == null || model.CountryId.Value == 0)
                    conns = await _connService.GetConnectionStrings();
                else
                    conns = await _connService.GetConnectionStringsByCountry(model.CountryId.Value);


                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                    x = await _executers.ExecuteCommand(constr,
                         conn => conn.ExecuteScalar<int>(UserManagementQuery.AddNewContact, model));
                }
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> UpdateUserStatus(AdvertiserDashboardResult model)
        {
            int x = 0;

            var sb1 = new StringBuilder();
            sb1.Append(UserManagementQuery.UpdateUserStatus);
            sb1.Append("UserId=@userId");

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb1.ToString());
            builder.AddParameters(new { userId = model.UserId });
            builder.AddParameters(new { Activated = model.Activated });

            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }


            var lst = await _connService.GetConnectionStrings();

            var sb2 = new StringBuilder();
            sb2.Append(UserManagementQuery.UpdateUserStatus);
            sb2.Append(" AdtoneServerUserId=@userId");
            var build = new SqlBuilder();
            var sel = build.AddTemplate(sb2.ToString());
            build.AddParameters(new { userId = model.UserId });
            build.AddParameters(new { Activated = model.Activated });

            try
            {
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    if (constr != null && constr.Length > 10)
                    {
                        x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(sel.RawSql, sel.Parameters));
                    }
                }

            }
            catch
            {
                throw;
            }
            return x;
        }


        public async Task<int> UpdateContact(Contacts model)
        {
            int x = 0;
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(UserManagementQuery.UpdateContacts, model));
            }
            catch
            {
                throw;
            }

            return x;
        }


        public async Task<int> UpdateUserPermission(IdCollectionViewModel model)
        {
            var build = new SqlBuilder();
            var sel = build.AddTemplate(UserManagementQuery.UpdateUserPermissions);
            build.AddParameters(new { userId = model.userId });
            build.AddParameters(new { perm = model.permData.ToString() });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(sel.RawSql, sel.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateUser(User profile)
        {
            int x = 0;
            var update_query = new StringBuilder(UserManagementQuery.UpdateUser);

            if (profile.PasswordHash != null && profile.PasswordHash != "")
                update_query.Append(" ,PasswordHash=@passwordHash, LastPasswordChangedDate=GETDATE() ");

            update_query.Append(" WHERE UserId = @UserId");
            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(update_query.ToString(), profile));
            }
            catch
            {
                throw;
            }


            return x;

        }



        // Comes from SoapApiService
        public async Task<int> UpdateCorpUser(string command, int userId)
        {
            int x = 0;

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
            builder.AddParameters(new { Id = userId });

            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }


            return x;

        }


        public async Task<User> GetUserById(int id)
        {
            var sb = new StringBuilder();
            sb.Append(UserManagementQuery.GetUserDetails);
            sb.Append(" UserId=@UserId");
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { UserId = id });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<User>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<User> GetUserByEmail(string email)
        {
            var sb = new StringBuilder();
            sb.Append(UserManagementQuery.GetUserDetails);
            sb.Append(" LOWER(Email)=@email");
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb.ToString());
            builder.AddParameters(new { email = email });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<User>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<Contacts> getContactByUserId(int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserManagementQuery.getContactByUserId);
            builder.AddParameters(new { userid = userId });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<Contacts>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<CompanyDetails> getCompanyDetails(int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserManagementQuery.GetCompanyDetails);
            builder.AddParameters(new { userid = userId });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<CompanyDetails>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        public async Task<OperatorAdminFormModel> getOperatorAdmin(int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserManagementQuery.GetOperatorAdmin);
            builder.AddParameters(new { userId = userId });
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<OperatorAdminFormModel>(select.RawSql, select.Parameters));

            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// If Contact form is not added for instance duplicate mobile number will also remove entry
        /// in main table. Insert into Operator table is done after so don't need to concern with that
        /// table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> DeleteNewUser(int userId)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(UserManagementQuery.DeleteAddedUser);
            builder.AddParameters(new { Id = userId });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                         conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertManagerToSalesExec(int manId, int execId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                     conn => conn.ExecuteScalar<int>(UserManagementQuery.InsertManagerToSalesExec, new { manId = manId, execId = execId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<string>> GetAdvertOperatorAdmins(int roleId, int operatorId = 0)
        {

            var sb1 = new StringBuilder();
            var builder = new SqlBuilder();
            sb1.Append(UserManagementQuery.GetAdvertAdminOperator);

            builder.AddParameters(new { RoleId = roleId });
            if (operatorId > 0)
            {
                sb1.Append(" AND OperatorId=@Id");
                builder.AddParameters(new { Id = operatorId });
            }

            var select = builder.AddTemplate(sb1.ToString());
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                         conn => conn.Query<string>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<ClientViewModel> GetClientDetails(int clientId)
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                    conn => conn.QueryFirstOrDefault<ClientViewModel>(CreateUpdateCampaignQuery.GetClientDetails, new { Id = clientId }));

            }
            catch
            {
                throw;
            }
        }


    }
}