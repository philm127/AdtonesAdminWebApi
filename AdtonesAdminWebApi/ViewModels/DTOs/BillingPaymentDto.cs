﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.DTOs
{
    public class BillingPaymentDto
    {
        public int CampaignProfileId { get; set; }
        public int AdvertiserId { get; set; }
        public int UserId { get; set; }
        public decimal AssignedCredit { get; set; }
        public decimal AvailableCredit { get; set; }
        public int CountryId { get; set; }
        public decimal TaxPercantage { get; set; }
        public decimal TotalFundAmount { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public int Outstandingdays { get; set; }
    }
}
