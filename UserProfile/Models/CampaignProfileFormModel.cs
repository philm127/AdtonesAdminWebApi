using System;
using System.Collections.Generic;

namespace UserProfile.Models
{
    public sealed class CampaignProfileFormModel
    {
        public int UserId { get; set; }


        public int? ClientId { get; set; }
        public int? CampaignId { get; set; }
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

        public float MaxMonthBudget { get; set; }
        public float MaxWeeklyBudget { get; set; }
        public float MaxHourlyBudget { get; set; }
        public float TotalCredit { get; set; }

        public float SpendToDate { get; set; }


        public int EmailsDelievered { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        public string EmailFileLocation { get; set; }

        public string SMSFileLocation { get; set; }

        public int Status { get; set; }

        public string EmailSenderAddress { get; set; }

        public string SmsOriginator { get; set; }

        public string SmsBody { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int NumberInBatch { get; set; }
        public int CountryId { get; set; }

        public string CurrencyCode { get; set; }

        public Client Client { get; set; }

        public ICollection<AdvertFormModel> Adverts { get; set; }

        public ICollection<CampaignProfileAdvertFormModel> CampaignProfileAdverts { get; set; }
        public ICollection<CampaignProfileAttitudeFormModel> CampaignProfileAttitudes { get; set; }
        public ICollection<CampaignProfileCinemaFormModel> CampaignProfileCinemas { get; set; }
        public ICollection<CampaignProfileInternetFormModel> CampaignProfileInternets { get; set; }
        public ICollection<CampaignProfileMobileFormModel> CampaignProfileMobiles { get; set; }
        public ICollection<CampaignProfilePressFormModel> CampaignProfilePresses { get; set; }
        public ICollection<CampaignProfileProductsServiceFormModel> CampaignProfileProductsServices { get; set; }
        public ICollection<CampaignProfileRadioFormModel> CampaignProfileRadios { get; set; }
        public ICollection<CampaignProfileTimeSettingFormModel> CampaignProfileTimeSettings { get; set; }
        public ICollection<CampaignProfileTvFormModel> CampaignProfileTvs { get; set; }
        public ICollection<CampaignAdvertFormModel> CampaignAdverts { get; set; }
        public ICollection<CampaignProfileDateSettingsFormModel> CampaignProfileDateSettings { get; set; }
        public ICollection<CampaignProfileDemographicsFormModel> CampaignProfileDemographicsFormModels { get; set; }

        //public ICollection<CampaignAuditFormModel> GetCampaignAudits(ICampaignAuditRepository auditRepo)
        //{
        //    return auditRepo.AsQueryable().Where(a => a.CampaignProfileId == CampaignProfileId).Select(a => Mapper.Map<CampaignAudit, CampaignAuditFormModel>(a)).ToList();
        //}
        //public IQueryable<CampaignAudit> GetDomainCampaignAudits(ICampaignAuditRepository auditRepo)
        //{
        //    return auditRepo.AsQueryable().Where(a => a.CampaignProfileId == CampaignProfileId);
        //}

        //public ICollection<CampaignProfilePreference> CampaignProfilePreferences { get; set; }

        //public ICollection<CampaignAuditFormModel> CampaignAudit2 { get; set; }

        public int CurrencyId { get; set; }
    }
}