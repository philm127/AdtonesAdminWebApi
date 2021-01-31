using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class BillingPaymentModel
    {
        public int CampaignProfileId { get; set; }
        public int? BillingId { get; set; }
        public int UserId { get; set; }
        public int AdvertiserId { get; set; }
        public int? ClientId { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }
        public decimal AvailableCredit { get; set; }
        public decimal TotalFundAmount { get; set; }
        public decimal AssignedCredit { get; set; }
        public decimal Fundamount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxPercantage { get; set; }
        public int PaymentMethodId { get; set; }
        public DateTime? SettledDate { get; set; }
        public int Status { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }

        public string CardType { get; set; }

        
        public string CardNumber { get; set; }
        
        public string ExpiryMonth { get; set; }
        
        public string ExpiryYear { get; set; }
        
        public string NameOfCard { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string SecurityCode { get; set; }
        
        public string BillingAddress { get; set; }
        public string BillingAddress2 { get; set; }
        
        public string BillingTown { get; set; }

        
        public string BillingPostcode { get; set; }

        public string PaypalTranID { get; set; }
        public int? AdtoneServerBillingId { get; set; }
    }
}
