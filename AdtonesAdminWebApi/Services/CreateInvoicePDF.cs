using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services.Mailer;
//using AdtonesAdminWebApi.ViewModels;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Text;

namespace AdtonesAdminWebApi.Services
{
    public interface ICreateInvoicePDF
    {
        bool CreatePDF(int CampaignProfileId, int BillingId, int AdvertiserId, string InvoiceNumber, int CountryId, int type, string paymentMethod, string currencySymbol1, string fromCurrencyCode, string toCurrencyCode);
    }


    public class CreateInvoicePDF : ICreateInvoicePDF
    {
        private readonly IHttpContextAccessor _httpAccessor;
        private readonly IBillingDAL _billDAL;
        private readonly IConfiguration _configuration;
        private readonly ICurrencyConversion _curConv;
        private readonly ILoggingService _logServ;
        private readonly ISalesManagementDAL _salesMan;
        private readonly ISendEmailMailer _mailer;
        const string PageName = "CreateInvoicePDF(Services)";

        public CreateInvoicePDF(IHttpContextAccessor httpAccessor, 
                                IBillingDAL billDAL,
                                IConfiguration configuration,
                                ICurrencyConversion curConv, ILoggingService logServ, ISalesManagementDAL salesMan,
                                ISendEmailMailer mailer)
        {
            _httpAccessor = httpAccessor;
            _billDAL = billDAL;
            _configuration = configuration;
            _curConv = curConv;
            _logServ = logServ;
            _salesMan = salesMan;
            _mailer = mailer;
        }

        public bool CreatePDF(int CampaignProfileId, int BillingId, int AdvertiserId, string InvoiceNumber, int CountryId, int type, string paymentMethod, string currencySymbol1, string fromCurrencyCode, string toCurrencyCode)
        {
            string path = string.Empty;
            InvoiceDto invoice = new InvoiceDto();
            InvoiceForPDFDto billingDetails = new InvoiceForPDFDto();
            try
            {
                CurrencySymbol currencySymbol = new CurrencySymbol();
                var currencyCode = "";
                currencyCode = currencySymbol1;

                billingDetails = _billDAL.GetInvoiceDetailsForPDF(BillingId).Result;
                AdtonesAdminWebApi.ViewModels.DTOs.Item item1 = new AdtonesAdminWebApi.ViewModels.DTOs.Item();
                item1.Description = billingDetails.CampaignName;
                item1.Price = billingDetails.FundAmount;
                item1.Quantity = 1;
                item1.Organisation = billingDetails.CompanyName;
                CustomerDto customer = new CustomerDto();
                customer.FullName = billingDetails.FullName;
                customer.AddressLine1 = billingDetails.AddressLine1;
                customer.AddressLine2 = billingDetails.AddressLine2;
                customer.City = billingDetails.City;
                customer.Country = billingDetails.InvoiceCountry;
                customer.Postcode = billingDetails.PostCode;
                customer.PhoneNumber = billingDetails.PhoneNumber;
                customer.Email = billingDetails.Email;

                invoice = new InvoiceDto(item1);
                invoice.InvoiceNumber = billingDetails.InvoiceNumber;
                invoice.vat = billingDetails.InvoiceTax / 100;
                invoice.InvoiceTax = billingDetails.InvoiceTax.ToString();
                invoice.InvoiceCountry = billingDetails.ShortName;
                invoice.MethodOfPayment = billingDetails.MethodOfPayment;
                invoice.typeOfPayment = billingDetails.PaymentMethodId;
                invoice.PONumber = billingDetails.PONumber;
                if (billingDetails.SettledDate == null)
                    billingDetails.SettledDate = DateTime.Now;
                if (billingDetails.MethodOfPayment == "Instantpayment")
                    invoice.SettledDate = null;
                else
                {
                    if (billingDetails.MethodOfPayment == "CreditPayment")
                    {
                        if (billingDetails.CreditPeriod == null)
                            invoice.SettledDate = billingDetails.SettledDate.Value.AddDays(7);
                        else
                            invoice.SettledDate = billingDetails.SettledDate.Value.AddDays(billingDetails.CreditPeriod.Value);
                    }
                    else invoice.SettledDate = billingDetails.SettledDate.Value.AddDays(45);
                }
                var otherpath = _configuration.GetValue<string>("AppSettings:adtonesServerDirPath");
                invoice.Imagepath = otherpath + "\\Images\\5acf06fc.png";
                invoice.Customer = customer;
                invoice.Items = 1;
                invoice.CountryId = CountryId;
                invoice.CurrencySymbol = currencyCode;

                GenerateInvoicePDF pdf = new GenerateInvoicePDF(_curConv);
                pdf.Invoice = invoice;
                path = pdf.CreatePDF(otherpath + "/Invoice", fromCurrencyCode, toCurrencyCode);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Payment inserted successfully against Campaign: {CampaignProfileId} BUT there was an issue PRODUCING the invoice No: {InvoiceNumber}";
                _logServ.ErrorMessage = errorMessage + " " + ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CreatePDF";
                _logServ.LogError();
                return false;
            }
            try
            {
                var mail = new SendEmailModel();
                mail.attachmentExt = path;
                mail.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;
                mail.Subject = $"Adtones Invoice: {invoice.InvoiceNumber}  Dated: {DateTime.Parse(billingDetails.SettledDate.ToString()).Day}-{DateTime.Parse(billingDetails.SettledDate.ToString()).Month}-{DateTime.Parse(billingDetails.SettledDate.ToString()).Year}";
                mail.Body = InvoiceTemplate(billingDetails.FullName);
                var mailAddr = string.Empty;
                var ytr = _httpAccessor.GetRoleIdFromJWT();
                if (ytr == (int)Enums.UserRole.SalesExec)
                {
                    mailAddr = _salesMan.GetSalesExecInvoiceMailDets(AdvertiserId).Result;
                    if (mailAddr == null || mailAddr.Length < 3)
                        mailAddr = billingDetails.Email;
                }
                else
                    mailAddr = billingDetails.Email;

                mail.SingleTo = mailAddr;

                _mailer.SendBasicEmail(mail);
                //string[] attachment = new string[1];
                //attachment[0] = path;
                //sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, 
                //    "Adtones Invoice (" + invoice.InvoiceNumber + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 
                //    2, mailto, null, null, attachment, true, DateTime.Now.ToString(), paymentMethod, invoice.SettledDate, invoice.InvoiceNumber);
                return true;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Payment inserted successfully against Campaign: {CampaignProfileId} BUT there was an issue SENDING the invoice No: {InvoiceNumber}";
                _logServ.ErrorMessage = errorMessage + " " + ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "CreatePDF";
                _logServ.LogError();
                return false;
            }
        }


        private string InvoiceTemplate(string fname)
        {
            var sb = new StringBuilder();
            sb.Append("<table width='580' border='0' cellpadding='2' cellspacing='0' style='border-collapse: collapse'>");
            string template = $"<tr><td>Dear {fname},</td></tr>";
            sb.Append(template);
            string template2 = @"<tr>
                                    <td>
                                        Thank you for the campaign payment, please find attached your invoice.
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        Thank you for trusting us with your campaign.
                                    </td>
                                </tr>
          
                                <tr>
                                    <td>
                                        Sincerely,
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Adtones Team<br />
                                        support@adtones.com
                                    </td>
                                </tr>
                            </table>";
            sb.Append(template2);
            return sb.ToString();
        }


    }
}
