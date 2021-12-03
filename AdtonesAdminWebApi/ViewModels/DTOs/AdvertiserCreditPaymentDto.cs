using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{
    public class AdvertiserCreditPaymentDto
    {
        public int UserId { get; set; }
        //public string Email { get; set; }
        public string FullName { get; set; }
        public int BillingId { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string InvoiceNumber { get; set; }
        public int CampaignProfileId { get; set; }
        public string CampaignName { get; set; }
    }
}
