using AdtonesAdminWebApi.ViewModels.Command;
using AdtonesAdminWebApi.ViewModels.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serialization.Json;
using System;
using System.Collections.Generic;
using com.paypal.sdk.profiles;
using com.paypal.sdk.services;
using PayPal;


namespace AdtonesAdminWebApi.Services
{
    public interface ISageCreditCardPaymentService
    {
        CardProcessedResult DoDirectSagePaymentCode(CreditCardPaymentCommand model);
    }

    public class SageCreditCardPaymentService : ISageCreditCardPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly ICurrencyConversion _curConv;
        public SageCreditCardPaymentService(IConfiguration configuration, ICurrencyConversion curConv)
        {
            _configuration = configuration;
            _curConv = curConv;
        }


        public CardProcessedResult DoDirectSagePaymentCode(CreditCardPaymentCommand model)
        {
            //NVPCallerServices caller = new NVPCallerServices();
            IAPIProfile profile = ProfileFactory.createSignatureAPIProfile();

            var cardResult = new CardProcessedResult();
            CurrencyModel currencyModel = new CurrencyModel();
            var authorization = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("authorization").Value;
            var vendorName = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("vendorName").Value;
            var sagePayCurrency = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("sagepaycurrency").Value;
            var sagepaycountry = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("sagepaycountry").Value;
            var transactionType = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("transactionType").Value;
            var merchantSessionKeys = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("MerchantSessionKeys").Value;
            var cardIdentifiers = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("CardIdentifiers").Value;
            var transactions = _configuration.GetSection("AppSettings").GetSection("SageSettings").GetSection("Transactions").Value;
            decimal finalAmount = 0.00M;
            decimal currencyRate = 0.00M;
            //string fromCurrencyCode = currencyCode;
            string toCurrencyCode = sagePayCurrency;
            string fromCurrencyCode = model.CurrencyCode;
            //string toCurrencyCode = currencyCode;
            if (toCurrencyCode == fromCurrencyCode)
            {
                finalAmount = model.TotalFundAmount * 100;
            }
            else
            {
                currencyRate = _curConv.GetCurrencyRateModel(fromCurrencyCode, toCurrencyCode);
                finalAmount = Math.Round((model.TotalFundAmount * currencyRate), 2);
                finalAmount = finalAmount * 100;
            }

            var deserial = new JsonDeserializer();
            IRestResponse response1;
            IRestResponse response2;
            var client = new RestClient(merchantSessionKeys);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", authorization);
            request.AddParameter("application/json", "{\r\n\t\"vendorName\":\"" + vendorName + "\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic jsonResponse = JsonConvert.DeserializeObject(response.Content);
            var OfferJ = deserial.Deserialize<List<Card>>(response);
            if (response.StatusCode.ToString() == "Created")
            {
                client = new RestClient(cardIdentifiers);
                request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("authorization", "Bearer " + OfferJ[0].MerchantSessionKey.ToString());
                request.AddHeader("content-type", "application/json");
                request.AddParameter("application/json", "{\r\n  \"cardDetails\": {\r\n    \"cardholderName\": \"" + model.FirstName + ' ' + model.LastName + "\",\r\n    \"cardNumber\": \"" + model.CardNumber + "\",\r\n    \"expiryDate\": \"" + model.ExpiryMonth + model.ExpiryYear.Substring(2, 2) + "\",\r\n    \"securityCode\": \"" + model.SecurityCode + "\"\r\n  }\r\n}", ParameterType.RequestBody);
                response1 = client.Execute(request);
                dynamic jsonResponse1 = JsonConvert.DeserializeObject(response1.Content);
                var OfferJ1 = deserial.Deserialize<List<CardIdentifiers>>(response1);
                if (response1.StatusCode.ToString() == "Created")
                {
                    Random randNo = new Random();
                    long randnum2 = (long)(randNo.NextDouble() * 9000000000) + 1000000000;
                    client = new RestClient(transactions);
                    request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("authorization", authorization);
                    request.AddHeader("content-type", "application/json");
                    var billingAddress = model.BillingAddress.Contains("\r\n") == true ? model.BillingAddress.Replace("\r\n", " ") : model.BillingAddress;
                    request.AddParameter("application/json", "{\r\n    \"paymentMethod\": {\r\n     \"card\": {\r\n      \"merchantSessionKey\":\"" + OfferJ[0].MerchantSessionKey.ToString() + "\",\r\n      \"cardIdentifier\":\"" + OfferJ1[0].CardIdentifier.ToString() + "\"\r\n     }\r\n    },\r\n    \"transactionType\":\"" + transactionType + "\",\r\n    \"vendorTxCode\":\"adtoneslimited-" + randnum2.ToString() + "\",\r\n    \"amount\":" + Convert.ToInt32(finalAmount) + ",\r\n    \"currency\":\"" + sagePayCurrency + "\",\r\n    \"customerFirstName\":\"" + model.FirstName + "\",\r\n    \"customerLastName\":\"" + model.LastName + "\",\r\n    \"billingAddress\":{\r\n        \"address1\":\"" + billingAddress + "\",\r\n        \"city\":\"" + model.BillingTown + "\",\r\n        \"postalCode\":\"" + model.BillingPostcode + "\",\r\n        \"country\":\"" + sagepaycountry + "\"\r\n    },\r\n    \"entryMethod\":\"Ecommerce\",\r\n    \"apply3DSecure\":\"Disable\",\r\n    \"applyAvsCvcCheck\":\"Disable\",\r\n    \"description\":\"Testing\",\r\n    \"customerEmail\":\"" + model.Email + "\",\r\n    \"customerPhone\":\"" + model.PhoneNumber + "\",\r\n    \"shippingDetails\":{\r\n        \"recipientFirstName\":\"" + model.FirstName + "\",\r\n        \"recipientLastName\":\"" + model.LastName + "\",\r\n        \"shippingAddress1\":\"" + billingAddress + "\",\r\n        \"shippingCity\":\"" + model.BillingTown + "\",\r\n        \"shippingPostalCode\":\"" + model.BillingPostcode + "\",\r\n        \"shippingCountry\":\"GB\"\r\n    }\r\n}\r\n", ParameterType.RequestBody);
                    response2 = client.Execute(request);
                    var finalstatus = deserial.Deserialize<List<Output>>(response2);
                    if (response2.StatusCode.ToString() == "Created")
                    {
                        cardResult.TransactionId = finalstatus[0].TransactionId;
                        cardResult.clientMessage = "Payment received successfully for " + model.InvoiceNumber;
                        return cardResult;
                    }
                    else
                    {
                        cardResult.errors = finalstatus[0].StatusDetail;
                        return cardResult;
                    }
                }
                else
                {
                    cardResult.errors = jsonResponse1.errors[0].clientMessage;
                    return cardResult;
                }
            }
            else
            {
                cardResult.errors = jsonResponse.errors[0].clientMessage;
                cardResult.errorCode = jsonResponse.code.ToString();
                return cardResult;
            }
        }

    }
}
