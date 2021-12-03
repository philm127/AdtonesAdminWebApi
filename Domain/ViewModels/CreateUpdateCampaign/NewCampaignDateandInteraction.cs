using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign
{
    public class NewCampaignDateandInteraction
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string SmsOriginator { get; set; }

        public string SmsBody { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }
    }
}
