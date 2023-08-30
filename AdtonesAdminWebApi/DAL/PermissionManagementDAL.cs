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
using System.Text;

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
                             conn => conn.QueryFirstOrDefault<string>("SELECT Permissions FROM Users WHERE UserId=@UserId", 
                                                                            new { UserId = userId }));
        }


        public async Task<IEnumerable<PermissionChangeModel>> GetPermissionsByRoleId(int[] roles)
        {
            string getUsersPermissionByRole = @"SELECT UserId,Permissions AS permissions FROM Users WHERE RoleId !=2 
                                                                                AND Permissions IS NOT NULL ";
            var builder = new SqlBuilder();
            
            var sb = new StringBuilder();
            sb.Append(getUsersPermissionByRole);
            if (roles.Length > 0)
            {
                sb.Append(" AND RoleId IN @RoleId ");
                builder.AddParameters(new { RoleId = roles.ToArray() });
            }

            var select = builder.AddTemplate(sb.ToString());
            
            return await _executers.ExecuteCommand(_connStr,
                             conn => conn.Query<PermissionChangeModel>(select.RawSql, select.Parameters));
        }


        public async Task<int> UpdateUserPermissions(int userId, string permissions)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate("UPDATE Users SET Permissions=@perms WHERE UserId=@Id");
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


        public async Task<string> GetPermissionsForSelectList(int roleid)
        {
            string permissionsForSelectList = @"SELECT TOP 1 Permissions AS permissions FROM Users WHERE RoleId=@RoleId 
                                                AND Activated=1 AND Permissions IS NOT NULL ORDER BY UserId ASC";
            return await _executers.ExecuteCommand(_connStr,
                             conn => conn.QueryFirstOrDefault<string>(permissionsForSelectList, new { RoleId = roleid }));
        }
    }
}