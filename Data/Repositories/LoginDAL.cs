using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using Domain.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{


    public class LoginDAL : ILoginDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IConnectionStringService _connService;

        public LoginDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _connService = connService;
        }


        public async Task<User> GetLoginUser(User userModel)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(LoginQuery.LoginUser);
            builder.AddParameters(new { email = userModel.Email.ToLower() });
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


        public async Task<int> UpdatePassword(User userModel)
        {
            int x = 0;
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(LoginQuery.UpdatePassword);
            builder.AddParameters(new { Email = userModel.Email });
            builder.AddParameters(new { PasswordHash = userModel.PasswordHash });

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

        public async Task<int> UpdateLastLoggedIn(int userId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(LoginQuery.UpdateLoggedIn, new { Id = userId }));
            }
            catch (Exception ex)
            {
                //var msg = ex.Message.ToString();
                throw;
            }
        }


        public async Task<int> UpdateUserLockout(User userModel)
        {
            int x = 0;
            var sb1 = new StringBuilder();
            sb1.Append(LoginQuery.UpdateLockout);
            sb1.Append("UserId=@userId");
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(sb1.ToString());
            builder.AddParameters(new { userId = userModel.UserId });
            builder.AddParameters(new { activated = userModel.Activated });
            builder.AddParameters(new { lockOutTime = userModel.LockOutTime });

            try
            {
                x = await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }

            if (userModel.OperatorId != 0)
            {
                var operatorConnectionString = await _connService.GetConnectionStringByOperator(userModel.OperatorId);
                var sb2 = new StringBuilder();
                sb2.Append(LoginQuery.UpdateLockout);
                sb2.Append("AdtoneServerUserId=@userId");
                var build = new SqlBuilder();
                var sel = build.AddTemplate(sb2.ToString());
                build.AddParameters(new { userId = userModel.UserId });
                build.AddParameters(new { activated = userModel.Activated });
                build.AddParameters(new { lockOutTime = userModel.LockOutTime });

                try
                {
                    x = await _executers.ExecuteCommand(operatorConnectionString,
                                 conn => conn.ExecuteScalar<int>(sel.RawSql, sel.Parameters));
                }
                catch
                {
                    throw;
                }
            }
            return x;
        }


    }
}
