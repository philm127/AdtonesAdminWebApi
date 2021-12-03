using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CampaignProfileResult
    {

        public int UserId { get; set; }

        public int? ClientId { get; set; }

        public int AdvertId { get; set; }
        public int CampaignProfileId { get; set; }


        public string CampaignName { get; set; }


        public string CampaignDescription { get; set; }


        public decimal TotalBudget { get; set; }


        public float MaxDailyBudget { get; set; }


        public float MaxBid { get; set; }


        public float AvailableCredit { get; set; }


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


        public bool Active { get; set; }


        public int NumberOfPlays { get; set; }


        public int AverageDailyPlays { get; set; }


        public int SmsRequests { get; set; }


        public int EmailsDelievered { get; set; }


        public string EmailSubject { get; set; }


        public string EmailBody { get; set; }


        public int Status { get; set; }

        public string EmailSenderAddress { get; set; }


        public string SmsOriginator { get; set; }

        public string SmsBody { get; set; }


        public DateTime CreatedDateTime { get; set; }


        public DateTime UpdatedDateTime { get; set; }

        public int finaltotalplays { get; set; }

        public decimal FundsAvailable { get; set; }
        public string advertname { get; set; }
        public int NumberInBatch { get; set; }
        public bool IsAdminApproval { get; set; }

        public int UserMatchedStatus { get; set; }

        public int NumUsersMatched { get; set; }

        //public Client Client { get; set; }
        //public ICollection<AdvertFormModel> Adverts { get; set; }
        //public ICollection<CampaignProfileAdvertFormModel> CampaignProfileAdverts { get; set; }
        //public ICollection<CampaignProfileAttitudeFormModel> CampaignProfileAttitudes { get; set; }
        //public ICollection<CampaignProfileCinemaFormModel> CampaignProfileCinemas { get; set; }
        //public ICollection<CampaignProfileInternetFormModel> CampaignProfileInternets { get; set; }
        //public ICollection<CampaignProfileMobileFormModel> CampaignProfileMobiles { get; set; }
        //public ICollection<CampaignProfilePressFormModel> CampaignProfilePresses { get; set; }
        //public ICollection<CampaignProfileProductsServiceFormModel> CampaignProfileProductsServices { get; set; }
        //public ICollection<CampaignProfileRadioFormModel> CampaignProfileRadios { get; set; }
        //public ICollection<CampaignProfileTimeSettingFormModel> CampaignProfileTimeSettings { get; set; }
        //public ICollection<CampaignProfileTvFormModel> CampaignProfileTvs { get; set; }
        //public ICollection<CampaignAdvertFormModel> CampaignAdverts { get; set; }
        //public ICollection<CampaignProfileDateSettingsFormModel> CampaignProfileDateSettings { get; set; }
        //public ICollection<CampaignProfileDemographicsFormModel> CampaignProfileDemographicsFormModels { get; set; }
        //public ICollection<CampaignAuditFormModel> CampaignAudits { get; set; }

        public decimal totalaveragebid { get; set; }
        public decimal totalspend { get; set; }

        public string ClientName { get; set; }

        public string CurrencyCode { get; set; }

        public int? CountryId { get; set; }

        public int Reach { get; set; }

        public int? CurrencyId { get; set; }
    }

}
