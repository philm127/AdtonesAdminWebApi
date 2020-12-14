using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign
{
    public class NewBudgetFormModel
    {
        public string MonthlyBudget { get; set; }

        public string WeeklyBudget { get; set; }

        public string DailyBudget { get; set; }

        public string HourlyBudget { get; set; }

        public string MaximumBid { get; set; }

        public int CurrencyId { get; set; }
    }
}