using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL.Shared
{
    public class SharedSelectListsDAL : ISharedSelectListsDAL
    {
        private readonly IConfiguration _configuration;


        public SharedSelectListsDAL(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectList<T>(string sql,dynamic model, int id = 0)
        {
            Type typeParameterType = typeof(T);
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sql);
            if (id != 0)
                builder.AddParameters(new { Id = id });

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
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


        public async Task<IEnumerable<SharedSelectListViewModel>> GetSelectList(string sql, int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sql);
            if (id != 0)
                builder.AddParameters(new { Id = id });

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
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


        public async Task<User> GetUserById(string sql, int id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    return await connection.QueryFirstOrDefaultAsync<User>(sql, new { UserId = id });
                }
            }
            catch
            {
                throw;
            }
        }


    }
}
