using AdtonesAdminWebApi.ViewModels;
using System;
using System.Collections.Generic;
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
        private readonly IHttpContextAccessor _httpAccessor;

        private readonly IUserManagementDAL _userDAL;

        private readonly ICurrencyDAL _currencyDAL;
        private readonly ICampaignDAL _campDAL;
        private readonly IBillingDAL _billDAL;
        private readonly IAdvertDAL _advertDAL;
        private readonly IAdvertiserFinancialDAL _userCredit;
        private static Random random = new Random();
        ReturnResult result = new ReturnResult();

        public BillingService(IHttpContextAccessor httpAccessor, IUserManagementDAL userDAL, ICurrencyDAL currencyDAL, ICampaignDAL campDAL,
                                IBillingDAL billDAL, IAdvertDAL advertDAL, IAdvertiserFinancialDAL userCredit)
        {
            _httpAccessor = httpAccessor;
            _userDAL = userDAL;
            _currencyDAL = currencyDAL;
            _campDAL = campDAL;
            _billDAL = billDAL;
            _advertDAL = advertDAL;
            _userCredit = userCredit;
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
            int billingId = 0;
            try
            {
                CurrencyModel currencyModel = new CurrencyModel();
                model.UserId = _httpAccessor.GetUserIdFromJWT();
                var operatorId = await _userDAL.GetOperatorIdByUserId(model.AdvertiserId);
                var campaignDetails = await _campDAL.GetCampaignProfileDetail(model.CampaignProfileId);
                var userCredDetails = await _userCredit.GetUserCreditDetail(model.AdvertiserId);

                // Id from the form selection
                var selectedCurrencyId = model.CurrencyId;

                // Currency details for for selection
                var selectedCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(selectedCurrencyId);
                var usercreditCurrencyData = await _currencyDAL.GetCurrencyUsingCurrencyIdAsync(userCredDetails.CurrencyId);
                var selectedCurrencyCountryId = selectedCurrencyData.CountryId;
                var selectedCurrencyCode = selectedCurrencyData.CurrencyCode;
                var campaignCurrencyCode = campaignDetails.CurrencyCode;
                var usercreditCurrencyCode = usercreditCurrencyData.CurrencyCode;

                decimal currencyRate = 0.00M;
                string fromCurrencyCode = selectedCurrencyCode;
                string toCurrencyCode = campaignCurrencyCode;

                //var companydetails = await _userDAL.getCompanyDetails(model.AdvertiserId);

                int CountryId = 0;

                    if (campaignDetails.CountryId.Value == 12 || campaignDetails.CountryId.Value == 13 || campaignDetails.CountryId.Value == 14) 
                        CountryId = 12;
                    else if (campaignDetails.CountryId.Value == 11) 
                        CountryId = 8;
                    else 
                        CountryId = campaignDetails.CountryId.Value;

                //if (selectedCurrencyCountryId != campaignDetails.CountryId)
                //{
                //    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", fromCurrencyCode, toCurrencyCode);
                //    currencyRate = currencyModel.Amount;
                //    if (currencyModel.Code == "OK")
                //    {
                //        double userCreditAvailable = Convert.ToDouble(model.CreditAvailable.ToString());
                //        double CreditAvailable = Convert.ToDouble(model.CreditAvailable.ToString());
                //        double FundAmount = Convert.ToDouble(model.Fundamount * currencyRate);
                //        if (FundAmount > CreditAvailable)
                //        {
                //            result.body = "Total amount is more than credit available";
                //            result.result = 0;
                //            return result;
                //        }
                //        var availableamount = CreditAvailable - FundAmount;
                //        model.Fundamount = Convert.ToDecimal(FundAmount.ToString());
                //        var final_amount = Convert.ToDecimal(FundAmount.ToString()) + totaltaxamount;
                //        _model.TotalAmount = final_amount;
                //    }

                    
                    var finalamount = model.CreditAvailable - model.TotalAmount;
                
                    model.InvoiceNumber = "A" + RandomString(6) + DateTime.Now.ToString("yy");
                model.PaymentMethodId = 1;
                model.Status = 2;
                model.SettledDate = null;
                model.AdtoneServerBillingId = null;
                if (model.ClientId == 0)
                    model.ClientId = null;
                billingId = await _billDAL.AddBillingRecord(model);
                if (billingId > 0)
                {
                    //Update Advert And CampaignAdvert
                    var advertDetails = await _campDAL.GetCampaignAdvertDetailsById(0, model.CampaignProfileId);
                    var x = await _advertDAL.UpdateAdvertForBilling(advertDetails.AdvertId, operatorId);
                    var y = await _campDAL.UpdateCampaignMatchesforBilling(model.CampaignProfileId, operatorId);
                    var creditForm = new AdvertiserCreditFormModel();
                    creditForm.AssignCredit = userCredDetails.AssignCredit;
                    creditForm.Id = userCredDetails.Id;
                    creditForm.AvailableCredit = userCredDetails.AvailableCredit - model.TotalAmount;
                    //if (campaignCurrencyCode == selectedCurrencyCurrency)
                    //{
                    var campaigncreditstatus = _campDAL.UpdateCampaignCredit(model, operatorId);

                    //update credit available of user
                    var creditStatus = await _userCredit.UpdateUserCredit(creditForm);
                    //}
                    //else
                    //{
                    //    currencyModel = _currencyConversion.ForeignCurrencyConversion("1", selectedCurrencyCurrency, campaignCurrencyCode);
                    //    currencyRate = currencyModel.Amount;
                    //    if (currencyModel.Code == "OK")
                    //    {
                    //        double FundAmount = Convert.ToDouble(Convert.ToDecimal(TempData["Fundamount"].ToString()) * currencyRate);
                    //        bool campaigncreditstatus = UpdateCampaignCredit(Convert.ToInt32(TempData["CampaingId"]), decimal.Parse(FundAmount.ToString()));
                    //        if (campaigncreditstatus)
                    //        {
                    //            creditStatus = UpdateUserCredit(efmvcUser.UserId, decimal.Parse(finalamount.ToString()));
                    //        }
                    //        else
                    //        {
                    //            TempData["error"] = "Internal Server error. Please try again.";
                    //        }
                    //    }
                    //}

                    if (creditStatus >= 0)
                    {
                        result.body = "Payment received successfully for " + model.InvoiceNumber;

                        //var currencySymbol1 = currencySymbol.GetCurrencySymbolusingCountryId(campaignCountryId);
                        var currencySymbol1 = Session["CurrencySymbol"].ToString();
                        var pdfStatus = CreatePDF(billingId, efmvcUser.UserId, 1, "CreditPayment", currencySymbol1, fromCurrencyCode, toCurrencyCode);
                        if (pdfStatus) 
                            return result;
                    }
                    else result.body = "Internal Server error. Please try again.";
                    result.result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "BillingService",
                    ProcedureName = "PayWithUserCredit"
                };
                _logging.LogError();
                result.result = 0;
                result.body = ex.Message.ToString();
                return result;
            }
        }


        private bool CreatePDF(int billingId, int userId, int type, string paymentMethod, string currencySymbol1, string fromCurrencyCode, string toCurrencyCode)
        {
            try
            {
                CurrencySymbol currencySymbol = new CurrencySymbol();
                string invoiceno = string.Empty;
                string customername = string.Empty;
                string companyname = string.Empty;
                string address = string.Empty;
                string addaddress = string.Empty;
                string town = string.Empty;
                string postcode = string.Empty;
                string country = string.Empty;
                string finaladdress = string.Empty;
                string itemdetails = string.Empty;
                string methodofPayment = string.Empty;
                int? typeofPayment;
                string country_Tax = string.Empty;

                var billingdetails = _billingRepository.Get(top => top.Id == billingId);
                invoiceno = billingdetails.InvoiceNumber;
                itemdetails = billingdetails.CampaignProfile.CampaignName;
                methodofPayment = _paymentMethodRepository.Get(top => top.Id == billingdetails.PaymentMethodId).Description;
                typeofPayment = billingdetails.PaymentMethodId;
                var userdetails = _userRepository.Get(top => top.UserId == userId);
                customername = userdetails.FirstName + " " + userdetails.LastName;
                var clientcompany = _companydetailsRepository.Get(top => top.UserId == userId);
                companyname = clientcompany.CompanyName;
                address = clientcompany.Address;
                addaddress = clientcompany.AdditionalAddress;
                town = clientcompany.Town;
                postcode = clientcompany.PostCode;
                country = clientcompany.Country.Name;
                var campaignCreditDetails = _campaignCreditPeriodRepository.Get(top => top.UserId == userId && top.CampaignProfileId == billingdetails.CampaignProfile.CampaignProfileId);
                int campaignId = billingdetails.CampaignProfileId.Value;

                int? countryId = 0;
                if (sCampaignProfileId > 0)
                {
                    countryId = _profileRepository.GetById(sCampaignProfileId).CountryId;
                }
                else
                {
                    countryId = _profileRepository.GetById(Convert.ToInt32(TempData["CampaingId"].ToString())).CountryId;
                }
                //var countryId = _currencyRepository.GetById(Convert.ToInt32(Session["currencyId"].ToString())).CountryId;
                int CountryId = 0;
                if (countryId == 12 || countryId == 13 || countryId == 14)
                    CountryId = 12;
                else if (countryId == 11)
                    CountryId = 8;
                else
                    CountryId = Convert.ToInt32(countryId);

                var customercontactinfo = _contactRepository.Get(top => top.UserId == userId);
                var currencyCode = "";
                currencyCode = currencySymbol1;
                EFMVC.Web.EFWebPDF.Item item1 = new EFMVC.Web.EFWebPDF.Item();
                item1.Description = itemdetails;
                item1.Price = billingdetails.FundAmount;
                item1.Quantity = 1;
                item1.Organisation = clientcompany.CompanyName;
                Customer customer = new Customer();
                customer.FullName = customername;
                customer.AddressLine1 = address;
                customer.AddressLine2 = addaddress;
                customer.City = town;
                customer.Country = country;
                customer.Postcode = postcode;
                customer.PhoneNumber = customercontactinfo.PhoneNumber;
                customer.Email = customercontactinfo.Email;
                Invoice invoice = new Invoice();
                invoice = new Invoice(item1);
                invoice.InvoiceNumber = billingdetails.InvoiceNumber;
                invoice.vat = (_countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage) / 100;
                invoice.InvoiceTax = _countryTaxRepository.Get(top => top.CountryId == CountryId).TaxPercantage.ToString();
                invoice.InvoiceCountry = _countryRepository.Get(top => top.Id == CountryId).ShortName;
                invoice.MethodOfPayment = methodofPayment;
                invoice.typeOfPayment = typeofPayment;
                invoice.PONumber = billingdetails.PONumber;
                if (paymentMethod == "Instantpayment") invoice.SettledDate = null;
                else
                {
                    if (paymentMethod == "CreditPayment")
                    {
                        if (campaignCreditDetails == null) invoice.SettledDate = billingdetails.SettledDate.AddDays(7);
                        else invoice.SettledDate = billingdetails.SettledDate.AddDays(campaignCreditDetails.CreditPeriod);
                    }
                    else invoice.SettledDate = billingdetails.SettledDate.AddDays(45);
                }
                invoice.Imagepath = Server.MapPath("~/Images/5acf06fc.png");
                invoice.Customer = customer;
                invoice.Items = 1;
                invoice.CountryId = countryId;
                invoice.CurrencySymbol = currencyCode;

                GeneratePDF pdf = new GeneratePDF(_currencyConversion);
                pdf.Invoice = invoice;
                string path = pdf.CreatePDF(Server.MapPath("~/Invoice"), fromCurrencyCode, toCurrencyCode);

                string[] mailto = new string[1];
                mailto[0] = userdetails.Email;
                string[] attachment = new string[1];
                attachment[0] = path;
                sendemailtoclient(userdetails.Email, userdetails.FirstName, userdetails.LastName, 
                    "Adtones Invoice (" + invoice.InvoiceNumber + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Day + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Month + ") (" + DateTime.Parse(billingdetails.SettledDate.ToString(), new CultureInfo("en-US")).Year + ")", 
                    2, mailto, null, null, attachment, true, DateTime.Now.ToString(), paymentMethod, invoice.SettledDate, invoice.InvoiceNumber);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }



        private async Task UpdateUserCredit(BillingPaymentModel model, AdvertiserCreditDetailModel usercreditDetails)
        {
            if (usercreditDetails != null)
            {
                usercreditDetails.AvailableCredit = usercreditDetails.AvailableCredit - model.TotalAmount;

                
            }


            private static string RandomString(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
