using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.Command
{
    public class AdvertiserCreditFormCommand
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CurrencyId { get; set; }
        public int CountryId { get; set; }

        public decimal AssignCredit { get; set; }
        public decimal AvailableCredit { get; set; }

        public DateTime UpdatedDate { get; set; }

        public int BillingId { get; set; }
        public decimal Amount { get; set; }
        public decimal OutstandingAmount { get; set; }
        public string Description { get; set; }
        public int CampaignProfileId { get; set; }
        public int Status { get; set; } = 0;
    }
}
