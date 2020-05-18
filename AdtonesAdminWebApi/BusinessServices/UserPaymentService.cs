using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class UserPaymentService : IUserPaymentService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();
        private readonly ISharedSelectListsDAL _sharedDal;


        public UserPaymentService(IConfiguration configuration, ISharedSelectListsDAL sharedDal)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
        }


        public async Task<ReturnResult> LoadPaymentDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<InvoiceResult>(InvoiceResultQuery());

                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "LoadDataTable"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }
        
        
        public async Task<ReturnResult> FillCampaignDropdown()
        {
            try
            {
                var select_query = ("SELECT Id AS Value,CampaignNameName AS Text FROM CampaignProfile");
                result.body = await _sharedDal.GetSelectList(select_query);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "FillCampaignDropdown"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;

        }


        public async Task<ReturnResult> FillUserPaymentDropdown()
        {
            try
            {
                var select_query = (@"SELECT UserId AS Value,CONCAT(FirstName,'',LastName,'(',Email,')') AS Text FROM Users
                                        WHERE Activated=1 AND RoleId=3 AND Verification=1");

                result.body = await _sharedDal.GetSelectList(select_query);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "FillUserPaymentDropdown"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> GetOutstandingBalance(int id)
        {
            try
            {
                decimal outstanding = await OutstandingBalance(id);
                if (outstanding == -1)
                    result.result = 0;
                else
                    result.body = outstanding;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "GetOutstandingBalance"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        /// <summary>
        /// Is actually a list of outstanding invoices against a campaign
        /// </summary>
        /// <param name="id">Campaign ProfileId</param>
        /// <returns></returns>
        public async Task<ReturnResult> GetInvoiceDetails(int id)
        {
            try
            {
                decimal outstanding = await OutstandingBalance(id);
                if (outstanding == -1)
                    result.result = 0;

                if (outstanding > 0)
                {
                    var select_query = (@"SELECT Id AS Value,InvoiceNumber AS Text FROM Billing WHERE PaymentMethod=1
                                            AND CampaignProfileId=@Id ORDER BY Id DEC");

                    result.body = await _sharedDal.GetSelectList(select_query,id);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "GetInvoiceDetails"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;

        }


        public async Task<ReturnResult> ReceivePayment(UserCreditPaymentFormModel model)
        {
            try
            {
                model.Status = 1;
                var insert_query = @"INSERT INTO UsersCreditPayment(UserId,BillingId,Amount,Description,CreatedDate,
                                        UpdatedDate,Status,CampaignProfileId)
                                        VALUES(@UserId,@BillingId,@Amount,@Description,@CreatedDate,
                                        GETDATE(),GETDATE(),@CampaignProfileId);";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var upd = await connection.ExecuteAsync(insert_query, model);
                }

                var done = UpdateUserCredit(model.UserId, model.Amount);

                result.body = "Payment received successfully";
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "RecievePayment"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }
        

        private async Task<bool> UpdateUserCredit(int userid, decimal receivedamount)
        {
            var upCredit = new UsersCreditFormModel();

            try
            {
                var select_query = (@"SELECT UserId,AvailableCredit AS Text FROM UsersCredit WHERE UserId=@Id");

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    upCredit = await connection.QueryFirstOrDefaultAsync<UsersCreditFormModel>(select_query, new { Id = userid });
                }

                upCredit.AvailableCredit += receivedamount;

                var update_query = @"UPDATE UsersCredit SET AvailableCredit=@AvailableCredit,UpdatedDate=GETDATE() WHERE UserId=@UserId;";

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    var upd = await connection.ExecuteAsync(update_query, upCredit);
                }
                return true;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "UpdateUserCredit"
                };
                _logging.LogError();
                return false;
            }
        }


        private async Task<decimal> OutstandingBalance(int campaignId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<decimal>(GetOutstandingBalance(), new { Id = campaignId });
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "UserPaymentService",
                    ProcedureName = "OutstandingBalance"
                };
                _logging.LogError();
                return -1;
            }
        }


        #region Long SQL Query


        private string InvoiceResultQuery()
        {
            return @"(SELECT MAX(x.Id) AS Id,x.CampaignProfileId FROM Billing x
	                    JOIN 
	                    (SELECT MAX(p.CampaignProfileId) AS CampaignProfileId FROM Billing p GROUP BY p.CampaignProfileId) y 
	                    ON y.CampaignProfileId = x.CampaignProfileId
	                    GROUP BY x.CampaignProfileId) topper
                    JOIN
                    (
                    SELECT bil.Id,bil.CampaignProfileId,bil.UserId,CONCAT(usr.FirstName,' ',usr.LastName) AS Name,usr.Email,
                    SUM(ISNULL(bil.FundAmount,0)) AS TotalAmount,SUM(ISNULL(ucp.Amount,0)) AS Amount,bil.PaymentDate AS CreatedDate,
                    (SUM(ISNULL(bil.FundAmount,0)) - SUM(ISNULL(ucp.Amount,0))) AS OutstandingAmount,camp.CampaignName,
                    ISNULL(cl.Name,'-') AS ClientName
                    FROM Billing AS bil LEFT JOIN UsersCreditPayment ucp ON bil.CampaignProfileId=ucp.CampaignProfileId
                    LEFt JOIN Users AS usr ON usr.UserId=bil.UserId
                    LEFT JOIN CampaignProfile AS camp ON camp.CampaignProfileId=bil.CampaignProfileId
                    LEFT JOIN Client AS cl on bil.ClientId=cl.Id
                    WHERE bil.PaymentMethodId=1
                    GROUP BY bil.CampaignProfileId,bil.UserId,usr.FirstName,usr.LastName,usr.Email,camp.CampaignName,cl.Name,bil.Id,bil.PaymentDate
                    ) AS bilit
                    ON bilit.Id=topper.Id AND bilit.CampaignProfileId=topper.CampaignProfileId;";
        }


        private string GetOutstandingBalance()
        {
            return @"SELECT ISNULL((bilit.TotalAmount - ISNULL(CAST(payit.Amount AS decimal(18,2)),0)),0) AS OutstandingAmount 
                    FROM
                        (SELECT SUM(ISNULL(bil.FundAmount,0)) AS TotalAmount,bil.CampaignProfileId
                        FROM Billing AS bil
                        WHERE PaymentMethodId=1 AND CampaignProfileId=@Id
                        GROUP BY bil.CampaignProfileId) bilit
                    LEFT JOIN
                        (SELECT SUM(ISNULL(ucp.Amount,0)) AS Amount,ucp.CampaignProfileId
                        FROM UsersCreditPayment ucp
                        WHERE CampaignProfileId=@Id
                        GROUP BY ucp.CampaignProfileId) payit
                    ON payit.CampaignProfileId=bilit.CampaignProfileId";
        }


        #endregion

    }
}
