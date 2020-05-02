﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class UserCreditPaymentFormModel
    {
        public int UserId { get; set; }
        public int BillingId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public int CampaignProfileId { get; set; }
        public int Status { get; set; } = 0;
    }
}
