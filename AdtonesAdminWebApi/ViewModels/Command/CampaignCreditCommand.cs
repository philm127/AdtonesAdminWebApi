using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.Command
{
    public class CampaignCreditCommand
    {
        public int CampaignProfileId { get; set; }

        public int MSCampaignProfileId { get; set; }
        public int UserId { get; set; }
        public decimal TotalCredit { get; set; }

        public decimal TotalBudget { get; set; }
        public decimal AvailableCredit { get; set; }
        public int Status { get; set; }
    }
}
