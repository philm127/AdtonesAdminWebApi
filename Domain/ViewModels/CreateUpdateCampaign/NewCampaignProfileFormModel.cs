using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.ViewModels.CreateUpdateCampaign
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
        public int OperatorId { get; set; }

        public bool Clientcheck { get; set; }

        public string PhoneticAlphabet { get; set; }

        public decimal TotalBudget { get; set; }
        public float MaxDailyBudget { get; set; }
        public float MaxBid { get; set; }
        public float MaxMonthBudget { get; set; }
        public float MaxWeeklyBudget { get; set; }
        public float MaxHourlyBudget { get; set; }
        public decimal TotalCredit { get; set; } = 0.00M;
        public float SpendToDate { get; set; } = 0.00F;
        public decimal AvailableCredit { get; set; }
        public int PlaysToDate { get; set; } = 0;
        public int PlaysLastMonth { get; set; } = 0;
        public int PlaysCurrentMonth { get; set; } = 0;
        public int CancelledToDate { get; set; } = 0;
        public int CancelledLastMonth { get; set; } = 0;
        public int CancelledCurrentMonth { get; set; } = 0;
        public int SmsToDate { get; set; } = 0;
        public int SmsLastMonth { get; set; } = 0;
        public int SmsCurrentMonth { get; set; } = 0;
        public int EmailToDate { get; set; } = 0;
        public int EmailsLastMonth { get; set; } = 0;
        public int EmailsCurrentMonth { get; set; } = 0;
        public string EmailFileLocation { get; set; }
        public bool Active { get; set; }
        public int NumberOfPlays { get; set; } = 0;
        public int AverageDailyPlays { get; set; } = 0;
        public int SmsRequests { get; set; } = 0;
        public int EmailsDelievered { get; set; } = 0;
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
        public int NumberInBatch { get; set; } = 1;
        public bool IsAdminApproval { get; set; }
        public float RemainingMaxDailyBudget => MaxDailyBudget;
        public float RemainingMaxHourlyBudget => MaxHourlyBudget;
        public float RemainingMaxWeeklyBudget => MaxWeeklyBudget;
        public float RemainingMaxMonthBudget => MaxMonthBudget;
        public decimal ProvidendSpendAmount { get; set; } = 0.00M;
        public int BucketCount { get; set; } = 0;
        public bool NextStatus { get; set; }

        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public int? CampaignCategoryId { get; set; }

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


    public class NewCampaignProfileFormDataModel : NewCampaignProfileFormModel
    {
        public int MSCampaignProfileId { get; set; }
    }
}
