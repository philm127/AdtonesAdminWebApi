using AdtonesAdminWebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{
    public class AdvertiserBillingResultDto
    {
        public int UserId { get; set; }
        public int BillingId { get; set; }
        public string InvoiceNumber { get; set; }
        public string ClientName { get; set; }
        public string CampaignName { get; set; }
        public int CampaignProfileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime SettledDate { get; set; }
        public int Status { get; set; }
        public string rStatus => $"{(BillingStatus)Status}";
        public string CurrencyCode { get; set; }
        public string PaymentMethod { get; set; }
        public string PONumber { get; set; }
        public string InvoicePath { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
