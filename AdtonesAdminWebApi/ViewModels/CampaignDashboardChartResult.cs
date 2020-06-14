using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.ViewModels
{
    public class CampaignDashboardChartResult
    {
        public string CampaignName { get; set; }
        public string AdvertName { get; set; }

        public decimal TotalBudget { get; set; }
        public double TotalSpend { get; set; }

        public double AverageBid { get; set; }
        public int TotalPlayed { get; set; }
        public int FreePlays { get; set; }
        public double AveragePlayTime { get; set; }
        public decimal MaxPlayLength { get; set; }
        public int TotalReach { get; set; }
        public int Reach { get; set; }
        public string CurrencyCode { get; set; }


        //public double PlaystoDate { get; set; }
        //public double SpendToDate { get; set; }
        //public double FreePlaysPercentage { get; set; }
        //public double TotalBudgetPercentage { get; set; }

        //public double MaxBid { get; set; }

        //public double AvgMaxBid { get; set; }
        //public double MaxBidPercantage { get; set; }
        //public double MaxPlayLengthPercantage { get; set; }
        //public double SMSCost { get; set; }
        //public double EmailCost { get; set; }

        //public double Cancelled { get; set; }
        public CurrencyConvertModel currencyConvertModels { get; set; }

        public CampaignDashboardChartResult()
        {
            currencyConvertModels = new CurrencyConvertModel();
        }
    }


    public class CampaignDashboardChartPREResult
    {
        public string CampaignName { get; set; }
        public string AdvertName { get; set; }
        public decimal Budget { get; set; }
        public double Spend { get; set; }
        public double AvgBid { get; set; }
        public double TotalPlays { get; set; }
        public int MoreSixSecPlays { get; set; }
        public int FreePlays { get; set; }
        public double AvgPlayLength { get; set; }
        public decimal MaxPlayLength { get; set; }
        public int Reach { get; set; }
        public int TotalReach { get; set; }
        public string CurrencyCode { get; set; }

        //public double Available { get; set; }
        //public double FreePlaysPercentage { get; set; }
        //public double TotalBudgetPercentage { get; set; }
        //public double MaxBid { get; set; }
        //public double MaxBidPercantage { get; set; }
        //public double MaxPlayLengthPercantage { get; set; }
        //public int TotalSMS { get; set; }
        //public double TotalSMSCost { get; set; }
        //public int TotalEmail { get; set; }
        //public double TotalEmailCost { get; set; }
        //public double Cancelled { get; set; }
        //public CurrencyConvertModel currencyConvertModels { get; set; }

        //public CampaignDashboardChartPREResult()
        //{
        //    currencyConvertModels = new CurrencyConvertModel();
        //}

    }

}
