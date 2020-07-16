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

namespace AdtonesAdminWebApi.DAL.Shared
{
    public class SharedSelectListsDAL : ISharedSelectListsDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;

        public SharedSelectListsDAL(IConfiguration configuration, IExecutionCommand executers)

        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
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
            try
            {
                return await GetSelectList(SharedListQuery.GetCountryList, id);
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
            try
            {
                return await GetSelectList(SharedListQuery.GetCampaignList, id);
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
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sql);

            if (id != 0)
            {
                builder.AddParameters(new { Id = id });
            }
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
                return await GetSelectList(SharedListQuery.GetInvoiceList, id);
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
