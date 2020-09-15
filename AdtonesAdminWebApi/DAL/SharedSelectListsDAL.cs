using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using AdtonesAdminWebApi.Services;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace AdtonesAdminWebApi.DAL.Shared
{
    public class SharedSelectListsDAL : BaseDAL, ISharedSelectListsDAL
    {
        private readonly ISaveGetFiles _getFile;

        public SharedSelectListsDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, ISaveGetFiles getFile,
                        IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        {
            _getFile = getFile;
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetCurrency(int id = 0)
        {
            var sb = new StringBuilder();
            sb.Append(SharedListQuery.GetCurrencyList);

            if (id != 0)
            {
                sb.Append(" WHERE CurrencyId=@Id");
            }
            try
            {
                return await GetSelectList(sb.ToString(), id);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<object>> GetUsersnRoles()
        {
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    return await connection.QueryAsync<object>(SharedListQuery.GetUserwRole);
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<object>> GetUserPermissionsWRoles()
        {
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    return await connection.QueryAsync<object>(SharedListQuery.GetUserPermissionwRole);
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetOperators(int id = 0)
        {
            var sb = new StringBuilder();
            sb.Append(SharedListQuery.GetOperators);

            if (id != 0)
            {
                sb.Append(" AND CountryId = @Id");
            }
            try
            {
                return await GetSelectList(sb.ToString(), id);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetCountry(int id = 0)
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(SharedListQuery.GetCountryList);

            var genFile = string.Empty;
            genFile = GetPermissionsByUserId().Result;

            List<PermissionModel> gen = JsonSerializer.Deserialize<List<PermissionModel>>(genFile);

            var page = gen.Find(u => u.pageName == "GeneralAccess");

            var els = page.elements.ToList();

            var testcty = els.Find(x => x.name == "country").arrayId;

            if (testcty != null)
            {
                int[] country = els.Find(x => x.name == "country").arrayId.ToArray();
                if (country.Length > 0 && id > 0)
                {
                    sb.Append($" WHERE Id=@Id AND Id IN @country ");
                    builder.AddParameters(new { country = country.ToArray() });
                    builder.AddParameters(new { Id = id });
                }
                else if (country.Length > 0 && id == 0)
                {
                    sb.Append(" WHERE Id IN @country ");
                    builder.AddParameters(new { country = country.ToArray() });
                }
            }
            else if (id > 0)
            {
                sb.Append(" WHERE Id=@Id ");
                builder.AddParameters(new { Id = id });
            }

            var select = builder.AddTemplate(sb.ToString());
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    return await connection.QueryAsync<SharedSelectListViewModel>(select.RawSql, select.Parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetCreditUsers(int id = 0)
        {
            try
            {
                return await GetSelectList(SharedListQuery.GetCreditUsers, id);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetCamapignList(int id = 0)
        {
            var sb = new StringBuilder();
            sb.Append(SharedListQuery.GetCampaignList);
            if(id > 0)
            {
                sb.Append(" WHERE UserId = @Id ");
            }

            try
            {
                return await GetSelectList(sb.ToString(), id);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetUserPaymentList(int id = 0)
        {
            try
            {
                return await GetSelectList(SharedListQuery.GetUserPaymentList, id);
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> AddCreditUsers(int id = 0)
        {
            var sb = new StringBuilder();
            sb.Append(SharedListQuery.AddCreditUserList);
            try
            {
                return await GetSelectList(sb.ToString(), id);
            }
            catch
            {
                throw;
            }
        }


        private async Task<IEnumerable<SharedSelectListViewModel>> GetSelectList(string sql, int id=0)
        {
            var sb = new StringBuilder();
            var builder = new SqlBuilder();
            sb.Append(sql);

            if (id != 0)
            {
                builder.AddParameters(new { Id = id });
            }

            var values = CheckGeneralFile(sb, builder,pais:"c",advs:"u");
            sb = values.Item1;
            builder = values.Item2;
            var select = builder.AddTemplate(sb.ToString());
            try
            {
                using (var connection = new SqlConnection(_connStr))
                {
                    connection.Open();
                    return await connection.QueryAsync<SharedSelectListViewModel>(select.RawSql, select.Parameters);
                }
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> GetInvoiceList(int id = 0)
        {
            try
            {
                return await GetSelectList(SharedListQuery.GetInvoiceNumberList, id);
            }
            catch
            {
                throw;
            }
        }


        


        //public async Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectList(string command)
        //{

        //    try
        //    {
        //        var query = await _executers.ExecuteCommand(_connStr,
        //                            conn => conn.Query<SharedSelectListViewModel>(command));
        //        return query;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        //public async Task<IEnumerable<SharedSelectListViewModel>> GetSelectListById(string command, int id = 0)
        //{
        //    var builder = new SqlBuilder();
        //    var select = builder.AddTemplate(command);

        //    if (id != 0)
        //        builder.AddParameters(new { Id = id });

        //    try
        //    {
        //        return await _executers.ExecuteCommand(_connStr,
        //                                conn => conn.Query<SharedSelectListViewModel>(select.RawSql, select.Parameters));
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

    }

}
