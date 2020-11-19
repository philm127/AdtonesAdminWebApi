using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class ManagementReportsSearch
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }

        public int[]? operators { get; set; }
        public int? country { get; set; }
        public int? currency { get; set; }

        public int singleOperator { get; set; }
        public string connstring { get; set; }

        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }


    public class SpendCredit
    {
        public int CampaignProfileId { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalCredit { get; set; }
        public string CurrencyCode { get; set; }
    }

    //public class ManagementReportModel
    //{
    //    public int NumOfTextFile { get; set; }
    //    public int NumOfTextLine { get; set; }
    //    public int NumOfUpdateToAudit { get; set; }
    //    public int NumOfPlay { get; set; }
    //    public int NumOfSMS { get; set; }
    //    public int NumOfEmail { get; set; }
    //    public int NumOfTotalUser { get; set; }
    //    public int NumOfUserForever { get; set; }
    //    public int NumOfRemovedUser { get; set; }
    //    public int NumOfLiveCampaign { get; set; }
    //    public int NumberOfAdsProvisioned { get; set; }
    //    public int TotalSpend { get; set; }
    //    public int TotalCredit { get; set; }
    //    public int NumOfCancel { get; set; }
    //    public long TotalPlayLength { get; set; }
    //    public int TotalPlays { get; set; }
    //    public int NumOfPlayUnder6secs { get; set; }

    //    public double AveragePlaysPerUser { get; set; }

    //    public double AveragePlayLength { get; set; }

    //    public string CurrencyCode { get; set; }
    //}

    public class TotalCostCredit
    {
        public double TotalSpend { get; set; }
        public double TotalCredit { get; set; }
    }

    public class CampaignTableManReport
    {
        public int TotOfPlaySixOver { get; set; }
        public int TotOfPlayUnderSix { get; set; }
        public int TotalPlays => TotOfPlaySixOver + TotOfPlayUnderSix;
        public long TotPlaylength { get; set; }
        public int TotOfSMS { get; set; }
        public int TotOfEmail { get; set; }
        public int TotCancelled { get; set; }

        public int NumOfPlaySixOver { get; set; }
        public int NumOfPlayUnderSix { get; set; }
        public int Plays => NumOfPlaySixOver + NumOfPlayUnderSix;
        public long Playlength { get; set; }
        public int NumOfSMS { get; set; }
        public int NumOfEmail { get; set; }
        public int NumCancelled { get; set; }
    }


    public class ManRepUsers
    {
        public int TotalUsers { get; set; }
        public int TotalRemovedUser { get; set; }
        public int AddedUsers { get; set; }
    }


    public class ManagementReportModel
    {
        public int TotalUsers { get; set; }
        public int TotalListened { get; set; }
        public int TotalRemovedUser { get; set; }
        public int AddedUsers { get; set; }
        public int NumListened { get; set; }
        public int TotalSpend { get; set; }
        public int TotalCredit { get; set; }
        public int AmountSpent { get; set; }
        public int AmountCredit { get; set; }
        public int Total6Over { get; set; }
        public int TotalUnder6 { get; set; }
        public int Num6Over { get; set; }
        public int NumUnder6 { get; set; }
        public double TotalAvgPlayLength { get; set; }
        public double NumAvgPlayLength { get; set; }
        public double TotalAvgPlays { get; set; }
        public double TotalAvgPlaysListened { get; set; }
        public double NumAvgPlays { get; set; }
        public double NumAvgPlaysListened { get; set; }
        public int TotalCampaigns { get; set; }
        public int TotalAdverts { get; set; }
        public int TotalCancelled { get; set; }
        public int NumCancelled { get; set; }
        public int CampaignsAdded { get; set; }
        public int AdvertsProvisioned { get; set; }
        public int TotalSMS { get; set; }
        public int TotalEmail { get; set; }
        public int NumSMS { get; set; }
        public int NumEmail { get; set; }
        public long TotalPlayLength { get; set; }
        public long PeriodPlayLength { get; set; }
        public int TotalPlays { get; set; }
        public int NumPlays { get; set; }

        public int TotalRewards { get; set; }
        public int TotRewardUsers { get; set; }
        public int NumRewards { get; set; }
        public int NumRewardUsers { get; set; }
        public decimal TotAvgRewards { get; set; }
        public decimal NumAvgRewards { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class TwoDigitsManRep
    {
        public int TotalItem { get; set; }
        public int NumItem { get; set; }
    }

    public class RewardsManModel
    {
        public int IsRewardReceivedTot { get; set; }
        public int UserProfileIdTot { get; set; }

        public int IsRewardReceivedNum { get; set; }
        public int UserProfileIdNum { get; set; }
    }
}
