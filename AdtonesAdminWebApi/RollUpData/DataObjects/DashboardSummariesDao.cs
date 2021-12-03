using System;

namespace AdtonesAdminWebApi.RollUpData.DataObjects
{
    public class DashboardSummariesDao : LeveledStatsObjectDaoBase
    {
        public int? AdvertId { get; set; }
        public int CampaignCountryId { get; set; }
        public int CampaignCurrencyId { get; set; }
        public DateTime LastUpdateDateTimeUtc { get; set; }
        public decimal Budget { get; set; }
        public decimal Spend { get; set; }
        public decimal FundsAvailable { get; set; }
        public decimal AvgBid { get; set; }
        public decimal MaxBid { get; set; }
        public long TotalSMS { get; set; }
        public decimal TotalSMSCost { get; set; }
        public long TotalEmail { get; set; }
        public decimal TotalEmailCost { get; set; }
        public long TotalPlays { get; set; }
        public long MoreSixSecPlays { get; set; }
        public long FreePlays { get; set; }
        public decimal AvgPlayLength { get; set; }
        public long MaxPlayLength { get; set; }
        public long Reach { get; set; }
    }
}