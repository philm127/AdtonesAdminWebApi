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

        public string UserName { get; set; }
        public string Organisation { get; set; }
        public string Email { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int CampaignId { get; set; }
        public string CampaignName { get; set; }

        public DateTime InvoiceDateSort { get; set; }
        public string InvoiceDate { get; set; }

        public decimal InvoiceTotal { get; set; }
        // public int Status { get; set; }
        public String status { get; set; }
        public int fstatus { get; set; }

        public DateTime SettledDateSort { get; set; }
        public string SettledDate { get; set; }

        public String MethodOfPayment { get; set; }

        public int PaymentMethodId { get; set; }

        public int UsersCreditPaymentID { get; set; }
    }
}
