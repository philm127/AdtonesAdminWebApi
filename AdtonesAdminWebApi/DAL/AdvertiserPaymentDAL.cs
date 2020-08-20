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
using System.Text.Json;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.DAL
{


    public class AdvertiserPaymentDAL : BaseDAL, IAdvertiserPaymentDAL
    {

        public AdvertiserPaymentDAL(IConfiguration configuration, IExecutionCommand executers, IConnectionStringService connService, IHttpContextAccessor httpAccessor)
                        : base(configuration, executers, connService, httpAccessor)
        { }


        


        public async Task<InvoicePDFEmailModel> GetInvoiceToPDF(int billingId, int UsersCreditPaymentID)
        {

            try
            {
                return await _executers.ExecuteCommand(_connStr,
                                conn => conn.QueryFirstOrDefault<InvoicePDFEmailModel>(AdvertiserPaymentQuery.GetInvoiceToPDF, 
                                                                                new { billingId = billingId, ucpId = UsersCreditPaymentID }));
            }
            catch
            {
                throw;
            }
        }


        public async Task<int> InsertPaymentFromUser(AdvertiserCreditFormModel model)
        {

            var builder = new SqlBuilder();
            var select = builder.AddTemplate(AdvertiserPaymentQuery.UpdateUserCreditPayment);
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


        public async Task<int> UpdateUserCredit(AdvertiserCreditFormModel _creditmodel)
        {
            try
            {
                return await _executers.ExecuteCommand(_connStr,
                             conn => conn.ExecuteScalar<int>(AdvertiserPaymentQuery.UpdateUserCredit,
                                                                                    new { Id = _creditmodel.UserId, AssignCredit = _creditmodel.Amount }));
            }
            catch
            {
                throw;
            }
        }

    }
}
