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
        private readonly string _connStr;
        private readonly ISharedListQuery _commandText;
        private readonly IExecutionCommand _executers;

        public SharedSelectListsDAL(IConfiguration configuration, ISharedListQuery commandText, IExecutionCommand executers)

        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _commandText = commandText;
            _executers = executers;
        }


        //public async Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectList<T>(string sql,dynamic model, int id = 0)
        //{
        //    Type typeParameterType = typeof(T);
        //    var builder = new SqlBuilder();
        //    var select = builder.AddTemplate(sql);
        //    if (id != 0)
        //        builder.AddParameters(new { Id = id });

        //    try
        //    {
        //        using (var connection = new SqlConnection(_connStr))
        //        {
        //            connection.Open();
        //            return await connection.QueryAsync<SharedSelectListViewModel>(select.RawSql, select.Parameters);
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        public async Task<IEnumerable<SharedSelectListViewModel>> GetSelectList(string sql, int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sql);
            if (id != 0)
                builder.AddParameters(new { Id = id });

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


        public async Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectList(string command)
        {

            try
            {
                var query = await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<SharedSelectListViewModel>(command));
                return query;
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<SharedSelectListViewModel>> TESTGetSelectListById(string command, int id = 0)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);

            if (id != 0)
                builder.AddParameters(new { Id = id });

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                        conn => conn.Query<SharedSelectListViewModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }

    }

    public interface ISharedListQuery
    {
        string GetCurrencyList { get; }
        string GetCurrencyListById { get; }
    }


    public class SharedListQuery : ISharedListQuery
    {
        public string GetCurrencyList => "SELECT CurrencyId AS Value,CurrencyCode AS Text FROM Currencies";
        public string GetCurrencyListById => "SELECT CurrencyId AS Value,CurrencyCode AS Text FROM Currencies WHERE CurrencyId=@Id";

    }
}
