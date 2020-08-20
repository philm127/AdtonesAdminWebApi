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
using Microsoft.AspNetCore.Http;

namespace AdtonesAdminWebApi.DAL
{
   public class PermissionManagementDAL : BaseDAL, IPermissionManagementDAL
    {

        public PermissionManagementDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
            : base(configuration, executers, connService, httpAccessor)
        { }


        /// <summary>
        /// </summary>
        /// <param name="userId">UserId.</param>
        /// <returns></returns>
        public async Task<string> GetPermissionsByUserId(int userId)
        {
            return await _executers.ExecuteCommand(_connStr,
                             conn => conn.QueryFirstOrDefault<string>(PermissionManagementQuery.GetPermissionById, new { UserId = userId }));
        }



        public async Task<int> UpdateUserPermissions(int userId, string permissions)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(PermissionManagementQuery.UpdateUserPermissions);
            builder.AddParameters(new { Id = userId });
            builder.AddParameters(new { perms = permissions });

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
    }
}