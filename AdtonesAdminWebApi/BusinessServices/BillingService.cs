using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using Microsoft.Extensions.Configuration;
using AdtonesAdminWebApi.Services.Mailer;
using System.Globalization;
using System.Text;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class BillingService : IBillingService
    {
        private readonly IHttpContextAccessor _httpAccessor;

        private readonly IUserManagementDAL _userDAL;

        private readonly ICurrencyDAL _currencyDAL;
        private readonly ICampaignDAL _campDAL;
        private readonly IBillingDAL _billDAL;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertiserFinancialDAL _userCredit;
        private readonly IAdvertiserFinancialService _userFin;
        private readonly IConfiguration _configuration;
        private readonly ICurrencyConversion _curConv;
        private readonly IConnectionStringService _conService;
        private readonly ILoggingService _logServ;
        private readonly ISalesManagementDAL _salesMan;
        private readonly ISendEmailMailer _mailer;
        private static Random random = new Random();
        ReturnResult result = new ReturnResult();
        const string PageName = "BillingService";

        public BillingService(IHttpContextAccessor httpAccessor, IUserManagementDAL userDAL, ICurrencyDAL currencyDAL, ICampaignDAL campDAL,
                                IBillingDAL billDAL, IAdvertDAL advertDAL, IAdvertiserFinancialDAL userCredit, 
                                IAdvertiserFinancialService userFin, IConfiguration configuration,
                                ICurrencyConversion curConv, IConnectionStringService conService, ILoggingService logServ, ISalesManagementDAL salesMan,
                                ISendEmailMailer mailer)
        {
            _httpAccessor = httpAccessor;
            _userDAL = userDAL;
            _currencyDAL = currencyDAL;
            _campDAL = campDAL;
            _billDAL = billDAL;
            _advertDAL = advertDAL;
            _userCredit = userCredit;
            _userFin = userFin;
            _configuration = configuration;
            _curConv = curConv;
            _conService = conService;
            _logServ = logServ;
            _salesMan = salesMan;
            _mailer = mailer;
        }



        public async Task<ReturnResult> GetPaymentData(int campaignId)
        {
            try
            {
                result.body = await _billDAL.GetCampaignBillingData(campaignId);
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "GetPaymentData";

                await _logServ.LogError();

                result.result = 0;
                result.body = ex.Message.ToString();
                return result;
            }
            return result;
        }


            /// <summary>
            /// This is adapted rfrom the original purely for use with User Credit funding campaigns
            /// so limits the currenecies available purely for either campaign currency or usercredit currency
            /// if they are the same then only that currency can be used.
            /// </summary>
            /// <param name="model"></param>
            /// <returns></returns>
            public async Task<ReturnResult> PaywithUserCredit(BillingPaymentModel model)
        {
            /// Ensure all this handled by front end
            /*
             * if (Convert.ToDecimal(TempData["Fundamount"].ToString()) <= 0)
                {
                    result.body = "Please enter a fundAmount bigger than {0}.";
            result.result = 0;
                    return res;
                }
                if (companydetails.Country == null)
                {
                    TempData["error"] = "Please update your country details before payment.";
                    return RedirectToAction("buy_credit");
                }
                if (companydetails == null)
                {
                    TempData["error"] = "Please update your company details before payment.";
                    return RedirectToAction("buy_credit");
                }
                var contactdetails = _contactRepository.Get(top => top.UserId == efmvcUser.UserId);
                if (contactdetails == null)
                {
                    TempData["error"] = "Please update your contact details before payment.";
                    return RedirectToAction("buy_credit");
                }
                int errorstatus = 0;
                errorstatus = ValidateBilling(errorstatus);
                if (errorstatus == 1) return RedirectToAction("buy_credit");
             * private int ValidateBilling(int errorstatus)
        {
            if (TempData["CampaingId"] == null)
            {
                TempData["error"] = "Please select atleast one campaign";
                errorstatus = 1;
                return errorstatus;
            }
            else
            {
                if (TempData["CampaingId"].ToString() == "0")
                {
                    TempData["error"] = "Please select atleast one campaign";
                    errorstatus = 1;
                    return errorstatus;
                }
            }
            if (TempData["Fundamount"].ToString() == "")
            {
                TempData["error"] = "Please enter fund amount.";
                errorstatus = 1;
                return errorstatus;
            }
            else if (TempData["Fundamount"] == null)
            {
                TempData["error"] = "Please enter valid fund amount.";
                errorstatus = 1;
                return errorstatus;
            }
            else
            {
                if (TempData["Fundamount"].ToString() == "0")
                {
                    TempData["error"] = "Please enter valid fund amount.";
                    errorstatus = 1;
                    return errorstatus;
                }
            }
            return errorstatus;
        }

            //check credit available
                    var CreditAvailable = Convert.ToDecimal(TempData["CreditAvailable"].ToString());
                    var userfundamount = Convert.ToDecimal(TempData["Fundamount"].ToString());
                    if (CreditAvailable == 0)
                    {
                        TempData["error"] = "Credit is not available so please try again.";//Credit is not available.so please try again.
                        return RedirectToAction("buy_credit");
                    }
                    else if (userfundamount > CreditAvailable)
                    {
                        TempData["error"] = "Fund amount is more than credit available";
                        return RedirectToAction("buy_credit");
                    }
            */
            
            try
            {
                model.SettledDate = DateTime.Now;
                var creditPeriod = await _billDAL.GetCreditPeriod(model.CampaignProfileId);
                        if (creditPeriod == 0)
                            model.SettledDate = model.SettledDate.Value.AddDays(7);
                        else
                            model.SettledDate = model.SettledDate.Value.AddDays(creditPeriod);
                    CurrencyModel currencyModel = new CurrencyModel();
                model.UserId = _httpAccessor.GetUserIdFromJWT();
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);
                var connString = await _conService.GetConnectionStringsByCountryId(campaignDetails.CountryId.Value);
                var userCredDetails = await _userCredit.GetUserCreditDetail(model.AdvertiserId);

                // Id from the form selection
                var selectedCurrencyId = model.CurrencyId;

                // Currency details for for selection
                var selectedCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(selectedCurrencyId);
                var usercreditCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(userCredDetails.CurrencyId);
                var selectedCurrencyCountryId = selectedCurrencyData.CountryId;
                
                // As paying with User Credit only selections should be UserCredit and Campaign currencies, in my mind.
                var selectedCurrencyCode = usercreditCurrencyData.CurrencyCode; // selectedCurrencyData.CurrencyCode;
                var campaignCurrencyCode = campaignDetails.CurrencyCode;
                var usercreditCurrencyCode = usercreditCurrencyData.CurrencyCode;

                decimal currencyRate = 1.00M;
                string fromCurrencyCode = selectedCurrencyCode;
                string toCurrencyCode = campaignCurrencyCode;

                model.CurrencyCode = selectedCurrencyCode;

                //var companydetails = await _userDAL.getCompanyDetails(model.AdvertiserId);

                int CountryId = 0;

                    if (campaignDetails.CountryId.Value == 12 || campaignDetails.CountryId.Value == 13 || campaignDetails.CountryId.Value == 14) 
                        CountryId = 12;
                    else if (campaignDetails.CountryId.Value == 11) 
                        CountryId = 8;
                    else 
                        CountryId = campaignDetails.CountryId.Value;

                if (selectedCurrencyCode != campaignCurrencyCode)
                    currencyRate = _curConv.GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode);
                    
                        double TotalAmount = Convert.ToDouble(model.TotalAmount * currencyRate);
                double CreditAvailable = Convert.ToDouble(model.AvailableCredit.ToString());

                        if (TotalAmount > CreditAvailable)
                        {
                            result.body = "Total amount is more than credit available";
                            result.result = 0;
                            return result;
                        }
                
                    model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
                model.PaymentMethodId = 1;
                model.Status = 2;
                model.AdtoneServerBillingId = null;
                model.BillingId = await _billDAL.AddBillingRecord(model);
                if (model.BillingId > 0)
                {
                    //Update Advert And CampaignAdvert
                    int x = 0;
                    var campadvertDetails = await _campDAL.GetCampaignAdvertDetailsById(0, model.CampaignProfileId);
                    if(campadvertDetails == null)
                    {
                        var advertDetails = await _advertDAL.GetAdvertIdByCampid(model.CampaignProfileId);
                        x = await _advertDAL.UpdateAdvertForBilling(advertDetails, connString);
                    }
                    else
                        x = await _advertDAL.UpdateAdvertForBilling(campadvertDetails.AdvertId, connString);

                    var y = await _campDAL.UpdateCampaignMatchesforBilling(model.CampaignProfileId, connString);

                    var creditForm = new AdvertiserCreditFormModel();
                    creditForm.UserId = userCredDetails.UserId;
                    creditForm.AssignCredit = userCredDetails.AssignCredit;
                    creditForm.Id = userCredDetails.Id;
                    creditForm.AvailableCredit = userCredDetails.AvailableCredit - Convert.ToDecimal(TotalAmount.ToString());
                    creditForm.CurrencyId = userCredDetails.CurrencyId;

                    //update credit available of user
                    var creditStatus = await _userFin.AddUserCredit(creditForm);

                    var campaigncreditstatus = _campDAL.UpdateCampaignCredit(model, connString);


                    if (creditStatus.result == 1)
                    {
                        result.body = $"Payment received successfully against Campaign: {campaignDetails.CampaignName} with Invoice Number: {model.InvoiceNumber}";

                        var currencySymbol1 = _curConv.GetCurrencySymbol(campaignCurrencyCode);
                        var pdfStatus = CreatePDF(model, CountryId, 1, "CreditPayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
                        if (pdfStatus) 
                            return result;
                    }
                    else result.body = "Internal Server error. Please try again.";
                    result.result = 0;
                }
                result.body = "Paid with invoice number " + model.InvoiceNumber;
                return result;
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "PayWithUserCredit";
                await _logServ.LogError();
                
                result.result = 0;
                result.body = ex.Message.ToString();
                return result;
            }
        }


        private bool CreatePDF(BillingPaymentModel model, int CountryId, int type, string paymentMethod, string currencySymbol1, string fromCurrencyCode, string toCurrencyCode)
        {
            try
            {
                CurrencySymbol currencySymbol = new CurrencySymbol();
                //string invoiceno = string.Empty;
                //string customername = string.Empty;
                //string companyname = string.Empty;
                //string address = string.Empty;
                //string addaddress = string.Empty;
                //string town = string.Empty;
                //string postcode = string.Empty;
                //string country = string.Empty;
                //string finaladdress = string.Empty;
                //string itemdetails = string.Empty;
                //string methodofPayment = string.Empty;
                //int? typeofPayment;
                //string country_Tax = string.Empty;

                //var billingdetails = _billingRepository.Get(top => top.Id == billingId);
                //invoiceno = model.InvoiceNumber;
                //itemdetails = billingdetails.CampaignProfile.CampaignName;
                //methodofPayment = _paymentMethodRepository.Get(top => top.Id == billingdetails.PaymentMethodId).Description;
                //typeofPayment = model.PaymentMethodId;
                //var userdetails = _userDAL.GetUserById(model.AdvertiserId).Result;
                //customername = userdetails.FirstName + " " + userdetails.LastName;
                //var clientcompany = _userDAL.getCompanyDetails(model.AdvertiserId).Result;
                //companyname = clientcompany.CompanyName;
                //address = clientcompany.Address;
                //addaddress = clientcompany.AdditionalAddress;
                //town = clientcompany.Town;
                //postcode = clientcompany.PostCode;
                //country = clientcompany.Country.Name;
                //var campaignCreditDetails = _campaignCreditPeriodRepository.Get(top => top.UserId == userId && top.CampaignProfileId == billingdetails.CampaignProfile.CampaignProfileId);
                // int campaignId = billingdetails.CampaignProfileId.Value;

                

                // var customercontactinfo = _contactRepository.Get(top => top.UserId == userId);
                var currencyCode = "";
                currencyCode = currencySymbol1;

                var billingDetails = _billDAL.GetInvoiceDetailsForPDF(model.BillingId.Value).Result;
                AdtonesAdminWebApi.ViewModels.Item item1 = new AdtonesAdminWebApi.ViewModels.Item();
                item1.Description = billingDetails.CampaignName;
                item1.Price = billingDetails.FundAmount;
                item1.Quantity = 1;
                item1.Organisation = billingDetails.CompanyName;
                Customer customer = new Customer();
                customer.FullName = billingDetails.FullName;
                customer.AddressLine1 = billingDetails.AddressLine1;
                customer.AddressLine2 = billingDetails.AddressLine2;
                customer.City = billingDetails.City;
                customer.Country = billingDetails.InvoiceCountry;
                customer.Postcode = billingDetails.PostCode;
                customer.PhoneNumber = billingDetails.PhoneNumber;
                customer.Email = billingDetails.Email;
                Invoice invoice = new Invoice();
                invoice = new Invoice(item1);
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
                string path = pdf.CreatePDF(otherpath + "/Invoice", fromCurrencyCode, toCurrencyCode);
                var mail = new SendEmailModel();
                mail.attachmentExt = path;
                mail.From = _configuration.GetSection("AppSettings").GetSection("EmailSettings").GetSection("SiteEmailAddress").Value;
                mail.Subject = $"Adtones Invoice ({invoice.InvoiceNumber}) ({DateTime.Parse(billingDetails.SettledDate.ToString(), new CultureInfo("en-US")).Day}) (" + DateTime.Parse(billingDetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingDetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")";
                mail.Body = InvoiceTemplate(billingDetails.FullName);
                var mailAddr = string.Empty;
                var ytr = _httpAccessor.GetRoleIdFromJWT();
                if (ytr == (int)Enums.UserRole.SalesExec)
                {
                    mailAddr = _salesMan.GetSalesExecInvoiceMailDets(model.AdvertiserId).Result;
                    if (mailAddr == null || mailAddr.Length < 3)
                        mailAddr = billingDetails.Email;
                }
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
                return false;
            }
        }


            private static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
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
