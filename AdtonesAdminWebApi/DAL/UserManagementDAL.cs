using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Model;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
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


        public async Task<int> UpdateUserStatus(string command, AdvertiserDashboardResult model)
        {
            int x = 0;
            int operatorId = 0;

            var sb1 = new StringBuilder();
            sb1.Append(command);
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

            operatorId = _httpAccessor.GetOperatorFromJWT();

            if (operatorId != 0)
            {
                var operatorConnectionString = await _connService.GetSingleConnectionString(operatorId);

                var sb2 = new StringBuilder();
                sb2.Append(command);
                sb2.Append("AdtoneServerUserId=@userId");
                var build = new SqlBuilder();
                var sel = build.AddTemplate(sb2.ToString());
                build.AddParameters(new { userId = model.UserId });
                build.AddParameters(new { Activated = model.Activated });

                try
                {
                    /// TODO: make this live
                    //x = await _executers.ExecuteCommand(operatorConnectionString,
                    //             conn => conn.ExecuteScalar<int>(sel.RawSql, sel.Parameters));
                }
                catch
                {
                    throw;
                }
            }
            return x;

        }


        public async Task<User> GetUserById(string command, int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(command);
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



    }
}