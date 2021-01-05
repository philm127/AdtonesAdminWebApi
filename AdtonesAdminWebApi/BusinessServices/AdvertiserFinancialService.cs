using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.Services.Mailer;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class AdvertiserFinancialService : IAdvertiserFinancialService
    {
        private readonly IConfiguration _configuration;
        
        private readonly ISharedSelectListsDAL _sharedDal;
        private readonly IAdvertiserFinancialDAL _invDAL;
        private IWebHostEnvironment _env;
        private readonly ISendEmailMailer _mailer;
        private readonly ISaveGetFiles _getFiles;

        ReturnResult result = new ReturnResult();

        public AdvertiserFinancialService(IConfiguration configuration, ISharedSelectListsDAL sharedDal, IAdvertiserFinancialDAL invDAL,
                                           IWebHostEnvironment env, ISaveGetFiles getFiles, ISendEmailMailer mailer)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _invDAL = invDAL;
            _env = env;
            _mailer = mailer;
            _getFiles = getFiles;
        }



        /// <summary>
        /// Starts the sending of an invoice process
        /// </summary>
        /// <param name="model">Uses the IdCollectionViewModel model
        /// billingId and userId are included it uses the ID as UsersCreditPaymentID</param>
        /// <returns></returns>
        public async Task<ReturnResult> SendInvoice(IdCollectionViewModel model)
        {
            try
            {
                int UsersCreditPaymentID = model.id;
                InvoicePDFEmailModel pdfModel = new InvoicePDFEmailModel();
                //get billing data

                try
                {
                    pdfModel = await _invDAL.GetInvoiceToPDF(model.billingId, UsersCreditPaymentID);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "AdvertiserPaymentService",
                        ProcedureName = "SendInvoice-Getdata"
                    };
                    _logging.LogError();
                    result.result = 0;
                }
                var subject = "Adtones Invoice (" + pdfModel.InvoiceNumber + ") " +
                                "(" + DateTime.Parse(pdfModel.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") " +
                                "(" + DateTime.Parse(pdfModel.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") " +
                                "(" + DateTime.Parse(pdfModel.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")";

                /// TODO: When tested sort path and attatchment
                var attachment = "Invoice//Adtones_invoice_" + pdfModel.InvoiceNumber + ".pdf";

                //var attachment = "Invoice/Adtones_invoice_A54928820.pdf";// await _getFiles.GetIformFileFromPath(path); //new FormFile(memory, 0, memory.Length); //File(memory, GetContentType(path), Path.GetFileName(path));

                // Build the body out
                string paymentMethod = pdfModel.Description;
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                otherpath = Path.Combine(otherpath, "MailerTemplates");
                string template = string.Empty;
                var typeOfInv = 0;

                if (paymentMethod.ToLower() == "card")
                {
                    template = "InstantInvoice.html";
                    typeOfInv = 1;
                }
                else
                {
                    template = "Invoice.html";
                    typeOfInv = 2;
                }

                var pathTemp = Path.Combine(otherpath, template);
                string emailContent = string.Empty;
                using (var reader = new StreamReader(pathTemp))
                {
                    emailContent = reader.ReadToEnd();
                }
                string body = string.Empty;

                string link = _configuration.GetValue<string>("AppSettings:adtonesSiteAddress");
                link += "/Billing/buy_credit";

                if (typeOfInv == 1)
                    body = emailContent.Replace("\n", "<br/>").Replace("@Model.Fname", pdfModel.FirstName);
                else if (typeOfInv == 2)
                    body = emailContent.Replace("\n", "<br/>").Replace("@Model.Fname", pdfModel.FirstName)
                                       .Replace("@Model.InvoiceNumber", pdfModel.InvoiceNumber)
                                       .Replace("@Model.PaymentLink", link)
                                       .Replace("@DueDate", string.Format(new CustomDateProvider(), "{0}", pdfModel.SettledDate));

                // Body completed.

                string completedDatetime = DateTime.Now.ToString();

                var emailModel = new SendEmailModel();
                emailModel.SingleTo = pdfModel.Email;
                emailModel.Body = body;
                emailModel.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;
                emailModel.Subject = subject;
                emailModel.isBodyHTML = true;

                if (attachment != null)
                {
                    emailModel.attachment = attachment;
                }


                try
                {
                    await _mailer.SendBasicEmail(emailModel);
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "AdvertiserPaymentService",
                        ProcedureName = "sendemailtoclient - SendEmail"
                    };
                    _logging.LogError();

                    var msg = ex.Message.ToString();
                    result.result = 0;
                    result.error = "Email failed to send";
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserPaymentService",
                    ProcedureName = "SendInvoice"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }



        /// <summary>
        /// Gets the items required to populate the Recieve Payment screen
        /// </summary>
        /// <param name="billingId"></param>
        /// <returns>A body containing AdvertiserCreditPaymentResult</returns>
        public async Task<ReturnResult> GetToPayDetails(int billingId)
        {
            try
            {
                var details = await _invDAL.GetToPayDetails(billingId);
                details.OutstandingAmount = await _invDAL.GetCreditBalanceForInvoicePayment(billingId);
                result.body = details;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserFinancialService",
                    ProcedureName = "GetToPayDetails"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }
            

        public async Task<ReturnResult> ReceivePayment(AdvertiserCreditFormModel model)
        {
            model.Status = 1;
            
            try
            {
                var x = await _invDAL.InsertPaymentFromUser(model);

            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserPaymentService",
                    ProcedureName = "ReceivePayment"
                };
                _logging.LogError();
                result.result = 0;
            }

            var updated = await AddCredit(model);

            //if (model.Amount >= model.OutstandingAmount)
            //{
            //    int x = await _invDAL.UpdateInvoiceSettledDate(model.BillingId);
            //}

            return result;
        }



        #region Advertiser Credit


        /// <summary>
        /// Used for both Adding New Credit and Updating Existing Credit
        /// </summary>
        /// <param name="_usercredit"></param>
        /// <returns></returns>
        public async Task<ReturnResult> AddCredit(AdvertiserCreditFormModel _usercredit)
        {
            var newModel = new AdvertiserCreditFormModel();
            try
            {
                int x = 0;
                var creditDetails = await _invDAL.GetUserCreditDetail(_usercredit.UserId);

                if (creditDetails != null)
                {
                    var available = creditDetails.AvailableCredit + _usercredit.Amount;
                    
                    _usercredit.Id = creditDetails.Id;
                    if (available > creditDetails.AssignCredit)
                        _usercredit.AvailableCredit = creditDetails.AssignCredit;
                    else
                        _usercredit.AvailableCredit = creditDetails.AvailableCredit + _usercredit.Amount;
                    x = await _invDAL.UpdateUserCredit(_usercredit);
                }
                else
                {
                    x = await _invDAL.AddUserCredit(_usercredit);
                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertisersCreditService",
                    ProcedureName = "AddCredit"
                };
                _logging.LogError();
                result.result = 0;
                result.error = "Credit was not added successfully";
                return result;
            }
            result.body = "Assigned credit successfully";
            return result;
        }


        /// <summary>
        /// Populates the User Credit Details Screen
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> GetCreditDetails(int id)
        {
            string select_query = string.Empty;

            try
            {
                var creddet = await _invDAL.GetUserCreditDetail(id);
                var credhist = await _invDAL.GetUserCreditPaymentHistory(id);
                creddet.PaymentHistory = credhist.ToList();

                result.body = creddet;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertisersCreditService",
                    ProcedureName = "CreditDetails"
                };
                _logging.LogError();
                result.result = 0;
                return result;
            }
            return result;
        }


        public async Task<ReturnResult> UpdateCampaignCredit(CampaignCreditResult model)
        {
            try
            {
                // Need to do this to get OperatorId
                result.body = await _invDAL.UpdateCampaignCredit(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserPaymentService",
                    ProcedureName = "UpdateCampaignCredit"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> AddCampaignCredit(CampaignCreditResult model)
        {
            try
            {
                // Need to do this to get OperatorId
                result.body = await _invDAL.InsertCampaignCredit(model);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "AdvertiserPaymentService",
                    ProcedureName = "UpdateCampaignCredit"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }



        /// <summary>
        /// Calculates the available credit from the User Credit Details screen
        /// </summary>
        /// <param name="id">Passed the UserId</param>
        /// <returns>The available credit</returns>
        //private async Task<decimal> CalculateNewCredit(int userId)
        //{
        //    decimal available = 0.0000M;

        //    try
        //    {
        //        available = await _invDAL.GetCreditBalance(userId);
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "AdvertisersCreditService",
        //            ProcedureName = "CalculateNewCredit"
        //        };
        //        _logging.LogError();
        //        throw;
        //    }

        //    return available;
        //}



        #endregion




        //public async Task<ReturnResult> GetOutstandingBalance(int id)
        //{
        //    try
        //    {
        //        decimal outstanding = await OutstandingBalance(id);
        //        if (outstanding == -1)
        //            result.result = 0;
        //        else
        //            result.body = outstanding;
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "AdvertiserFinancialService",
        //            ProcedureName = "GetOutstandingBalance"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //    }
        //    return result;
        //}


        ///// <summary>
        ///// Is actually a list of outstanding invoices against a campaign
        ///// </summary>
        ///// <param name="id">Campaign ProfileId</param>
        ///// <returns></returns>
        //public async Task<ReturnResult> GetInvoiceDetails(int id)
        //{
        //    try
        //    {
        //        decimal outstanding = await OutstandingBalance(id);
        //        if (outstanding == -1)
        //            result.result = 0;

        //        if (outstanding > 0)
        //        {
        //            result.body = await _sharedDal.GetInvoiceList(id);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "AdvertiserFinancialService",
        //            ProcedureName = "GetInvoiceDetails"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //    }
        //    return result;

        //}


        //private async Task<decimal> OutstandingBalance(int billingId)
        //{
        //    try
        //    {
        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            await connection.OpenAsync();
        //            return await connection.QueryFirstOrDefaultAsync<decimal>(GetOutstandingBalance(), new { Id = billingId });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "AdvertiserFinancialService",
        //            ProcedureName = "OutstandingBalance"
        //        };
        //        _logging.LogError();
        //        return -1;
        //    }
        //}


        ///// <summary>
        ///// Updates FROM the User Credit Details screen
        ///// </summary>
        ///// <param name="_creditmodel"></param>
        ///// <returns></returns>
        //public async Task<ReturnResult> UpdateCredit(UsersCreditFormModel _creditmodel)
        //{
        //    try
        //    {
        //        _creditmodel.AvailableCredit = await CalculateNewCredit(_creditmodel.UserId);

        //        var update_query = @"UPDATE UsersCredit SET UserId = @UserId,AssignCredit = @AssignCredit,AvailableCredit = @AvailableCredit,
        //                                                                                 UpdatedDate = GETDATE(),CurrencyId = @CurrencyId
        //                                                                                 WHERE Id = @Id";


        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            await connection.OpenAsync();
        //            result.body = await connection.ExecuteAsync(update_query, _creditmodel);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        var _logging = new ErrorLogging()
        //        {
        //            ErrorMessage = ex.Message.ToString(),
        //            StackTrace = ex.StackTrace.ToString(),
        //            PageName = "UserCreditService",
        //            ProcedureName = "UpdateCredit"
        //        };
        //        _logging.LogError();
        //        result.result = 0;
        //    }
        //    return result;

        //}


        //private string GetOutstandingBalance()
        //{
        //    string sql = @"SELECT ISNULL((bilit.TotalAmount - ISNULL(CAST(payit.Amount AS decimal(18,2)),0)),0) AS OutstandingAmount 
        //            FROM
        //                (SELECT ISNULL(CAST(bil.TotalAmount AS decimal(18,2)),0) AS TotalAmount,Id  
        //                FROM Billing AS bil
        //                WHERE bil.Id=@Id) bilit
        //            LEFT JOIN
        //                (SELECT SUM(ISNULL(CAST(ucp.Amount AS decimal(18,2)),0)) AS Amount,BillingId  
        //                FROM UsersCreditPayment ucp
        //                WHERE BillingId=@Id
        //                GROUP BY BillingId) payit
        //            ON payit.BillingId=bilit.Id";

        //    return sql;
        //}


    }
}
