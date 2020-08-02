using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using DocumentFormat.OpenXml.Office2010.Drawing;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertiserCreditDAL : BaseDAL, IAdvertiserCreditDAL
    {

        public AdvertiserCreditDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService) 
            : base(configuration, executers, connService)
        { }


        


        public async Task<int> AddUserCredit(AdvertiserCreditFormModel _creditmodel)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(AdvertiserCreditQuery.AddUserCredit,_creditmodel));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> UpdateUserCredit(AdvertiserCreditFormModel _creditmodel)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(AdvertiserCreditQuery.UpdateUserCredit, _creditmodel));
            }
            catch
            {
                throw;
            }
        }


        public async Task<bool> CheckUserCreditExist(int userId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<bool>(AdvertiserCreditQuery.CheckIfUserExists,
                                                                    new {userId = userId}));
            }
            catch
            {
                throw;
            }
        }


        public async Task<AdvertiserCreditFormModel> GetUserCreditDetail(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserCreditQuery.UserCreditDetails);
            try
            {
                builder.AddParameters(new { Id = id });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<AdvertiserCreditFormModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<decimal> GetCreditBalance(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserCreditQuery.GetTotalPaymentsByUser);
            builder.AddParameters(new { UserId = id });

            var builder2 = new SqlBuilder();
            var select2 = builder2.AddTemplate(AdvertiserCreditQuery.GetTotalBilledByUser);
            builder2.AddParameters(new { UserId = id });
            try
            {
                Task<decimal> credit = _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<decimal>(select.RawSql, select.Parameters));

                Task<decimal> bill = _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<decimal>(select2.RawSql, select2.Parameters));

                await Task.WhenAll(credit, bill);

                return credit.Result - bill.Result;
            }
            catch
            {
                throw;
            }
        }

        
    }
}
