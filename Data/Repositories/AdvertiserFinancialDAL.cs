using BusinessServices.Interfaces.Repository;
using Data.Repositories.Queries;
using AdtonesAdminWebApi.Services;
using Domain.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Data.Repositories
{


    public class AdvertiserFinancialDAL : BaseDAL, IAdvertiserFinancialDAL
    {

        public AdvertiserFinancialDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, 
                                        IHttpContextAccessor httpAccessor)
                                        : base(configuration, executers, connService, httpAccessor)
        {}


        


        public async Task<InvoicePDFEmailModel> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoicePDFEmailModel>(AdvertiserFinancialQuery.GetInvoiceToPDF, 
                                                                                new { billingId = billingId, ucpId = UsersCreditPaymentID }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<AdvertiserCreditPaymentResult> GetToPayDetails(int billingId)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<AdvertiserCreditPaymentResult>(AdvertiserFinancialQuery.GetPrePaymentDetails,
                                                                                new { billingId = billingId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertPaymentFromUser(AdvertiserCreditFormModel model)
        {

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserFinancialQuery.UpdateUserCreditPayment);
            try
            {
                builder.AddParameters(new { UserId = model.UserId });
                builder.AddParameters(new { BillingId = model.BillingId });
                builder.AddParameters(new { Amount = model.Amount });
                builder.AddParameters(new { Description = model.Description });
                builder.AddParameters(new { Status = model.Status });
                builder.AddParameters(new { CampaignprofileId = model.CampaignProfileId });


                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }




        public async Task<int> AddUserCredit(AdvertiserCreditFormModel _creditmodel)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(AdvertiserFinancialQuery.AddUserCredit, _creditmodel));
            }
            catch
            {
                throw;
            }
        }

        public async Task<int> UpdateInvoiceSettledDate(int billingId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.ExecuteScalar<int>(AdvertiserFinancialQuery.UpdateInvoiceSettledDate,
                                                                                                            new { Id = billingId }));

            }
            catch
            {
                throw;
            }
        }


        public async Task<decimal> GetAvailableCredit(int userId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.QueryFirstOrDefault<decimal>(AdvertiserFinancialQuery.GetAvailableCredit,
                                                                                                               new { Id = userId }));
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
                             conn => conn.ExecuteScalar<int>(AdvertiserFinancialQuery.UpdateUserCredit,
                             new { Id = _creditmodel.Id, AssignCredit = _creditmodel.AssignCredit, AvailableCredit = _creditmodel.AvailableCredit }));
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
                             conn => conn.ExecuteScalar<bool>(AdvertiserFinancialQuery.CheckIfUserExists,
                                                                    new { userId = userId }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<AdvertiserCreditDetailModel> GetUserCreditDetail(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserFinancialQuery.UserCreditDetails);
            try
            {
                builder.AddParameters(new { Id = id });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<AdvertiserCreditDetailModel>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<IEnumerable<CreditPaymentHistory>> GetUserCreditPaymentHistory(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserFinancialQuery.GetPaymentHistory);
            try
            {
                builder.AddParameters(new { userid = id });

                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.Query<CreditPaymentHistory>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<decimal> GetCreditBalance(int id)
        {
            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserFinancialQuery.GetTotalPaymentsByUser);
            builder.AddParameters(new { UserId = id });

            var builder2 = new SqlBuilder();
            var select2 = builder2.AddTemplate(AdvertiserFinancialQuery.GetTotalBilledByUser);
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


        public async Task<decimal> GetCreditBalanceForInvoicePayment(int billingId)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                    conn => conn.QueryFirstOrDefault<decimal>(AdvertiserFinancialQuery.GetOutstandingBalanceInvoice, 
                                                                                                            new { Id = billingId }));

            }
            catch
            {
                throw;
            }
        }



        public async Task<IEnumerable<CampaignAdminResult>> GetCampaignResultSetById(int id)
        {

            var sb = new StringBuilder();
            var builder = new SqlBuilder();

            sb.Append(CampaignQuery.GetCampaignResultSet);
            sb.Append(" WHERE camp.CampaignProfileId=@Id ");
            builder.AddParameters(new { Id = id });


            var select = builder.AddTemplate(sb.ToString());

            try
            {
                // builder.AddParameters(new { siteAddress = _configuration.GetValue<string>("AppSettings:SiteEmailAddress") });

                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.Query<CampaignAdminResult>(select.RawSql, select.Parameters));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertCampaignCredit(CampaignCreditResult model)
        {
            int countryId = 0;
            var campaign = await GetCampaignResultSetById(model.CampaignProfileId);

            try
            {
                foreach (var camp in campaign)
                {
                    countryId = camp.CountryId;
                    model.AdtoneServerCampaignCreditPeriodId = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(AdvertiserFinancialQuery.InsertCampaignCredit, model));

                    var lst = await _connService.GetConnectionStringsByCountry(countryId);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        model.UserId = await _connService.GetUserIdFromAdtoneIdByConnString(model.UserId, constr);
                        model.CampaignProfileId = await _connService.GetCampaignProfileIdFromAdtoneIdByConnString(model.CampaignProfileId, constr);

                        var x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(AdvertiserFinancialQuery.InsertCampaignCredit, model));
                    }
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
            sb.Append(AdvertiserFinancialQuery.UpdateCampaignCredit);
            var sbOp = new StringBuilder();
            sbOp.Append(AdvertiserFinancialQuery.UpdateCampaignCredit);
            sb.Append(" CampaignCreditPeriodId=@Id;");
            sbOp.Append(" AdtoneServerCampaignCreditPeriodId=@Id;");


            try
            {
                var x = await _executers.ExecuteCommand(_connStr,
                                conn => conn.ExecuteScalar<int>(sb.ToString(), new { Id = model.CampaignCreditPeriodId, CreditPeriod = model.CreditPeriod }));


                int countryId = 0;
                var campaign = await GetCampaignResultSetById(model.CampaignProfileId);
                foreach (var camp in campaign)
                {
                    countryId = camp.CountryId;

                    var lst = await _connService.GetConnectionStringsByCountry(countryId);
                    List<string> conns = lst.ToList();

                    foreach (string constr in conns)
                    {
                        x = await _executers.ExecuteCommand(constr,
                                        conn => conn.ExecuteScalar<int>(sbOp.ToString(), new { Id = model.CampaignCreditPeriodId, CreditPeriod = model.CreditPeriod }));
                    }
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
