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
    public class AdvertiserPaymentService : IAdvertiserPaymentService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();
        private readonly ISharedSelectListsDAL _sharedDal;
        private readonly IAdvertiserPaymentDAL _invDAL;
        private readonly IAdvertiserCreditService _credService;
        private IWebHostEnvironment _env;
        private readonly ISendEmailMailer _mailer;
        private readonly ISaveGetFiles _getFiles;

        public AdvertiserPaymentService(IConfiguration configuration, ISharedSelectListsDAL sharedDal, IAdvertiserPaymentDAL invDAL,
                                           IAdvertiserCreditService credService, IWebHostEnvironment env,
                                ISaveGetFiles getFiles, ISendEmailMailer mailer)

        {
            _configuration = configuration;
            _sharedDal = sharedDal;
            _invDAL = invDAL;
            _credService = credService;
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
                var attachment = "Invoice/Adtones_invoice_" + pdfModel.InvoiceNumber + ".pdf";

                //var attachment = "Invoice/Adtones_invoice_A54928820.pdf";// await _getFiles.GetIformFileFromPath(path); //new FormFile(memory, 0, memory.Length); //File(memory, GetContentType(path), Path.GetFileName(path));

                // Build the body out
                string paymentMethod = pdfModel.Description;
                var otherpath = _env.ContentRootPath;
                otherpath = Path.Combine(otherpath, "MailerTemplates\\");
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

                var pathTemp = otherpath + template;
                var reader = new StreamReader(pathTemp);

                string emailContent = reader.ReadToEnd();
                string body = string.Empty;

                string link = _configuration.GetSection("AppSettings").GetSection("adtonesSiteAddress").Value;
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
                    await _mailer.SendEmail(emailModel);
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
                    result.body = await _sharedDal.GetInvoiceList(id);
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


        public async Task<ReturnResult> ReceivePayment(AdvertiserCreditFormModel model)
        {
            model.Status = 1;
            try
            {
                var x = await _invDAL.InsertPaymentFromUser(model);
                var updated = await _invDAL.UpdateUserCredit(model);
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

            return result;
        }




        private async Task<decimal> OutstandingBalance(int billingId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    return await connection.QueryFirstOrDefaultAsync<decimal>(GetOutstandingBalance(), new { Id = billingId });
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
