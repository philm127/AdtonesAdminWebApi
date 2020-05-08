
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.Services.Mailer;
using AdtonesAdminWebApi.ViewModels;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IConfiguration _configuration;
        ReturnResult result = new ReturnResult();


        public InvoiceService(IConfiguration configuration)

        {
            _configuration = configuration;
        }


        /// <summary>
        /// Populate the datatable
        /// </summary>
        /// <returns></returns>
        public async Task<ReturnResult> LoadInvoiceDataTable()
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    result.body = await connection.QueryAsync<InvoiceResult>(GetInvoiceResultSet());

                }
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "InvoiceService",
                    ProcedureName = "LoadData"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
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
                    using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                    {
                        await connection.OpenAsync();
                        pdfModel = await connection.QueryFirstOrDefaultAsync<InvoicePDFEmailModel>(GetInvoiceToPDF(), new { billingId = model.billingId, ucpId = UsersCreditPaymentID });
                    }
                }
                catch (Exception ex)
                {
                    var _logging = new ErrorLogging()
                    {
                        ErrorMessage = ex.Message.ToString(),
                        StackTrace = ex.StackTrace.ToString(),
                        PageName = "InvoiceService",
                        ProcedureName = "SendInvoice-Getdata"
                    };
                    _logging.LogError();
                    result.result = 0;
                }

                string[] mailto = new string[1];
                mailto[0] = pdfModel.Email;
                string[] attachment = new string[1];
                /// TODO: Attach to core def of file management
                attachment[0] = "";// Server.MapPath("~/Invoice/Adtones_invoice_" + pdfModel.InvoiceNumber + ".pdf");

                string[] ParamNames = new string[] { "dotnetesting@gmail.com" };
                sendemailtoclient(pdfModel.Email, pdfModel.FirstName, pdfModel.LastName,
                    "Adtones Invoice (" + pdfModel.InvoiceNumber + ") " +
                    "(" + DateTime.Parse(pdfModel.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") " +
                    "(" + DateTime.Parse(pdfModel.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") " +
                    "(" + DateTime.Parse(pdfModel.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 2,
                    ParamNames, null, null, attachment, true, DateTime.Now.ToString(), pdfModel.Description, pdfModel.SettledDate,
                    pdfModel.InvoiceNumber);
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "InvoiceService",
                    ProcedureName = "SendInvoice"
                };
                _logging.LogError();
                result.result = 0;
            }
            return result;
        }

        /// TODO: Sort out standardised email servive
        private void sendemailtoclient(string to, string fname, string lname, string subject, int formatId, string[] mailTo, string[] mailCC,
                                        string[] mailBcc, string[] attachment, bool isBodyHTML, string completedDatetime,
                                        string paymentMethod, DateTime? dueDate, string InvoiceNumber)
        {


            var EmailContent = new SendEmailModel();
            if (mailTo != null)
            {
                EmailContent.To = mailTo;
            }
            if (mailCC != null)
            {
                EmailContent.CC = mailCC;
            }
            if (mailBcc != null)
            {
                EmailContent.Bcc = mailBcc;
            }
            if (attachment != null)
            {
                EmailContent.attachment = attachment;
            }
            /// TODO: Sort config get
            // EmailContent.Link = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Admin/UserManagement/Index";
            EmailContent.isBodyHTML = isBodyHTML;
            EmailContent.Fname = fname;
            EmailContent.Lname = lname;
            EmailContent.Subject = subject;
            EmailContent.FormatId = formatId;
            EmailContent.InvoiceNumber = InvoiceNumber;
            EmailContent.CompletedDatetime = completedDatetime;

            EmailContent.PaymentMethod = paymentMethod;
            if (paymentMethod == "Card")
            {
                EmailContent.PaymentMethod = "Instantpayment";
            }
            if (EmailContent.PaymentMethod == "Instantpayment")
            {
                EmailContent.DueDate = null;
            }
            else
            {
                EmailContent.DueDate = dueDate;
                /// TODO: Sort config get
                // EmailContent.PaymentLink = ConfigurationManager.AppSettings["siteAddress"].ToString() + "Billing/buy_credit";
            }
            /// TODO: Sort Email send
            // sendEmailMailer.SendEmail(EmailContent);
        }


#region Longer SQL Query


        private string GetInvoiceResultSet()
        {
            string select_query = @"SELECT bil.Id,bil.InvoiceNumber,bil.PONumber,bil.ClientId,ISNULL(cl.Name,'-') AS ClientName,
                    ucp.CampaignProfileId as CampaignId,camp.CampaignName,bil.PaymentDate AS InvoiceDate,
                    ucp.Amount as InvoiceTotal,(Case WHEN bil.Status=3 THEN 'Fail' ELSE 'Paid' END) as status,bil.SettledDate,
                    (CASE WHEN bil.PaymentMethodId=1 THEN 'Cheque' ELSE pay.Description END) AS MethodOfPayment,
                    bil.PaymentMethodId,bil.Status as fstatus,ucp.UserId,CONCAT(usr.FirstName,' ',usr.LastName) as UserName,
                    usr.Email,ucp.Id AS UsersCreditPaymentID, ISNULL(usr.Organisation, '-') AS Organisation
                    FROM UsersCreditPayment AS ucp 
                    LEFT JOIN Billing AS bil ON ucp.BillingId=bil.Id 
                    LEFT JOIN Client AS cl ON bil.ClientId=cl.Id
                    LEFT JOIN CampaignProfile camp ON camp.CampaignProfileId=ucp.CampaignProfileId
                    LEFT JOIN PaymentMethod AS pay ON bil.PaymentMethodId=pay.Id
                    LEFT JOIN Users AS usr ON ucp.UserId=usr.UserId;";

            return select_query;
        }


        private string GetInvoiceToPDF()
        {
            string select_query = @"SELECT pay.Description,bil.InvoiceNumber,bil.SettledDate,co.CountryId,ucp.Amount,usr.Email,
                        usr.FirstName, usr.LastName
                        FROM Billing AS bil 
                        INNER JOIN PaymentMethod pay ON bil.PaymentMethodId=pay.Id
                        INNER JOIN Users usr ON bil.UserId=usr.UserId
                        LEFT JOIN CompanyDetails co ON usr.UserId=co.UserId,
                        UsersCreditPayment ucp
                        WHERE bil.Id=@billingId AND ucp.Id=@ucpId;";

            return select_query;
        }


        #endregion
    }
}
