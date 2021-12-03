using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    //public class CampaignAdminResult
    //{
    //    public int UserId { get; set; }
    //    public string Email { get; set; }
    //    public string UserName { get; set; }
    //    public string SalesExec { get; set; }
    //    public int sUserId { get; set; }
    //    public int? ClientId { get; set; }
    //    public string ClientName { get; set; }
    //    public int AdvertId { get; set; }
    //    public string AdvertName { get; set; }
    //    public int CampaignProfileId { get; set; }
    //    public string CampaignName { get; set; }
    //    public string categoryName { get; set; }
    //    public decimal TotalBudget { get; set; }
    //    public int finaltotalplays { get; set; }
    //    public decimal FundsAvailable { get; set; }
    //    public decimal AvgBidValue { get; set; }
    //    public decimal TotalSpend { get; set; }
    //    public int Status { get; set; }
    //    public string rStatus => $"{(Enums.CampaignStatus)Status}";
    //    public DateTime CreatedDate { get; set; }

    //    public int TicketCount { get; set; }
    //    public bool IsAdminApproval { get; set; }
    //    public int CountryId { get; set; }
    //    public int OperatorId { get; set; }
    //    public string OperatorName { get; set; }
    //    public string MobileNumber { get; set; }
    //    public string Organisation { get; set; }
    //    public string CountryName { get; set; }
    //    public string CurrencyCode { get; set; }

    //}


    public class CampaignProfileUpdate
    {
        public int UserId { get; set; }

        public int? ClientId { get; set; }

        public int AdvertId { get; set; }
        public int CampaignProfileId { get; set; }


        public string CampaignName { get; set; }


        public string CampaignDescription { get; set; }


        public decimal TotalBudget { get; set; }


        public float MaxDailyBudget { get; set; }

        public float MaxMonthBudget { get; set; }

        public float MaxWeeklyBudget { get; set; }

        public float MaxHourlyBudget { get; set; }


        public float MaxBid { get; set; }


        public bool Active { get; set; }


        public string EmailSubject { get; set; }


        public string EmailBody { get; set; }

        public int Status { get; set; }

        public string EmailSenderAddress { get; set; }


        public string SmsOriginator { get; set; }

        public string SmsBody { get; set; }
        
        public DateTime UpdatedDateTime { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal FundsAvailable { get; set; }
        public string advertname { get; set; }
        public int NumberInBatch { get; set; }
        public bool IsAdminApproval { get; set; }

        public string CurrencyCode { get; set; }

        public int? CountryId { get; set; }

        public int? OperatorId { get; set; }

        public int? CurrencyId { get; set; }

        public int? CampaignCategoryId { get; set; }

        public decimal MinBid { get; set; }
    }

    //public class CampaignProfile
    //{
    //    public int UserId { get; set; }

    //    public int? ClientId { get; set; }

    //    public int AdvertId { get; set; }
    //    public int CampaignProfileId { get; set; }


    //    public string CampaignName { get; set; }


    //    public string CampaignDescription { get; set; }


    //    public decimal TotalBudget { get; set; }
    //    public decimal TotalCredit { get; set; }

    //    public float MaxDailyBudget { get; set; }

    //    public float MaxMonthBudget { get; set; }

    //    public float MaxWeeklyBudget { get; set; }

    //    public float MaxHourlyBudget { get; set; }


    //    public float MaxBid { get; set; }


    //    public float AvailableCredit { get; set; }


    //    public int PlaysToDate { get; set; }


    //    public int PlaysLastMonth { get; set; }


    //    public int PlaysCurrentMonth { get; set; }


    //    public int CancelledToDate { get; set; }


    //    public int CancelledLastMonth { get; set; }


    //    public int CancelledCurrentMonth { get; set; }


    //    public int SmsToDate { get; set; }


    //    public int SmsLastMonth { get; set; }


    //    public int SmsCurrentMonth { get; set; }


    //    public int EmailToDate { get; set; }


    //    public int EmailsLastMonth { get; set; }


    //    public int EmailsCurrentMonth { get; set; }


    //    public bool Active { get; set; }


    //    public int NumberOfPlays { get; set; }


    //    public int AverageDailyPlays { get; set; }


    //    public int SmsRequests { get; set; }


    //    public int EmailsDelievered { get; set; }


    //    public string EmailSubject { get; set; }


    //    public string EmailBody { get; set; }

    //    public string EmailFileLocation { get; set; }

    //    public string SMSFileLocation { get; set; }
    //    public int Status { get; set; }

    //    public string EmailSenderAddress { get; set; }


    //    public string SmsOriginator { get; set; }

    //    public string SmsBody { get; set; }


    //    public DateTime CreatedDateTime { get; set; }


    //    public DateTime UpdatedDateTime { get; set; }

    //    public DateTime? StartDate { get; set; }
    //    public DateTime? EndDate { get; set; }

    //    public int finaltotalplays { get; set; }

    //    public decimal FundsAvailable { get; set; }
    //    public string advertname { get; set; }
    //    public int NumberInBatch { get; set; }
    //    public bool IsAdminApproval { get; set; }

    //    public decimal totalaveragebid { get; set; }
    //    public decimal totalspend { get; set; }

    //    public string ClientName { get; set; }

    //    public string CurrencyCode { get; set; }

    //    public int? CountryId { get; set; }

    //    public int Reach { get; set; }

    //    public int? CurrencyId { get; set; }

    //    public int OperatorId { get; set; }

    //    public int? CampaignCategoryId { get; set; }
    //}


    public class CampaignBudgetModel
    {
        public float MaxHourlyBudget { get; set; }
        public float MaxBid { get; set; }
    }


    

}
