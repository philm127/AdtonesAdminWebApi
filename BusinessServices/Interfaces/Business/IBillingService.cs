﻿using Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessServices.Interfaces.Business
{
    public interface IBillingService
    {
        Task<ReturnResult> PaywithUserCredit(BillingPaymentModel model);
        Task<ReturnResult> GetPaymentData(int campaignId);
    }
}
