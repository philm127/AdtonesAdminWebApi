using AdtonesAdminWebApi.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.BusinessServices.Interfaces;
using AdtonesAdminWebApi.ViewModels.Command;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using AdtonesAdminWebApi.Services.Mailer;
using AdtonesAdminWebApi.ViewModels.DTOs;
using AutoMapper;
using DocumentFormat.OpenXml.EMMA;
using System.Collections.Generic;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class BillingService : IBillingService
    {
        private readonly ICreateInvoicePDF _createPDF;


        private readonly IHttpContextAccessor _httpAccessor;

        private readonly IMapper _mapper;

        private readonly ICurrencyDAL _currencyDAL;
        private readonly ICampaignDAL _campDAL;
        private readonly ICampaignMatchDAL _campMatchDAL;
        private readonly IBillingDAL _billDAL;
        private readonly IConfiguration _configuration;
        private readonly IAdvertDAL _advertDAL;
        private readonly ICurrencyConversion _curConv;
        private readonly IConnectionStringService _conService;
        private readonly ILoggingService _logServ;
        private readonly ISendEmailMailer _mailer;
        private readonly IGenerateTicketService _ticketService;
        private readonly ISageCreditCardPaymentService _cardProcessing;
        private readonly IUserCreditService _creditService;
        private readonly IUserCreditDAL _creditDAL;
        private static Random random = new Random();
        ReturnResult result = new ReturnResult();
        const string PageName = "BillingService";

        public BillingService(ICreateInvoicePDF createPDF, IConfiguration configuration, IGenerateTicketService ticketService,
                                IHttpContextAccessor httpAccessor, ICurrencyDAL currencyDAL, ICampaignDAL campDAL, ICampaignMatchDAL campMatchDAL,
                                IBillingDAL billDAL, IAdvertDAL advertDAL, ISendEmailMailer mailer, ISageCreditCardPaymentService cardProcessing,
                                ICurrencyConversion curConv, IConnectionStringService conService, ILoggingService logServ, IMapper mapper,
                                IUserCreditService creditService, IUserCreditDAL creditDAL)
        {
            _mapper = mapper;
            _createPDF = createPDF;
            _configuration = configuration;
            _httpAccessor = httpAccessor;
            _currencyDAL = currencyDAL;
            _campDAL = campDAL;
            _campMatchDAL = campMatchDAL;
            _billDAL = billDAL;
            _advertDAL = advertDAL;
            _curConv = curConv;
            _conService = conService;
            _logServ = logServ;
            _mailer = mailer;
            _ticketService = ticketService;
            _cardProcessing = cardProcessing;
            _creditService = creditService;
            _creditDAL = creditDAL;
        }

        #region PayForService


        /// <summary>
        /// This is adapted rfrom the original purely for use with User Credit funding campaigns
        /// so limits the currenecies available purely for either campaign currency or usercredit currency
        /// if they are the same then only that currency can be used.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ReturnResult> PaywithUserCredit(UserPaymentCommand model)
        {
            // Available credit is reduced as more credit is used for this
            model.AvailableCredit = (model.AvailableCredit - model.Fundamount);

            try
            {
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);
                var connStringList = await _conService.GetConnectionStringsByCountryId(campaignDetails.CountryId.Value);

                model.SettledDate = await SetDettledDate(model.CampaignProfileId);
                
                var currencySymbol1 = _curConv.GetCurrencySymbol(campaignDetails.CurrencyCode);


                    try
                {
                    model = await AddBillingRecord(model, connStringList);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "PayWithUserCredit - AddBillingRecord";
                    await _logServ.LogError();

                    result.result = 0;
                    result.body = ex.Message.ToString();
                    return result;
                }

                try
                {
                    var pdfStatus = await _createPDF.CreatePDF(model.CampaignProfileId, model.BillingId.Value, model.AdvertiserId, model.InvoiceNumber, model.CountryId, 1, "CreditPayment", currencySymbol1, model.CurrencyCode, model.CurrencyCode);
                    if (!pdfStatus)
                    {
                        result.error = "The payments were inserted BUT there was an issue producing the invoice";
                        result.result = 0;
                    }
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


                try
                {
                    //update credit available of user
                    var creditStatus = await _creditDAL.UpdateUserCredit(model.UserId, model.AvailableCredit);
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

                    var campadvertDetails = await _campDAL.GetCampaignAdvertDetailsById(0, model.CampaignProfileId);
                    var x = await _advertDAL.UpdateAdvertForBilling(campadvertDetails.AdvertId, connStringList);

                try
                {
                    var creditCampaignUpdate = UpdateCampaignProfileCredit(model, campaignDetails, connStringList);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "PayWithUserCredit - creditCampaignUpdate";
                    await _logServ.LogError();

                    result.result = 0;
                    result.body = ex.Message.ToString();
                    return result;
                }

                try
                {
                    var creditCampaignMatchUpdate = UpdateCampaignMatchCredit(model, campaignDetails, connStringList);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "PayWithUserCredit - creditCampaignMatchUpdate";
                    await _logServ.LogError();

                    result.result = 0;
                    result.body = ex.Message.ToString();
                    return result;
                }


                


                result.body = $"Paid with invoice number {{ model.InvoiceNumber }} against Campaign: {{campaignDetails.CampaignName}}";
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


        private async Task<DateTime> SetDettledDate(int campaignId)
        {
            var SettledDate = DateTime.Now;
            var creditPeriod = await _billDAL.GetCreditPeriod(campaignId);
            if (creditPeriod == 0)
                SettledDate = SettledDate.AddDays(7);
            else
                SettledDate = SettledDate.AddDays(creditPeriod);
            return SettledDate;
        }

        private async Task<UserPaymentCommand> AddBillingRecord(UserPaymentCommand model, List<string> connStringList)
        {
            model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
            model.PaymentMethodId = 1;
            model.Status = 2;
            model.AdtoneServerBillingId = null;
            model.BillingId = await _billDAL.AddBillingRecord(model, connStringList);
            return model;
        }

        private async Task<int> UpdateCampaignProfileCredit(UserPaymentCommand model,CampaignProfileDto campModel, List<string> connStrings)
        {
            CampaignCreditCommand campCreditModel = new CampaignCreditCommand()
            {
                AvailableCredit = (decimal)model.AvailableCredit,
                TotalBudget = (decimal)campModel.TotalBudget + model.Fundamount,
                CampaignProfileId = campModel.CampaignProfileId,
                Status = (int)Enums.CampaignStatus.Play,
                TotalCredit = (decimal)campModel.TotalCredit + model.TotalAmount
            };

            return await _campDAL.UpdateCampaignCredit(campCreditModel, connStrings);
        }

        private async Task<int> UpdateCampaignMatchCredit(UserPaymentCommand model, CampaignProfileDto campModel, List<string> connStrings)
        {
            CampaignCreditCommand campCreditModel = new CampaignCreditCommand()
            {
                AvailableCredit = (decimal)model.AvailableCredit,
                TotalBudget = (decimal)campModel.TotalBudget + model.Fundamount,
                CampaignProfileId = campModel.CampaignProfileId,
                Status = (int)Enums.CampaignStatus.Play,
                TotalCredit = (decimal)campModel.TotalCredit + model.TotalAmount
            };

            return await _campMatchDAL.UpdateCampaignMatchCredit(campCreditModel, connStrings);
        }


        public async Task<ReturnResult> PaywithSagePayCreditCard(CreditCardPaymentCommand model)
        {
            //UserPaymentCommand userPaymentCommand = new UserPaymentCommand();
            //var cardResult = new CardProcessedResult();
            //try
            //{
            //    var efmvcUser = _httpAccessor.GetUserIdFromJWT();
            //    if (model.AdvertiserId == 0)
            //        model.AdvertiserId = efmvcUser;
            //    if (model.UserId == 0)
            //        model.UserId = efmvcUser;

            //    var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);
            //    var connString = await _conService.GetConnectionStringsByCountryId(campaignDetails.CountryId.Value);

            //    CurrencySymbol currencySymbol = new CurrencySymbol();
            //    CurrencyModel currencyModel = new CurrencyModel();

            //    //check credit available
            //    var CreditAvailable = model.AvailableCredit;
            //    var taxpercantage = model.TaxPercantage;
            //    var totaltaxamount = (model.Fundamount) * (taxpercantage / 100);
            //    var final_amount = model.Fundamount + totaltaxamount;
            //    model.TotalAmount = final_amount;

            //    model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
            //    model.PaymentMethodId = 2;
            //    model.Status = 2;
            //    model.SettledDate = DateTime.Now.AddDays(model.Outstandingdays);


            //    var msg = _cardProcessing.DoDirectSagePaymentCode(model);
            //    if (msg.errors.Length > 0)
            //    {
            //        await _ticketService.CreateAdTicketForBilling(efmvcUser, "Card payment error", msg.errors, 2, model.ClientId, model.CampaignProfileId, model.PaymentMethodId);
            //        result.body = msg;
            //        result.result = 0;
            //        return result;
            //    }
            //    else
            //    {

            //        model.BillingId = await _billDAL.AddBillingRecord(model);
            //        if (model.BillingId > 0)
            //        {
            //            //Update Advert And CampaignAdvert
            //            var campadvertDetails = await _campDAL.GetCampaignAdvertDetailsById(0, model.CampaignProfileId);
            //            var x = await _advertDAL.UpdateAdvertForBilling(campadvertDetails.AdvertId, connString);

            //            var payment = await ReceivePayment(model.BillingId, fundamount, "Card", model.CampaignProfileId);
            //            if (payment != 0)
            //            {
            //                userPaymentCommand = _mapper.Map<UserPaymentCommand>(model);
            //                var campaigncreditstatus = UpdateCampaignCampaignMatchCredit(userPaymentCommand, connString);

            //            }
            //            //if (campaigncreditstatus)
            //            //{
            //            bool tranStatus = AddSageBillingDetails(_model, result.Id);
            //            //}
            //            //var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCountryId(campaignCountryId);
            //            var currencySymbol1 = "XXXXXX";
            //            var pdfStatus = CreatePDF(result.Id, efmvcUser, 2, "Instantpayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
            //            result.body = "Payment received successfully for " + model.InvoiceNumber;

            //        }

            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logServ.ErrorMessage = ex.Message.ToString();
            //    _logServ.StackTrace = ex.StackTrace.ToString();
            //    _logServ.PageName = PageName;
            //    _logServ.ProcedureName = "PayWithSagePayCreditCard";
            //    await _logServ.LogError();

            //    result.result = 0;
            //}
            return result;
        }

        #endregion

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
                InvoicePDFEmailDto pdfModel = new InvoicePDFEmailDto();
                //get billing data

                try
                {
                    pdfModel = await _billDAL.GetInvoiceToPDF(model.billingId, UsersCreditPaymentID);
                }
                catch (Exception ex)
                {
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "SendInvoice-Getdata";
                    await _logServ.LogError();

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
                    _logServ.ErrorMessage = ex.Message.ToString();
                    _logServ.StackTrace = ex.StackTrace.ToString();
                    _logServ.PageName = PageName;
                    _logServ.ProcedureName = "sendemailtoclient - SendEmail";
                    await _logServ.LogError();


                    var msg = ex.Message.ToString();
                    result.result = 0;
                    result.error = "Email failed to send";
                }
            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "SendInvoice";
                await _logServ.LogError();

                result.result = 0;
            }
            return result;
        }


        public async Task<ReturnResult> ReceivePayment(AdvertiserCreditFormCommand model)
        {
            model.Status = 1;

            try
            {
                var x = await _billDAL.InsertPaymentFromUser(model);

            }
            catch (Exception ex)
            {
                _logServ.ErrorMessage = ex.Message.ToString();
                _logServ.StackTrace = ex.StackTrace.ToString();
                _logServ.PageName = PageName;
                _logServ.ProcedureName = "ReceivePayment";
                await _logServ.LogError();

                result.result = 0;
            }

            var updated = await _creditService.AddPaymentToUserCredit(model);

            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="currencyId">From the form selected currency</param>
        /// <returns></returns>
        private async Task<decimal> SelectedAndCampaign(int currencyId, string currencyCode)
        {

            // Currency details for for selection
            var selectedCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(currencyId);
            
            var selectedCurrencyCountryId = selectedCurrencyData.CountryId;

            // As paying with User Credit only selections should be UserCredit and Campaign currencies, in my mind.
            var selectedCurrencyCode = selectedCurrencyData.CurrencyCode; 
            var campaignCurrencyCode = currencyCode;
            

            decimal currencyRate = 1.00M;
            
            currencyRate = _curConv.GetCurrencyRateModel(selectedCurrencyCode, campaignCurrencyCode);

            return currencyRate;
        }


        private async Task<decimal> SelectedAndUsersCredit(int selectedCurrencyId, int userCurrencyId)
        {
            
            var usercreditCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(userCurrencyId);
            var usercreditCurrencyCode = usercreditCurrencyData.CurrencyCode;
            var selectedCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(selectedCurrencyId);

            var selectedCurrencyCountryId = selectedCurrencyData.CountryId;

            decimal currencyRate = 1.00M;

            currencyRate = _curConv.GetCurrencyRateModel(selectedCurrencyData.CurrencyCode, usercreditCurrencyCode);

            return currencyRate;
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




        private static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
