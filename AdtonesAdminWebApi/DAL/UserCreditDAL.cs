using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class UserCreditDAL : IUserCreditDAL
    {
        private readonly IConfiguration _configuration;
        private readonly string _connStr;
        private readonly IExecutionCommand _executers;
        private readonly IUserCreditQuery _commandText;

        public UserCreditDAL(IConfiguration configuration, IExecutionCommand executers, IUserCreditQuery commandText)
        {
            _configuration = configuration;
            _connStr = _configuration.GetConnectionString("DefaultConnection");
            _executers = executers;
            _commandText = commandText;
        }


        public async Task<IEnumerable<UserCreditResult>> LoadUserCreditResultSet()
        {
            try
            {

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<UserCreditResult>(_commandText.LoadUserCreditDataTable));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> AddUserCredit(UserCreditFormModel _creditmodel)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(_commandText.AddUserCredit,_creditmodel));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateUserCredit(UserCreditFormModel _creditmodel)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(_commandText.UpdateUserCredit, _creditmodel));
            }
            catch
            {
                throw;
            }
        }


    }
}
