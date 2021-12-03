using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum QuestionSubjectStatus
    {
        Technical = 1,
        Billing = 2,
        Adreview = 3,
        OperatorAdreview = 4,
        CampaignSupport = 5,
        CreditRequest = 6,
        OperatorCreditRequest = 7,
        RewardsAndCredits = 8,
        InappropriateContent = 9,
        AdvertError = 10,
        Other = 11
    }
}
