using AdtonesAdminWebApi.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.BusinessServices.Interfaces;

namespace AdtonesAdminWebApi.BusinessServices
{
    public class BillingService : IBillingService
    {
        private readonly ICreateInvoicePDF _createPDF;


        private readonly IHttpContextAccessor _httpAccessor;

        //private readonly IUserManagementDAL _userDAL;

        private readonly ICurrencyDAL _currencyDAL;
        private readonly ICampaignDAL _campDAL;
        private readonly IBillingDAL _billDAL;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertiserFinancialDAL _userCredit;
        private readonly IAdvertiserFinancialService _userFin;
        private readonly ICurrencyConversion _curConv;
        private readonly IConnectionStringService _conService;
        private readonly ILoggingService _logServ;
        private static Random random = new Random();
        ReturnResult result = new ReturnResult();
        const string PageName = "BillingService";

        public BillingService(ICreateInvoicePDF createPDF,
            IHttpContextAccessor httpAccessor, ICurrencyDAL currencyDAL, ICampaignDAL campDAL,
                                IBillingDAL billDAL, IAdvertDAL advertDAL, IAdvertiserFinancialDAL userCredit, 
                                IAdvertiserFinancialService userFin,
                                ICurrencyConversion curConv, IConnectionStringService conService, ILoggingService logServ)
        {
            _createPDF = createPDF;

            _httpAccessor = httpAccessor;
            _currencyDAL = currencyDAL;
            _campDAL = campDAL;
            _billDAL = billDAL;
            _advertDAL = advertDAL;
            _userCredit = userCredit;
            _userFin = userFin;
            _curConv = curConv;
            _conService = conService;
            _logServ = logServ;
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
                    if (campadvertDetails == null)
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
                        var pdfStatus = _createPDF.CreatePDF(model, CountryId, 1, "CreditPayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
                        if (pdfStatus)
                            return result;
                        else
                        {
                            result.error = "The payments were inserted BUT there was an issue producing the invoice";
                            result.result = 0;
                            return result;
                        }
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



        private static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }


        
    }
}
