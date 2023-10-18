using DocumentFormat.OpenXml.Spreadsheet;

namespace AdtonesAdminWebApi.Model
{
    public class DailyReportResponse
    {
        public string Date { get; set; }
        public int PaidPlays { get; set; }
        public int FreePlays { get; set; }
        public decimal RevenueGenerated { get; set; }
        public int SubscribersOptedIn { get; set; }
        public int TotalRewards { get; set; }
        public int UsersRewarded { get; set; }
    }
}
