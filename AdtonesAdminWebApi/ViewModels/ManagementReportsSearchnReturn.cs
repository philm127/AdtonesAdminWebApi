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
    }


    public class SpendCredit
    {
        public int CampaignProfileId { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalCredit { get; set; }
        public string CurrencyCode { get; set; }
    }

    public class ManagementReportModel
    {
        public int NumOfTextFile { get; set; }
        public int NumOfTextLine { get; set; }
        public int NumOfUpdateToAudit { get; set; }
        public int NumOfPlay { get; set; }
        public int NumOfSMS { get; set; }
        public int NumOfEmail { get; set; }
        public int NumOfTotalUser { get; set; }
        public int NumOfRemovedUser { get; set; }
        public int NumOfLiveCampaign { get; set; }
        public int NumberOfAdsProvisioned { get; set; }
        public double TotalSpend { get; set; }
        public double TotalCredit { get; set; }
        public int NumOfCancel { get; set; }

        public int NumOfPlayUnder6secs { get; set; }

        public double AveragePlaysPerUser { get; set; }
    }

    public class TotalCostCredit
    {
        public double TotalSpend { get; set; }
        public double TotalCredit { get; set; }
    }
}
