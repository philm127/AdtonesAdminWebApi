using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.Command
{
    public class CampaignCreditPeriodCommand
    {
        public int CampaignCreditPeriodId { get; set; }
        public int UserId { get; set; }
        public int CampaignProfileId { get; set; }
        public int CreditPeriod { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? AdtoneServerCampaignCreditPeriodId { get; set; }
    }
}
