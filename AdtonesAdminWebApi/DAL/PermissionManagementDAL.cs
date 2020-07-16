using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Caching.Memory;
using AdtonesAdminWebApi.ViewModels;

namespace AdtonesAdminWebApi.DAL
{
   public class PermissionManagementDAL : IPermissionManagementDAL
    {
        private readonly IExecutionCommand _executers;
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private IMemoryCache cache;
        public string _dbQuery { get; private set; }

        public PermissionManagementDAL(IExecutionCommand executers,
                            IConfiguration configuration)
        {
            _executers = executers;
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
        }


        /// <summary>
        /// </summary>
        /// <param name="userId">UserId.</param>
        /// <returns></returns>
        public async Task<string> GetPermissionsByUserId(int userId)
        {
            return await _executers.ExecuteCommand(_connStr,
                             conn => conn.QueryFirstOrDefault<string>(PermissionManagementQuery.GetPermissionById, new { UserId = userId }));
        }
    }
}