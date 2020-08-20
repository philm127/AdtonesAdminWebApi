using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.DAL.Queries;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertiserCreditDAL : BaseDAL, IAdvertiserCreditDAL
    {
        private readonly ICampaignDAL _campDAL;

        public AdvertiserCreditDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, ICampaignDAL campDAL, 
                            IHttpContextAccessor httpAccessor) : base(configuration, executers, connService, httpAccessor)
        {
            _campDAL = campDAL;
        }


        


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
                             conn => conn.ExecuteScalar<int>(AdvertiserCreditQuery.UpdateUserCredit, 
                                                                                    new { Id = _creditmodel.Id, AssignCredit = _creditmodel.AssignCredit }));
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


        public async Task<int> InsertCampaignCredit(CampaignCreditResult model)
        {
            int countryId = 0;
            var campaign = await _campDAL.GetCampaignResultSetById(model.CampaignProfileId);
            foreach (var camp in campaign)
            {
                countryId = camp.CountryId;
            }

            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(AdvertiserCreditQuery.InsertCampaignCredit, model));


                model.AdtoneServerCampaignCreditPeriodId = x;
                var lst = await _connService.GetConnectionStringsByCountry(countryId);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(AdvertiserCreditQuery.InsertCampaignCredit, model));
                }
            }
            catch
            {
                throw;
            }
            return countryId;
        }


        public async Task<int> UpdateCampaignCredit(CampaignCreditResult model)
        {
            var sb = new StringBuilder();
            sb.Append(AdvertiserCreditQuery.UpdateCampaignCredit);
            var sbOp = new StringBuilder();
            sbOp.Append(AdvertiserCreditQuery.UpdateCampaignCredit);
            sb.Append(" CampaignCreditPeriodId=@Id;");
            sbOp.Append(" AdtoneServerCampaignCreditPeriodId=@Id;");


            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { Id = model.CampaignCreditPeriodId, CreditPeriod = model.CreditPeriod }));


                int countryId = 0;
                var campaign = await _campDAL.GetCampaignResultSetById(model.CampaignProfileId);
                foreach (var camp in campaign)
                {
                    countryId = camp.CountryId;
                }

                var lst = await _connService.GetConnectionStringsByCountry(countryId);
                List<string> conns = lst.ToList();

                foreach (string constr in conns)
                {
                    x = await _executers.ExecuteCommand(constr,
                                    conn => conn.ExecuteScalar<int>(sbOp.ToString(), new { Id = model.CampaignCreditPeriodId, CreditPeriod = model.CreditPeriod }));
                }
            }
            catch
            {
                throw;
            }
            return 0;
        }



    }
}
