using AdtonesAdminWebApi.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class InvoiceResult
    {
        public int ID { get; set; }
        public string InvoiceNumber { get; set; }
        public string PONumber { get; set; }

        public int UserId { get; set; }

        public string FullName { get; set; }
        public string Organisation { get; set; }
        public string Email { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }
        // Invoice Date.
        public string CreatedDate { get; set; }

        public decimal InvoiceTotal { get; set; }
        // public int Status { get; set; }
        public int Status { get; set; }
        public string rStatus { get; set; }

        public string SettledDate { get; set; }

        public string PaymentMethod { get; set; }

        public int PaymentMethodId { get; set; }

        public int UsersCreditPaymentID { get; set; }
    }
}
