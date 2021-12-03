using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class InvoiceForPDF
    {
        public DateTime? SettledDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string InvoiceCountry { get; set; }

        public decimal InvoiceTax { get; set; }
        public string MethodOfPayment { get; set; }
        public int? PaymentMethodId { get; set; }
        public string PONumber { get; set; }

        public string FullName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string ShortName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CampaignName { get; set; }
        public int CampaignProfileId { get; set; }
        public string CompanyName { get; set; }
        public decimal FundAmount { get; set; }
        public int? CreditPeriod { get; set; }
    }
}
