﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels
{
    public class InvoiceBase
    {
        public int UserId { get; set; }
        public int BillingId { get; set; }
        public string InvoiceNumber { get; set; }
        public string FullName { get; set; }
        public string Organisation { get; set; }
        public string Email { get; set; }
        public string ClientName { get; set; }
        public string CampaignName { get; set; }
        public int CampaignProfileId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string CurrencyCode { get; set; }
        public string SalesExec { get; set; }
        public int SUserId { get; set; }
        public string InvoicePath { get; set; }
    }
    
    public class InvoiceResult : InvoiceBase
    {
        
        public string PONumber { get; set; }
       
        public decimal InvoiceTotal { get; set; }
        // public int Status { get; set; }
       
        public string rStatus { get; set; }

        public DateTime SettledDate { get; set; }

        public string PaymentMethod { get; set; }

        public int UsersCreditPaymentId { get; set; }
    }

    public class OutstandingInvoiceResult : InvoiceBase
    {
        public decimal PaidAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal OutstandingAmount { get; set; }
        
    }
}
