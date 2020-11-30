using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels.CreateUpdateCampaign
{
    public class NewCampaignProfileFormModel
    {
        public int CampaignProfileId { get; set; }

        public int UserId { get; set; }

        public int? ClientId { get; set; }

        public int? CampaignId { get; set; }

        public string CampaignName { get; set; }

        public string CampaignDescription { get; set; }

        public int CountryId { get; set; }

        public bool Clientcheck { get; set; }

        public string PhoneticAlphabet { get; set; }

        public decimal TotalBudget { get; set; }
        public float MaxDailyBudget { get; set; }
        public float MaxBid { get; set; }
        public float MaxMonthBudget { get; set; }
        public float MaxWeeklyBudget { get; set; }
        public float MaxHourlyBudget { get; set; }
        public decimal TotalCredit { get; set; }
        public float SpendToDate { get; set; }
        public decimal AvailableCredit { get; set; }
        public int PlaysToDate { get; set; }
        public int PlaysLastMonth { get; set; }
        public int PlaysCurrentMonth { get; set; }
        public int CancelledToDate { get; set; }
        public int CancelledLastMonth { get; set; }
        public int CancelledCurrentMonth { get; set; }
        public int SmsToDate { get; set; }
        public int SmsLastMonth { get; set; }
        public int SmsCurrentMonth { get; set; }
        public int EmailToDate { get; set; }
        public int EmailsLastMonth { get; set; }
        public int EmailsCurrentMonth { get; set; }
        public string EmailFileLocation { get; set; }
        public bool Active { get; set; }
        public int NumberOfPlays { get; set; }
        public int AverageDailyPlays { get; set; }
        public int SmsRequests { get; set; }
        public int EmailsDelievered { get; set; }
        public string EmailSubject { get; set; }
        public string EmailBody { get; set; }
        public string EmailSenderAddress { get; set; }
        public string SmsOriginator { get; set; }
        public string SmsBody { get; set; }
        public string SMSFileLocation { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime UpdatedDateTime { get; set; }
        public int Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int NumberInBatch { get; set; }
        public bool IsAdminApproval { get; set; }
        public float RemainingMaxDailyBudget { get; set; }
        public float RemainingMaxHourlyBudget { get; set; }
        public float RemainingMaxWeeklyBudget { get; set; }
        public float RemainingMaxMonthBudget { get; set; }
        public decimal ProvidendSpendAmount { get; set; }
        public int BucketCount { get; set; }
        public bool NextStatus { get; set; }

        public string CurrencyCode { get; set; }

        public int? AdtoneServerCampaignProfileId { get; set; }

        public NewClientFormModel newClientFormModel { get; set; }
        public NewBudgetFormModel newBudgetFormModel { get; set; }
        public NewAdvertFormModel newAdvertFormModel { get; set; }
        public NewCampaignDateandInteraction newCampaignDateandInteraction { get; set; }
        public NewAdProfileMappingFormModel newAdProfileMappingFormModel { get; set; }

        public NewCampaignProfileFormModel()
        {
            newClientFormModel = new NewClientFormModel();
            newBudgetFormModel = new NewBudgetFormModel();
            newAdvertFormModel = new NewAdvertFormModel();
            newCampaignDateandInteraction = new NewCampaignDateandInteraction();
            newAdProfileMappingFormModel = new NewAdProfileMappingFormModel();
        }
    }
}
